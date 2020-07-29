using DNS.Protocol;
using SharpNS.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SharpNS.Models.API
{
    public class DnsRecord : IpChange
    {
        [Required(ErrorMessage = "'domain' is a required field.")]
        [Domain]
        public string Domain { get; set; }

        [Required(ErrorMessage = "'type' is a required field.")]
        public RecordType Type { get; set; }
    }
}
