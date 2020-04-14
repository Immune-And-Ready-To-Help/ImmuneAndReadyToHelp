using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Tests
{
    [TestClass]
    public class GeocodingTests
    {
        private static IConfiguration Config { get; }
            = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        private IGeocodingService GeocodingService { get; }
            = new GoogleMapsGeocodingService(Config);

        [TestMethod]
        public async Task TestGeocoding()
        {
            var whoAddress
                = "Avenue Appia 20 " + Environment.NewLine
                + "CH - 1211 Geneva 27, Switzerland";

            var whoLocation = await GeocodingService.GetGeolocationFromAddress(whoAddress);
            Assert.IsNotNull(whoLocation);
        }
    }
}
