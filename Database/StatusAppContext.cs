using Microsoft.EntityFrameworkCore;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Database
{
    public class StatusAppContext : DbContext
    {
        public DbSet<Service> Services { get; set; }

        public StatusAppContext(DbContextOptions<StatusAppContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Service model creation
            builder.Entity<Service>().HasKey(x => x.Key);
            builder.Entity<Service>().Property(x => x.Key).ValueGeneratedOnAdd();
            builder.Entity<Service>().Property(x => x.Url).IsRequired();
            builder.Entity<Service>().Property(x => x.Name).IsRequired();
        }
    }
}