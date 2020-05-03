using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.Model;
using ImmuneAndReadyToHelp.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Tests
{
    [TestClass]
    public class DataAccessTests
    {
        private static IConfiguration Config { get; }
            = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        private IOpportunityDataAccess OpportunityDataAccess { get; }
            = new MongoOpportunityDataAccess(Config);

        public Opportunity Opportunity { get; }
            = new Opportunity
            {
                OpportunityId = new Guid("ff0602f5-2483-4a87-bbe1-4795db1078a2"),
                Title = "Test Opportunity",
                Description = "A description of the opportunity.",
                ImmunityProofRequirements = "Doctor's note.",
                LocationOfOpportunity = new Coordinate { Latitude = 1, Longitude = 2},
                DeleteId = "123",
                EditId = "456",
                EMailAddressOfOpportunityContact = "ImmuneAndReadyToHelp@GMail.com",
                FullAddress = "1600 Pennsylvania Ave NW, Washington, DC 20500",
                CompanyLogo = new Uri("https://www.google.com/maps/uv?hl=en&pb=!1s0x89b7b7bcdecbb1df%3A0x715969d86d0b76bf!3m1!7e115!4s%2F%2Flh5.googleusercontent.com%2Fproxy%2F9KFhRXn1ErU1c0Ql21SVaEfzVmuSUF4A5Yvv-ve_-nYDnSDeawRzyOdgs0r6nrcc4dA7JnSAIsY34iu3CtzAsawqJWYtk8C3oRZwMHmRKmHxkulFyUnXkA2fuTbqBm7muk4p1QboYetDRb2Prr4I446uESR6Uw%3Dw460-h320-k-no!5swhite%20house%20address%20-%20Google%20Search!15sCAQ&imagekey=!1e1!2shttp%3A%2F%2Ft0.gstatic.com%2Fimages%3Fq%3Dtbn%3AANd9GcTXL4tehXDJbwMNQUoGVEk4QTPu3A4vyhQ7SHe690BXGhpSp6H3&sa=X&ved=2ahUKEwiGgY3khMnoAhVEA6wKHTLlD6cQoiowHnoECBkQCQ#")
            };

        [TestMethod]
        public async Task TestAddOpportunity()
        {
            var opportunity 
                = await OpportunityDataAccess.UpsertOpportunity(
                    Opportunity
                );
        }

        [TestMethod]
        public async Task TestActivateOpportunity()
        {
            await OpportunityDataAccess.ActivateOpportunity(
                    Opportunity.ActivationId
                );

            //assumes immediate vs. eventual consistency
            var activatedOopportunity = await FindById(Opportunity.OpportunityId);
            Assert.IsTrue(activatedOopportunity.Active);
        }

        [TestMethod]
        public async Task TestDeleteOpportunity()
        {
            await OpportunityDataAccess.DeleteOpportunity(
                    Opportunity.DeleteId
                );

            //assumes immediate vs. eventual consistency
            var deletedOpportunity = await FindById(Opportunity.OpportunityId);
            Assert.IsFalse(deletedOpportunity.Active);
        }

        [TestMethod]
        public async Task FindOpportunity()
        {
            var opportunity = await FindById(Opportunity.OpportunityId);

            Assert.IsNotNull(opportunity);
        }

        [TestMethod]
        public async Task FindOpportunityByEditId()
        {
            var opportunity = await OpportunityDataAccess.FindOpportunityByEditId(Opportunity.EditId);

            Assert.IsNotNull(opportunity);
        }

        private async Task<Opportunity> FindById(Guid opportunityId)
        {
            return await OpportunityDataAccess.FindOpportunityById(
                    Opportunity.OpportunityId
                );
        }


    }
}
