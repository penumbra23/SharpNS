using DNS.Client.RequestResolver;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;
using DNS.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpNS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SharpNS.HostedServices
{
    public class FallbackRequestResolver : IRequestResolver
    {
        private IRequestResolver[] resolvers;

        public FallbackRequestResolver(params IRequestResolver[] resolvers)
        {
            this.resolvers = resolvers;
        }

        public async Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            IResponse response = null;

            foreach (IRequestResolver resolver in resolvers)
            {
                response = await resolver.Resolve(request, cancellationToken);
                if (response.AnswerRecords.Count > 0) break;
            }

            return response;
        }
    }

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
            Response response = Response.FromRequest(request);
            
            try
            {
                foreach (Question question in request.Questions)
                {
                    var directRes = DirectDomainQuery(question);

                    if (directRes.Count > 0)
                    {
                        Merge(response.AnswerRecords, directRes, question);
                        continue;
                    }

                    var wildcardRes = WildcardDomainQuery(question);

                    if (wildcardRes.Count > 0)
                        Merge(response.AnswerRecords, wildcardRes, question);
                }
            }
            catch(Exception e)
            {
                Logger.LogWarning($"--- DNS ERROR --- msg: {e.Message} stacktrace: {e.StackTrace}");
            }
            
            return Task.FromResult<IResponse>(response);
        }

        private static void Merge(IList<IResourceRecord> domains, List<Record> records, Question question)
        {
            foreach(Record record in records)
                domains.Add(new IPAddressResourceRecord(
                    new Domain(question.Name.ToString()), 
                    IPAddress.Parse(record.IpAddress)));
        }

        private DNSContext GetDbContext() => Services.CreateScope().ServiceProvider.GetRequiredService<DNSContext>();

        private List<Record> DirectDomainQuery(Question question)
        {
            using var dbContext = GetDbContext();
            string direct = question.Name.ToString().Replace(".", "\\.");
            return dbContext.MatchDomainQuery($"^{direct}$", question.Type).ToList();
        }

        private List<Record> WildcardDomainQuery(Question question)
        {
            using var dbContext = GetDbContext();
            string wildcard = ("\\*." + string.Join(".", question.Name.ToString().Split(".").Skip(1))).Replace(".", "\\.");
            return dbContext.MatchDomainQuery($"^{wildcard}$", question.Type).ToList();
        }
    }

    public class DnsHostedService : IHostedService
    {
        public DnsHostedService(IServiceProvider services, ILogger<DnsHostedService> logger)
        {
            Resolver = new SqliteResolver(services, logger);
            // TODO: refactor
            // Server = new DnsServer(masterFile, "8.8.8.8");
            Server = new DnsServer(new FallbackRequestResolver(Resolver, new UdpRequestResolver(new IPEndPoint(IPAddress.Parse("8.8.8.8"), 53))));
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
