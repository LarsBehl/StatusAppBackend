using System;
using System.Collections.Generic;
using System.Linq;
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
            using(IServiceScope scope = this._scopeFactory.CreateScope())
            {
                StatusAppContext dbContext = scope.ServiceProvider.GetRequiredService<StatusAppContext>();

                IEnumerable<UserCreationToken> expiredTokens = dbContext.UserCreationTokens.Where(uct => uct.IssuedAt + TimeSpan.FromDays(7) < DateTime.UtcNow);
                dbContext.RemoveRange(expiredTokens);
                await dbContext.SaveChangesAsync();
            }

            this._logger.LogInformation("Cleaned tokens");
        }
    }
}