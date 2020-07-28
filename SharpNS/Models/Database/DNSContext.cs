using DNS.Protocol;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharpNS.Models.Database
{
    public class DNSContext : DbContext
    {
        public DNSContext() : base()
        {
            InitializeFunctions();
        }

        public DbSet<Record> Records { get; set; }

        public void InitializeFunctions()
        {
            if (Database.GetDbConnection() is SqliteConnection sqlite)
            {
                sqlite.CreateFunction(
                    "REGEXP",
                    (string pattern, string value) =>
                    {
                        return Regex.IsMatch(value, pattern);
                    });
            }
        }

        public IQueryable<Record> MatchDomainQuery(string query, RecordType type) =>
            Records.FromSqlRaw("SELECT * FROM Records WHERE Domain REGEXP {0} AND Type = {1}", query, type);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=dns.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Record>().Property(r => r.Type).HasConversion<int>();
            builder.Entity<Record>().HasIndex(e => new { e.Domain, e.Type }).IsUnique();
            base.OnModelCreating(builder);
        }
    }
}
