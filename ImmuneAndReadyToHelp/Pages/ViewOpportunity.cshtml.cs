using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImmuneAndReadyToHelp.Pages
{
    public class ViewOpportunityModel : PageModel
    {
        [BindProperty(SupportsGet = true)]public Guid OpportunityId { get; set; }
        [BindProperty] public string Title { get; set; }
        [BindProperty] public string Description { get; set; }
        [BindProperty] public string FullAddressOfOpportunity { get; set; }
        [BindProperty] public string ImmunityProofRequirements { get; set; }

        private IOpportunityDataAccess OpportunityDataAccess { get; set; }

        public ViewOpportunityModel(IOpportunityDataAccess opportunityDataAccess)
        {
            OpportunityDataAccess = opportunityDataAccess;
        }

        public void OnGet()
        {
            OpportunityId = new Guid(HttpContext.Request.Query["OpportunityId"]);
            var opportunityToPresent =
                OpportunityId.Equals(Guid.Empty) ?
                //by convention, empty GUID means the default "spread the word" opportunity
                Opportunity.MakeSpreadTheWordOpportunity(
                    new Coordinate { Latitude = -90, Longitude = -180 },
                    new Coordinate { Latitude = 90, Longitude = 180 }
                    ) :
                Task.Run(() => OpportunityDataAccess.FindOpportunityById(OpportunityId)).Result;

            Title = opportunityToPresent.Title;
            Description = opportunityToPresent.Description;
            ImmunityProofRequirements = opportunityToPresent.ImmunityProofRequirements;
            FullAddressOfOpportunity = opportunityToPresent.FullAddress;
        }
    }
}