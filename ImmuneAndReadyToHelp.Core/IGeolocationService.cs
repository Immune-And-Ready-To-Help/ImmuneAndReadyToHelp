﻿using ImmuneAndReadyToHelp.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Core
{
    public interface IGeocodingService
    {
        Task<Coordinate> GetGeolocationFromAddress(string address);
    }
}
