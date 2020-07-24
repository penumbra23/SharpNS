using SharpNS.Models.API;
using SharpNS.Models.Database;

namespace SharpNS.Convertes
{
    public static class DomainModelConverter
    {
        public static Record ToDomainModel(DnsRecord apiModel)
        {
            return new Record() { Domain = apiModel.Domain, IpAddress = apiModel.Ip, Type = apiModel.Type };
        }

        public static DnsRecord ToApiModel(Record domainModel)
        {
            return new DnsRecord() { Domain = domainModel.Domain, Ip = domainModel.IpAddress, Type = domainModel.Type };
        }
    }
}
