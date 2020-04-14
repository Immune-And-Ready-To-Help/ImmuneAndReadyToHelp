using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core.Model;

namespace ImmuneAndReadyToHelp.Core.Services.Mock
{
    public class MockOpportunityDataAccess : IOpportunityDataAccess
    {
        public async Task<bool> DeleteOpportunity(Guid opportunityId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Opportunity>> FindOpportunitiesInRange(Coordinate topLeft, Coordinate bottomRight)
        {
            throw new NotImplementedException();
        }

        public Task<Opportunity> FindOpportunityByDeleteId(string deleteId)
        {
            throw new NotImplementedException();
        }

        public Task<Opportunity> FindOpportunityByEditId(string editId)
        {
            throw new NotImplementedException();
        }

        public Task<Opportunity> FindOpportunityById(Guid opportunityId)
        {
            throw new NotImplementedException();
        }

        public async Task<Opportunity> UpsertOpportunity(Opportunity opportunity)
        {
            throw new NotImplementedException();
        }
    }
}
