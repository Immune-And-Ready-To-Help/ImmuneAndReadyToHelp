using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ImmuneAndReadyToHelp.Pages
{
    public class FaqModel : PageModel
    {
        public Coordinate AustinDefaultLocation { get; } = new Coordinate { Latitude = 30.2672, Longitude = -97.7431 };

        public string GoogleMapsApiKey { get; set; }

        public FaqModel(IConfiguration config)
        {
            GoogleMapsApiKey = config["GoogleMapsApiKey"];
        }
    }
}
