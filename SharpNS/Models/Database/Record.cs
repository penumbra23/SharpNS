using DNS.Protocol;

namespace SharpNS.Models.Database
{
    public class Record
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string IpAddress { get; set; }
        public RecordType Type { get; set; }
    }
}
