using System;
using System.Collections.Generic;
using System.Text;

namespace ImmuneAndReadyToHelp.Core.Model
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsInRectangle(Coordinate topLeft, Coordinate bottomRight)
        {
            if (Latitude > topLeft.Latitude)
                return false;
            else if (Latitude < bottomRight.Latitude)
                return false;

            var longitudeWest = topLeft.Longitude;
            var longitudeEast = bottomRight.Longitude;

            if (longitudeEast >= longitudeWest)
                return ((Longitude >= longitudeWest) && (Longitude <= longitudeEast));
            else
                return (Longitude >= longitudeWest) || (Longitude <= longitudeEast);
        }
    }
}
