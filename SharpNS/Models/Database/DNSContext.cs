using Microsoft.EntityFrameworkCore;

namespace SharpNS.Models.Database
{
    public class DNSContext : DbContext
    {
        public DbSet<Record> Records { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=dns.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Record>().HasIndex(r => r.Domain).IsUnique();
            builder.Entity<Record>().Property(r => r.Type).HasConversion<int>();

            base.OnModelCreating(builder);
        }
    }
}
