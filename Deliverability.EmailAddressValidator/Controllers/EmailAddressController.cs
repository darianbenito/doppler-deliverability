using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deliverability.EmailAddressValidator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailAddressController : ControllerBase
    {
        private readonly ILogger<EmailAddressController> _logger;

        public EmailAddressController(ILogger<EmailAddressController> logger)
        {
            _logger = logger;
        }
    }
}
