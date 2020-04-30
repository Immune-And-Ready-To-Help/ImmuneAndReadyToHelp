using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImmuneAndReadyToHelp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeocodingController : ControllerBase
    {
        public IGeocodingService GeocodingService { get; set; }

        public GeocodingController(IGeocodingService geocodingService)
        {
            GeocodingService = geocodingService;
        }

        // Get: api/<controller>/<address>
        [HttpGet]
        [Route("{address}")]
        public async Task<GeocodingResult> Get(string address)
        {
            return await GeocodingService.GetGeolocationFromAddress(address);
        }
    }
}