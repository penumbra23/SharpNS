using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharpNS.Models.Database;

namespace SharpNS.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController(ILogger logger, DNSContext context)
        {
            Context = context;
            Logger = logger;
        }

        protected DNSContext Context { get; set; }
        protected ILogger Logger { get; set; }
    }
}
