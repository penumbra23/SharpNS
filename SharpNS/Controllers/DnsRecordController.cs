using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharpNS.Models.Database;

namespace SharpNS.Controllers
{
    [Route("record")]
    [ApiController]
    public class DnsRecordController : BaseController
    {
        public DnsRecordController(ILogger<DnsRecordController> logger, DNSContext context) : base(logger, context)
        {
        }

        [HttpGet]
        public IActionResult ListDnsRecords()
        {
            return Ok(Context.Records.ToList());
        }
    }
}
