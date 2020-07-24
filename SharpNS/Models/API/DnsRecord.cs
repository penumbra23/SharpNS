using DNS.Protocol;
using System.ComponentModel.DataAnnotations;

namespace SharpNS.Models.API
{
    public class DnsRecord : IpChange
    {
        [Required(ErrorMessage = "'domain' is a required field.")]
        public string Domain { get; set; }

        [Required(ErrorMessage = "'type' is a required field.")]
        public RecordType Type { get; set; }
    }
}
