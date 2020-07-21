using DNS.Server;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace SharpNS.HostedServices
{
    public class DnsHostedService : IHostedService
    {
        public DnsHostedService()
        {
            MasterFile masterFile = new MasterFile();
            // TODO: remove testing data
            masterFile.AddIPAddressResourceRecord("google.com", "12.23.34.45");
            Server = new DnsServer(masterFile, "8.8.8.8");
        }

        private DnsServer Server { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Server.Listen();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
