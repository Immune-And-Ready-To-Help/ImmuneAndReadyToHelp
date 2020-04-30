using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<GeocodingResult> GetGeolocationFromAddress(string address)
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
            //          "address_components" : [
            //        ...
            //          {
            //              "long_name" : "Mountain View",
            //              "short_name" : "Mountain View",
            //              "types" : [ "locality", "political" ]
            //          },
            //          {
            //              "long_name" : "California",
            //              "short_name" : "CA",
            //              "types" : [ "administrative_area_level_1", "political" ]
            //          }
            //      ],
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
                dynamic firstResult = parsedResult.results[0];
                dynamic addressComponents = firstResult.address_components;
                string city = null, state = null;
                foreach(var addressComponent in addressComponents)
                {
                    var types = addressComponent.types as JArray;

                    if (ContainsValue(types, "administrative_area_level_1"))
                        state = addressComponent.long_name;
                    if (ContainsValue(types, "locality"))
                        city = addressComponent.long_name;
                }
                dynamic location = firstResult.geometry.location;
                return new GeocodingResult
                {
                    City = city,
                    State = state,
                    Coordinate = new Coordinate
                    {
                        Latitude = location.lat,
                        Longitude = location.lng
                    }
                };
            }
            catch(Exception ex)
            {
                throw new Exception("Invalid Address", ex);
            }
        }

        private static bool ContainsValue(JArray jArray, string value)
        {
            return jArray.Any(t => t.Value<string>() == value);
        }
    }
}
