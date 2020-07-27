using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public List<Record> MatchDomainQuery(string query) =>
            Records.FromSqlRaw($"SELECT * FROM Records WHERE Domain REGEXP '{query}'").ToList();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=dns.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Record>().HasIndex(r => r.Domain).IsUnique();
            builder.Entity<Record>().Property(r => r.Type).HasConversion<int>();

            base.OnModelCreating(builder);
        }
    }
}
