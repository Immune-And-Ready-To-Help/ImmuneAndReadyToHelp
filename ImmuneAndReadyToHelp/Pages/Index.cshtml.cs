using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ImmuneAndReadyToHelp.Pages
{
    public class IndexModel : PageModel
    {
        public string GoogleMapsApiKey { get; set; }

        public IndexModel(IConfiguration config)
        {
            GoogleMapsApiKey = config["GoogleMapsApiKey"];
        }
    }
}
