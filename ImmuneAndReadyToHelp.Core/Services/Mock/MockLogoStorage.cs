using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Core.Services.Mock
{
    public class MockLogoStorage : ILogoStorage
    {
        public Task DeleteLogo(Uri logoUri)
        {
            throw new NotImplementedException();
        }

        public Task<Uri> SaveLogo(Stream logo)
        {
            throw new NotImplementedException();
        }
    }
}
