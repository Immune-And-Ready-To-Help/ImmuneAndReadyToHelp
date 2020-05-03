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
    public class DeleteOpportunityModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public Guid OpportunityId { get; set; }
        [BindProperty] public string Title { get; set; }
        [BindProperty] public Coordinate OpportunityLocation { get; set; }
        [BindProperty] public Uri CompanyLogoUri { get; set; }

        public string GoogleMapsApiKey { get; set; }

        private IOpportunityDataAccess OpportunityDataAccess { get; set; }

        public DeleteOpportunityModel(IConfiguration config, IOpportunityDataAccess opportunityDataAccess)
        {
            GoogleMapsApiKey = config["GoogleMapsApiKey"];
            OpportunityDataAccess = opportunityDataAccess;
        }

        public async Task OnGet()
        {
            var deleteId = HttpContext.Request.Query["DeleteId"];
            var opportunityToPresent = await OpportunityDataAccess.DeleteOpportunity(deleteId);

            OpportunityId = opportunityToPresent.OpportunityId;

            Title = opportunityToPresent.Title;
            OpportunityLocation = opportunityToPresent.LocationOfOpportunity;
            CompanyLogoUri = opportunityToPresent.CompanyLogo;
        }
    }
}