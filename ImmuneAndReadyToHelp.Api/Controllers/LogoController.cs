using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImmuneAndReadyToHelp.Core;

namespace ImmuneAndReadyToHelp.Api.Controllers
{
    [Route("api/logo")]
    [ApiController]
    public class LogoController : ControllerBase
    {
        private readonly ILogoStorage _logoStorageService;

        public LogoController(ILogoStorage logoStorageService)
        {
            _logoStorageService = logoStorageService;
        }

        [HttpPost]
        public async Task<Uri> UploadLogo()
        {
            var imgStream = new MemoryStream();
            await Request.Body.CopyToAsync(imgStream);
            return await _logoStorageService.SaveLogo(imgStream);
        }

        [HttpDelete]
        public async Task DeleteLogo(Uri uri) => await _logoStorageService.DeleteLogo(uri);
    }
}