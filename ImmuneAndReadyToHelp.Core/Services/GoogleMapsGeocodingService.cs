using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Core.Services
{
    public class GoogleMapsGeocodingService : IGeocodingService
    {
        private RestClient GoogleMapsClient { get; set; }
        private string GoogleMapsApiKey { get; set; }

        public GoogleMapsGeocodingService(IConfiguration config)
        {
            GoogleMapsClient = new RestClient
            {
                BaseUrl = new Uri("https://maps.googleapis.com")
            };

            var apiKey = config["GoogleMapsApiKey"];
            GoogleMapsClient.AddDefaultParameter("key", apiKey, ParameterType.QueryString);
        }

        public async Task<Coordinate> GetGeolocationFromAddress(string address)
        {
            var request = new RestRequest
            {
                Method = Method.GET,
                Resource = $"maps/api/geocode/json"
            };
            request.AddQueryParameter("address", address);

            var response = await GoogleMapsClient.ExecuteAsync(request);
            //see here for response formatting: https://developers.google.com/maps/documentation/geocoding/intro
            //{
            //    "results" : [
            //       {
            //        ...
            //        "geometry" : {
            //            "location" : {
            //                "lat" : 37.4224764,
            //                "lng" : -122.0842499
            //              }
            //        }
            //    ]
            //}
            try
            {
                dynamic parsedResult = JsonConvert.DeserializeObject(response.Content);
                dynamic location = parsedResult.results[0].geometry.location;
                return new Coordinate
                {
                    Latitude = location.lat,
                    Longitude = location.lng
                };
            }
            catch(Exception ex)
            {
                throw new Exception("Invalid Address", ex);
            }
        }


    }
}
