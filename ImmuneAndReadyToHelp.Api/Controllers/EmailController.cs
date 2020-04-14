using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImmuneAndReadyToHelp.Core;
using ImmuneAndReadyToHelp.Core.Model;

namespace ImmuneAndReadyToHelp.Api.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IApplicantEmailService _emailService;

        public EmailController(IApplicantEmailService emailService)
        {
            _emailService = emailService;
        }
        
        [HttpPost]
        public async Task<bool> SendEmail([FromBody] EmailMessage message)
            => await _emailService.Send(message);
    }
}