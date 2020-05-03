using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.EmailTemplates;
using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ImmuneAndReadyToHelp.Pages
{
    public class OpportunityModel : PageModel
    {
        [BindProperty]
		[Required(ErrorMessage = "Please enter a title for your opportunity.")]
		public string Title { get; set; }
        [BindProperty]
		[Required(ErrorMessage = "Please enter a valid email so we can send applications and a link to manage this opportunity.")]
		public string EmailOfOpportunityContact { get; set; }
        [BindProperty]
        public Uri OpportunityPageUri { get; set; }
        [BindProperty]
		[Required(ErrorMessage = "Please describe the nature of your opportunity here in detail.")]
		public string Description { get; set; }
		[BindProperty]
		[Required(ErrorMessage = "We need the address of your opportunity to show it in nearby search results.")]
		public string FullAddressOfOpportunity { get; set; }
		[BindProperty]
		public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(7);
		[BindProperty]
		[Required(ErrorMessage = "We don't verify immunity for you, so you'll have to describe your approach to immunity verification here.")]
		public string ImmunityProofRequirements { get; set; }
		[BindProperty]
		[Required(ErrorMessage = "We need a 300x300 logo uploaded for your organization to show your brand and get applicants' attention.")]
		public IFormFile LogoToDisplay { get; set; }

        public Coordinate AustinDefaultLocation { get; } = new Coordinate { Latitude = 30.2672, Longitude = -97.7431 };

        private IOpportunityDataAccess OpportunityDataAccess { get; set; }
        private IEmailService EmailService { get; set; }
        private IGeocodingService GeolocationService { get; set; }
        private ILogoStorage LogoStorage { get; set; }

        public string GoogleMapsApiKey { get; set; }

        public OpportunityModel(
            IConfiguration config,
            IOpportunityDataAccess opportunityDataAccess, 
            IEmailService emailService, 
            IGeocodingService geolocationService,
            ILogoStorage logoStorage
            )
        {
            GoogleMapsApiKey = config["GoogleMapsApiKey"];
            OpportunityDataAccess = opportunityDataAccess;
            EmailService = emailService;
            GeolocationService = geolocationService;
            LogoStorage = logoStorage;
        }

        public async Task<IActionResult> OnPost()
        {
            var newOpportunity = new Opportunity();

            newOpportunity.Title = Title;
            newOpportunity.Description = Description;
            newOpportunity.EMailAddressOfOpportunityContact = EmailOfOpportunityContact;
            newOpportunity.FullAddress = FullAddressOfOpportunity;
            newOpportunity.ImmunityProofRequirements = ImmunityProofRequirements;
            newOpportunity.ExpirationDate = ExpirationDate;
            newOpportunity.OpportunityPageUri = OpportunityPageUri;

            //TODO: note... this is currently a mock instance that we'll want to replace with Google Maps
            await Task.WhenAll(
                    Task.Run(async () => { newOpportunity.LocationOfOpportunity = (await Getgeocoding()).Coordinate; }),
                    Task.Run(async () => { newOpportunity.CompanyLogo = await UploadLogo(); })
                );

            if (!ModelState.IsValid)
                return Page();

            await OpportunityDataAccess.UpsertOpportunity(newOpportunity);

			await SendConfirmationEmail(newOpportunity);

			return RedirectToPage("Confirmation");
        }

        private async Task<Uri> UploadLogo()
        {
			if (LogoToDisplay == null)
				return AddModelError("LogoToDisplay", "A 300x300 logo is required to create an opportunity.");

            //TODO: validate file type/size
            var maxFileSizeInKilobytes = 200 * 1024;
            if (LogoToDisplay.Length > maxFileSizeInKilobytes)
                return AddModelError("LogoToDisplay", $"The uploaded images should be a maximum of {maxFileSizeInKilobytes / 1024}k in size.");

            using (var logoStream = new MemoryStream())
            {
                await LogoToDisplay.CopyToAsync(logoStream);
                var image = Image.FromStream(logoStream);

                var imageSize = 300;
                if (image.Height != imageSize || image.Width != imageSize)
                    return AddModelError("LogoToDisplay", $"Image size should be {imageSize}px x {imageSize}px. Please resize your logo.");

                logoStream.Position = 0;
                return await LogoStorage.SaveLogo(logoStream);
            }
        }

        private Uri AddModelError(string key, string errorMessage)
        {
            ModelState.AddModelError(key, errorMessage);
            //return a null Uri on purpose. This is just shorthand for adding an error, then returning null.
            return null;
        }

        private async Task<GeocodingResult> Getgeocoding()
        {

            if (!String.IsNullOrEmpty(FullAddressOfOpportunity))
                try
                {
                    var geocodeOfAddress = await GeolocationService.GetGeolocationFromAddress(FullAddressOfOpportunity);
                    return geocodeOfAddress;
                }
                catch
                {
                    ModelState.AddModelError("FullAddressOfOpportunity", "We could not find that address. Please check it and try again.");
                }
            return null;
        }

        private async Task SendConfirmationEmail(Opportunity opportunity)
        {
            //add the necessary parameters for opportunity management
            var parameterDictionary = new Dictionary<string, string>();

            parameterDictionary.Add("EditId", opportunity.EditId);
            parameterDictionary.Add("ActivationId", opportunity.ActivationId);
            parameterDictionary.Add("DeleteId", opportunity.DeleteId);

            var emailBody = EmailTemplates.GetTemplate("OpportunityAdminEmail.html", parameterDictionary);

			var opportunityAdminEmail = new EmailMessage
            {
				Subject = $"ACTION REQUIRED: Your ImmuneAndReadyToHelp.com Opportunity ({opportunity.Title}) Is Almost Live!",
				Body = emailBody,
				To = EmailOfOpportunityContact
            };

			await EmailService.Send(opportunityAdminEmail);
        }
    }
}