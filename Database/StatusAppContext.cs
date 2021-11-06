using Microsoft.EntityFrameworkCore;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Database
{
    public class StatusAppContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceInformation> ServiceInformations { get; set; }

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

            // ServiceInformation model creation
            builder.Entity<ServiceInformation>().HasKey(x => x.Key);
            builder.Entity<ServiceInformation>().Property(x => x.Key).ValueGeneratedOnAdd();
            builder.Entity<ServiceInformation>().Property(x => x.ResponseTime).IsRequired();
            builder.Entity<ServiceInformation>().Property(x => x.StatusCode).IsRequired();
            builder.Entity<ServiceInformation>().Property(x => x.TimeRequested).IsRequired();
            builder.Entity<ServiceInformation>().HasOne(x => x.Service).WithMany().HasForeignKey(x => x.ServiceKey);

            // Data seeding
            builder.Entity<Service>().HasData(new Service() { Key = 1, Name = "Steam Server Info", Url = "https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1/" });
            builder.Entity<Service>().HasData(new Service() { Key = 2, Name = "GitHub API", Url = "https://api.github.com" });
        }
    }
}