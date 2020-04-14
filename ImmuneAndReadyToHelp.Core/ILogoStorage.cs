using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImmuneAndReadyToHelp.Core
{
    public interface ILogoStorage
    {
        Task<Uri> SaveLogo(Stream logo);
        Task DeleteLogo(Uri logoUri);
    }
}
