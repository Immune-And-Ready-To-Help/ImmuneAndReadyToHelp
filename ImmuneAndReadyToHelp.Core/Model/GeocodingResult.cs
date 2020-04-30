using System;
using System.Collections.Generic;
using System.Text;

namespace ImmuneAndReadyToHelp.Core.Model
{
    public class GeocodingResult
    {
        public string City { get; set; }
        public string State { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
