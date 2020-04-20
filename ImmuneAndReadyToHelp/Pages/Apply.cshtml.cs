using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ImmuneAndReadyToHelp.Pages
{
    public class ApplyModel : PageModel
    {
        [BindProperty(SupportsGet = true)]public Guid OpportunityId { get; set; }
        [BindProperty] public string Title { get; set; }
        [BindProperty] public string Description { get; set; }
        [BindProperty] public string FullAddressOfOpportunity { get; set; }
        [BindProperty] public string ImmunityProofRequirements { get; set; }
        [BindProperty] public Coordinate OpportunityLocation { get; set; }
        [BindProperty] public Uri CompanyLogoUri { get; set; }
        public Uri OpportunityPageUri { get; set; }
        public string EMailAddressOfOpportunityContact { get; set; }

        public string GoogleMapsApiKey { get; set; }

        private IOpportunityDataAccess OpportunityDataAccess { get; set; }

        public ApplyModel(IConfiguration config, IOpportunityDataAccess opportunityDataAccess)
        {
            GoogleMapsApiKey = config["GoogleMapsApiKey"];
            OpportunityDataAccess = opportunityDataAccess;
        }

        public void OnGet()
        {
            OpportunityId = new Guid(HttpContext.Request.Query["OpportunityId"]);
            var opportunityToPresent =
                OpportunityId.Equals(Guid.Empty) ?
                //by convention, empty GUID means the default "spread the word" opportunity
                Opportunity.MakeSpreadTheWordOpportunity(
                    new Coordinate { Latitude = 30.2672, Longitude = -97.7431 },
                    new Coordinate { Latitude = 30.2672, Longitude = -97.7431 }
                    ) :
                Task.Run(() => OpportunityDataAccess.FindOpportunityById(OpportunityId)).Result;

            Title = opportunityToPresent.Title;
            Description = opportunityToPresent.Description;
            ImmunityProofRequirements = opportunityToPresent.ImmunityProofRequirements;
            FullAddressOfOpportunity = opportunityToPresent.FullAddress;
            OpportunityLocation = opportunityToPresent.LocationOfOpportunity;
            CompanyLogoUri = opportunityToPresent.CompanyLogo;
            OpportunityPageUri = opportunityToPresent.OpportunityPageUri;
            EMailAddressOfOpportunityContact = opportunityToPresent.EMailAddressOfOpportunityContact;
        }
    }
}