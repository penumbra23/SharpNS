using System.ComponentModel.DataAnnotations;

namespace SharpNS.Models.API
{
    public class IpChange
    {
        [Required(ErrorMessage = "'ip' is a required field.")]
        public string Ip { get; set; }
    }
}
