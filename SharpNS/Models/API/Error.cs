using System.Collections.Generic;

namespace SharpNS.Models.API
{
    public class Error
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Reasons { get; set; }
    }
}
