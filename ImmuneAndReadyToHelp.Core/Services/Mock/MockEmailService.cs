using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core.Model;

namespace ImmuneAndReadyToHelp.Core.Services.Mock
{
    public class MockEmailService : IEmailService
    {
        public async Task Send(EmailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
