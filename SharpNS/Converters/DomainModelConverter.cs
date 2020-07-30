using SharpNS.Models.API;
using SharpNS.Models.Database;

namespace SharpNS.Converters
{
    public static class DomainModelConverter
    {
        public static Record ToDomainModel(DnsRecord apiModel) => new Record() { Domain = apiModel.Domain, IpAddress = apiModel.Ip, Type = apiModel.Type };

        public static DnsRecord ToApiModel(Record domainModel) => new DnsRecord() { Domain = domainModel.Domain, Ip = domainModel.IpAddress, Type = domainModel.Type };
    }
}
