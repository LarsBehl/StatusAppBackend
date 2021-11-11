using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StatusAppBackend.Database;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Services
{
    public class CleanTokenService : CronJobService
    {
        private static readonly int OFFSET = 86400;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CleanTokenService> _logger;

        public CleanTokenService(IServiceScopeFactory scopeFactory, ILogger<CleanTokenService> logger) : base(OFFSET)
        {
            this._scopeFactory = scopeFactory;
            this._logger = logger;
        }

        public override async Task DoWork(CancellationToken ct)
        {
            await this.CleanTokenTable();
        }

        private async Task CleanTokenTable()
        {
            using (IServiceScope scope = this._scopeFactory.CreateScope())
            {
                StatusAppContext dbContext = scope.ServiceProvider.GetRequiredService<StatusAppContext>();

                // check if there is any token in the db
                if (dbContext.UserCreationTokens.Count() <= 0)
                {
                    UserCreationToken seedToken;
                    using (RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider())
                    {
                        byte[] idBuf = new byte[4];
                        csp.GetBytes(idBuf);
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
                    await dbContext.UserCreationTokens.AddAsync(seedToken);

                    this._logger.LogInformation($"Added initial token {seedToken.Token}");
                }

                IEnumerable<UserCreationToken> expiredTokens = dbContext.UserCreationTokens.Where(uct => uct.IssuedAt + TimeSpan.FromDays(7) < DateTime.UtcNow && uct.CreatedUserId == null && uct.IssuerId != null);
                dbContext.RemoveRange(expiredTokens);
                await dbContext.SaveChangesAsync();
            }

            this._logger.LogInformation("Cleaned tokens");
        }
    }
}