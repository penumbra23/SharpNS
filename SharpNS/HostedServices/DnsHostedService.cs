using DNS.Client.RequestResolver;
using DNS.Protocol;
using DNS.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpNS.Models.Database;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharpNS.HostedServices
{
    public class SqliteResolver : IRequestResolver
    {
        public SqliteResolver(IServiceProvider services, ILogger<DnsHostedService> logger)
        {
            Services = services;
            Logger = logger;
        }

        private IServiceProvider Services { get; set; }
        private ILogger Logger { get; set; }

        public Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = default)
        {
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DNSContext>();
                // TODO: implement resolve logic
            }
            return Task.FromResult<IResponse>(new Response() { });
        }
    }

    public class DnsHostedService : IHostedService
    {
        public DnsHostedService(IServiceProvider services, ILogger<DnsHostedService> logger)
        {
            Resolver = new SqliteResolver(services, logger);

            MasterFile masterFile = new MasterFile();
            // TODO: remove testing data
            masterFile.AddIPAddressResourceRecord("google.com", "12.23.34.45");
            // TODO: refactor
            // Server = new DnsServer(masterFile, "8.8.8.8");
            Server = new DnsServer(Resolver);
        }

        private DnsServer Server { get; }
        private SqliteResolver Resolver { get; set; }

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
