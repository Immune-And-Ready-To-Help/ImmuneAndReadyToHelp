using ImmuneAndReadyToHelp.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Core
{
    // I decided to implment this as extension methods vs. an abstract class to share consistent functionality
    // between IOpportunityDataAccess implmentations without having to worry or understand a superclass with
    // sealed virtual functions. 
    public static class OpportunityServiceExtensions
    {
        public static async Task<Opportunity> DeleteOpportunity(this IOpportunityDataAccess dataAccess, string deleteId)
        {
            var opportunityToDelete = await dataAccess.FindOpportunityByDeleteId(deleteId);

            //just deactivate so we can still track or re-activate the opportunity
            opportunityToDelete.Active = false;
            opportunityToDelete.ManualDeletionDate = DateTime.Now;

            await dataAccess.UpsertOpportunity(opportunityToDelete);

            return opportunityToDelete;
        }

        public static async Task<Opportunity> ActivateOpportunity(this IOpportunityDataAccess dataAccess, string activationId)
        {
            var opportunityToActivate = await dataAccess.FindOpportunityByActivationId(activationId);

            opportunityToActivate.Active = true;
            var oneWeekFromNow = DateTime.Now.AddDays(7);
            opportunityToActivate.ExpirationDate = oneWeekFromNow;

            await dataAccess.UpsertOpportunity(opportunityToActivate);

            return opportunityToActivate;
        }
    }
}
