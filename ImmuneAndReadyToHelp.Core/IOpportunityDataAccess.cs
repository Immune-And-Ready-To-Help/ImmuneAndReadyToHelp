using ImmuneAndReadyToHelp.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Core
{
    public interface IOpportunityDataAccess
    {
        Task<Opportunity> FindOpportunityById(Guid opportunityId);
        Task<Opportunity> FindOpportunityByEditId(string editId);
        Task<Opportunity> FindOpportunityByDeleteId(string deleteId);
        Task<Opportunity> UpsertOpportunity(Opportunity opportunity);
        Task<bool> DeleteOpportunity(Guid opportunityId);
        Task<List<Opportunity>> FindOpportunitiesInRange(Coordinate northWest, Coordinate southEast);
    }
}
