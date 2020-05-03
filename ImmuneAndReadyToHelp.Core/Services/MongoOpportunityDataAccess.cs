using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ImmuneAndReadyToHelp.Core.Services
{
    public class MongoOpportunityDataAccess : IOpportunityDataAccess
    {
        private IConfiguration Configuration { get; set; }
        private IMongoCollection<Opportunity> OpportunitiesCollection { get; set; }

        public MongoOpportunityDataAccess(IConfiguration configuration)
        {
            Configuration = configuration;

            var client = new MongoClient(configuration["MongoDb:ConnectionString"]);
            var database = client.GetDatabase("Opportunities");
            OpportunitiesCollection = database.GetCollection<Opportunity>("OpportunitiesCollection");
        }

        public async Task<Opportunity> FindOpportunityById(Guid opportunityId)
        {
            var results = await OpportunitiesCollection.FindAsync(
                doc => doc.OpportunityId == opportunityId
                );
            return results.FirstOrDefault();
        }

        public async Task<Opportunity> FindOpportunityByEditId(string editId)
        {
            var results = await OpportunitiesCollection.FindAsync(
                doc => doc.EditId == editId
                );
            return results.FirstOrDefault();
        }

        public async Task<Opportunity> FindOpportunityByDeleteId(string deleteId)
        {
            var results = await OpportunitiesCollection.FindAsync(
                doc => doc.DeleteId == deleteId
                );
            return results.FirstOrDefault();
        }

        public async Task<Opportunity> FindOpportunityByActivationId(string activationId)
        {
            var results = await OpportunitiesCollection.FindAsync(
                doc => doc.ActivationId == activationId
                );
            return results.FirstOrDefault();
        }

        public async Task<List<Opportunity>> FindOpportunitiesInRange(Coordinate topLeft, Coordinate bottomRight)
        {
            var sortByExpirationDate = Builders<Opportunity>.Sort.Ascending((o) => o.ExpirationDate);
            var options = new FindOptions<Opportunity>
            {
                Sort = sortByExpirationDate
            };

            var results = await OpportunitiesCollection.FindAsync(
                //this math is imperfect because it doesn't account for edges of where lat/long ends
                //however, it will work for now
                doc => doc.LocationOfOpportunity.Longitude >= topLeft.Longitude
                        && doc.LocationOfOpportunity.Latitude <= topLeft.Latitude
                        && doc.LocationOfOpportunity.Latitude >= bottomRight.Latitude
                        && doc.LocationOfOpportunity.Longitude <= bottomRight.Longitude
                        && doc.ExpirationDate >= DateTime.Now
                        && doc.Active,
                options
                );
            //just return everything for now. TODO: page or limit results.
            return results?.ToList() ?? new List<Opportunity>();
        }

        public async Task<Opportunity> UpsertOpportunity(Opportunity opportunity)
        {
            var result = await OpportunitiesCollection.ReplaceOneAsync(
                filter: doc => doc.OpportunityId == opportunity.OpportunityId,
                replacement: opportunity,
                options: new ReplaceOptions { IsUpsert = true }
                );

            //in case nothing should be updated. just return the parameter.
            return opportunity;
        }
    }
}
