using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharpNS.Converters;
using SharpNS.Exceptions;
using SharpNS.Models.API;
using SharpNS.Models.Database;
using System.Threading.Tasks;

namespace SharpNS.Controllers
{
    [Route("record")]
    [ApiController]
    public class DnsRecordController : BaseController
    {
        public DnsRecordController(ILogger<DnsRecordController> logger, DNSContext context) : base(logger, context) { }

        [HttpGet]
        public async Task<IActionResult> ListDnsRecords()
        {
            return Ok(await Context.Records.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListDnsRecords([FromRoute] int id)
        {
            Record record = await Context.Records.FindAsync(id);
            if (record == null) throw new ApiException(404, $"Record with id {id} not found.");
            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddDnsRecord(DnsRecord record)
        {
            var entity = await Context.Records.AddAsync(DomainModelConverter.ToDomainModel(record));
            await Context.SaveChangesAsync();
            return Ok(entity.Entity);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeRecordAddress([FromRoute]int id, [FromBody] IpChange changeReq)
        {
            Record record = await Context.Records.FindAsync(id);
            if (record == null) throw new ApiException(404, $"Record with id {id} not found.");
            record.IpAddress = changeReq.Ip;
            await Context.SaveChangesAsync();
            return Ok(record);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord([FromRoute] int id)
        {
            Record record = await Context.Records.FindAsync(id);
            if (record == null) throw new ApiException(404, $"Record with id {id} not found.");
            Context.Records.Remove(record);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
