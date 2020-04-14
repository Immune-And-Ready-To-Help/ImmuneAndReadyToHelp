using ImmuneAndReadyToHelp.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Core.Services.Mock
{
    public class MockGeolocationService : IGeocodingService
    {
        public async Task<Coordinate> GetGeolocationFromAddress(string address)
        {
            return new Coordinate
            {
                Latitude = 1,
                Longitude = 1
            };
        }
    }
}
