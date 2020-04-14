using System;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;

namespace ImmuneAndReadyToHelp.Core.Services
{
    public class AzureBlobLogoStorage : ILogoStorage
    {
        private CloudBlobClient BlobClient { get; set; }
        public string ContainerName { get; set; }


        public AzureBlobLogoStorage(IConfiguration configuration)
        {
            ContainerName = configuration["Azure:BlobStorage:LogoContainerName"];
            var blobConnectionString = configuration["Azure:BlobStorage:LogoConnectionString"];
            BlobClient = CloudStorageAccount.Parse(blobConnectionString).CreateCloudBlobClient();
        }

        public async Task<Uri> UploadToBlob(Stream stream, string containerName, string blobName)
        {
            var container = await GetBlobContainer();
            var blockBlob = container.GetBlockBlobReference(blobName);
            stream.Position = 0;
            await blockBlob.UploadFromStreamAsync(stream);
            var blob = await GetBlobReference(containerName, blobName);
            return blob.Uri;
        }

        public async Task DeleteLogo(Uri uri)
        {
            var blob = await GetBlobReference(uri);
            await blob.DeleteAsync();
        }

        private async Task<ICloudBlob> GetBlobReference(Uri uri)
            => await BlobClient.GetBlobReferenceFromServerAsync(uri);

        private async Task<ICloudBlob> GetBlobReference(string containerName, string blobName)
        {
            var container = BlobClient.GetContainerReference(containerName);
            return await container.GetBlobReferenceFromServerAsync(blobName);
        }

        private async Task<CloudBlobContainer> GetBlobContainer()
        {
            var container = BlobClient.GetContainerReference(ContainerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }


        public async Task<Uri> SaveLogo(Stream logo)
        {
            var img = await GetImage(logo);
            return await UploadToBlob(logo, ContainerName, CreateFileName(img.GetExtension()));
        }

        private string CreateFileName(string extension) => $"logo-{Guid.NewGuid()}{extension}";
        private async Task<Image> GetImage(Stream logo) => await Task.Run(() => Image.FromStream(logo));
    }
}