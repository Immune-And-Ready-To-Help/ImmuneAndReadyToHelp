using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace ImmuneAndReadyToHelp.Core.Model
{
    public class Opportunity
    {
        #region fields
        private string title, description, immunityProofRequirements;
        #endregion fields

        [BsonId]
        public Guid OpportunityId { get; set; } = Guid.NewGuid();
        public string DeleteId { get; set; } = GenerateLongId();
        public string EditId { get; set; } = GenerateLongId();
        public string ActivationId { get; set; } = GenerateLongId();
        public string FullAddress { get; set; }
        //TODO: automatically get lat/long based on postal code
        public Coordinate LocationOfOpportunity { get; set; }
        public bool Active { get; set; } = false;
        public string Title
        {
            //sanitize in both cases in case we missed something in an earlier version of code
            get { return title.SanitizeInput(); }
            set { title = value.SanitizeInput(); }
        }
        public string Description
        {
            //sanitize in both cases in case we missed something in an earlier version of code
            get { return description.SanitizeInput(); }
            set { description = value.SanitizeInput(); }
        }
        public string ImmunityProofRequirements
        {
            //sanitize in both cases in case we missed something in an earlier version of code
            get { return immunityProofRequirements.SanitizeInput(); }
            set { immunityProofRequirements = value.SanitizeInput(); }
        }
        public Uri CompanyLogo { get; set; }
        public string EMailAddressOfOpportunityContact { get; set; }
        public Uri OpportunityPageUri { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? ManualDeletionDate { get; set; }

        /// <summary>
        /// Creates a long, computationally-infeasible-to-replicate ID used for unique links used to edit or delete posts.
        /// </summary>
        /// <returns>A long, computationally-infeasible-to-replicate ID.</returns>
        private static string GenerateLongId()
        {
            var length = 60;
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var idBuilder = new StringBuilder();
            using (var cryptographicallyStrongRandomGenerator = new RNGCryptoServiceProvider())
            {
                var nextBytesBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    cryptographicallyStrongRandomGenerator.GetBytes(nextBytesBuffer);
                    var num = BitConverter.ToUInt32(nextBytesBuffer, 0);
                    idBuilder.Append(validCharacters[(int)(num % (uint)validCharacters.Length)]);
                }
            }

            return idBuilder.ToString();
        }


        //this generates a default opportunity used for every area search result
        public static Opportunity MakeSpreadTheWordOpportunity(Coordinate northWestCorner, Coordinate southEastCorner)
        {
            var centerOfMap = new Coordinate
            {
                Latitude = (northWestCorner.Latitude + southEastCorner.Latitude) / 2,
                Longitude = (northWestCorner.Longitude + southEastCorner.Longitude) / 2
            };

            return new Opportunity
            {
                OpportunityId = Guid.Empty,
                Title = "Help Us Spread The Word About ImmuneAndReadyToHelp.com!",
                Description =
                "Together, we have a real chance to make and impact on the world, but we need your help!" + Environment.NewLine + Environment.NewLine
                + "Help spread the word about the website and help the global community!",
                ImmunityProofRequirements = "None. Just a willingness to tell your collegues and friends!",
                LocationOfOpportunity = centerOfMap,
                CompanyLogo = new Uri("https://www.ImmuneAndReadyToHelp.com/images/Antibody.png"),
                ExpirationDate = DateTime.Now,
                OpportunityPageUri = new Uri("https://www.facebook.com/ImmuneAndReadyToHelp"),
                Active = true
            };
        }
    }
}
