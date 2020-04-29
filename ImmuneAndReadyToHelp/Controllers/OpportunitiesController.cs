using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImmuneAndReadyToHelp.Controllers
{
    [Route("api/[controller]")]
    public class OpportunitiesController : Controller
    {
        public IOpportunityDataAccess OpportunityDataAccess { get; set; }

        public OpportunitiesController(IOpportunityDataAccess opportunityDataAccess)
        {
            OpportunityDataAccess = opportunityDataAccess;
        }

        // Post: api/<controller>/FindWithinRange
        [HttpPost]
        [Route("FindWithinRange")]
        public async Task<IEnumerable<OpportunityEssentials>> Post(
            [FromBody]
            OpportunityCoordinateRequestBody cornersOfBounds
            )
        {
            //currently there's a mismatch between this API and the back-end
            //TODO: sync them up
            var northWestCorner = new Coordinate { Longitude = cornersOfBounds.SouthWestCorner.Longitude, Latitude = cornersOfBounds.NorthEastCorner.Latitude };
            var southEastCorner = new Coordinate { Longitude = cornersOfBounds.NorthEastCorner.Longitude, Latitude = cornersOfBounds.SouthWestCorner.Latitude };

            var listOfOpportunities = await OpportunityDataAccess.FindOpportunitiesInRange(northWestCorner, southEastCorner);
            listOfOpportunities.Add(
                Opportunity.MakeSpreadTheWordOpportunity(northWestCorner, southEastCorner)
                );
            var listOfOpportunityEssentials = listOfOpportunities.ConvertAll(
                (opportunity) =>
                {
                    return new OpportunityEssentials
                    {
                        OpportunityId = opportunity.OpportunityId,
                        Title = opportunity.Title,
                        Description = opportunity.Description,
                        ImmunityProofRequirements = opportunity.ImmunityProofRequirements,
                        LocationOfOpportunity = opportunity.LocationOfOpportunity,
                        LogoUrl = opportunity.CompanyLogo?.AbsoluteUri,
                        ExpirationDate = opportunity.ExpirationDate
                    };
                }
            );
            

            return listOfOpportunityEssentials.OrderByDescending((o) => o.ExpirationDate);
        }
    }

    public class OpportunityCoordinateRequestBody
    {
        public Coordinate NorthEastCorner { get; set; }
        public Coordinate SouthWestCorner { get; set; }
    }

    public class OpportunityEssentials
    {
        public Guid OpportunityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImmunityProofRequirements { get; set; }
        public string LogoUrl { get; set; }
        public Coordinate LocationOfOpportunity { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
