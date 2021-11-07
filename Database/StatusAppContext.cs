using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Database
{
    public class StatusAppContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceInformation> ServiceInformations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCreationToken> UserCreationTokens { get; set; }

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

            // User model creation
            builder.Entity<User>().HasKey(x => x.Id);
            builder.Entity<User>().Property(x => x.Id).IsRequired();
            builder.Entity<User>().Property(x => x.Username).IsRequired();
            builder.Entity<User>().Property(x => x.Hash).IsRequired();
            builder.Entity<User>().Property(x => x.Salt).IsRequired();

            // UserCreationToken model creation
            builder.Entity<UserCreationToken>().HasKey(x => x.Id);
            builder.Entity<UserCreationToken>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<UserCreationToken>().Property(x => x.IssuedAt).IsRequired();
            builder.Entity<UserCreationToken>().Property(x => x.Token).IsRequired();
            builder.Entity<UserCreationToken>().HasOne(x => x.Issuer).WithMany().HasForeignKey(x => x.IssuerId);
            builder.Entity<UserCreationToken>().HasOne(x => x.CreatedUser).WithOne().HasForeignKey(typeof(User), nameof(UserCreationToken.CreatedUserId));

            // Data seeding
            builder.Entity<Service>().HasData(new Service() { Key = 1, Name = "Steam Server Info", Url = "https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1/" });
            builder.Entity<Service>().HasData(new Service() { Key = 2, Name = "GitHub API", Url = "https://api.github.com" });

            UserCreationToken seedToken;
            using (RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider())
            {
                byte[] idBuf = new byte[4];
                csp.GetNonZeroBytes(idBuf);
                byte[] token = new byte[8];
                csp.GetNonZeroBytes(token);
                int id = BitConverter.ToInt32(idBuf);
                id = id < 0 ? -id : id;

                seedToken = new UserCreationToken()
                {
                    Id = id,
                    Token = BitConverter.ToString(token).Replace("-", ""),
                    CreatedUserId = null,
                    IssuerId = null,
                    IssuedAt = DateTime.UtcNow
                };
            }
            builder.Entity<UserCreationToken>().HasData(seedToken);
        }
    }
}