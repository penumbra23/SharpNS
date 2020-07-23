using Microsoft.AspNetCore.Mvc;

namespace SharpNS.Controllers
{
    [Route("record")]
    [ApiController]
    public class DnsRecordController : ControllerBase
    {
        [HttpGet]
        public IActionResult ListDnsRecords()
        {
            return Ok("test");
        }
    }
}
