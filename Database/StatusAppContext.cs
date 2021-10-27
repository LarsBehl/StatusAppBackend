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

            // Data seeding
            builder.Entity<Service>().HasData(new Service() { Key = 1, Name = "Steam Server Info", Url = "https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1/" });
            builder.Entity<Service>().HasData(new Service() { Key = 2, Name = "GitHub API", Url = "https://api.github.com" });
        }
    }
}