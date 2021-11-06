using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StatusAppBackend.Database;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Services
{
    public class FetchServiceInformationService : CronJobService
    {
        private static readonly int OFFSET = 60;
        private readonly ILogger<FetchServiceInformationService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;

        public FetchServiceInformationService(ILogger<FetchServiceInformationService> logger, IServiceScopeFactory scopeFactory) : base(OFFSET)
        {
            this._logger = logger;
            this._scopeFactory = scopeFactory;
            this._httpClient = new HttpClient();
            this._httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($".NET{Environment.Version}");
        }

        public override async Task DoWork(CancellationToken ct)
        {
            await UpdateServiceInformationAsync();
        }

        public async Task UpdateServiceInformationAsync()
        {
            using (IServiceScope scope = this._scopeFactory.CreateScope())
            {
                // get a new instance of the db context as this is a singleton
                StatusAppContext dbContext = scope.ServiceProvider.GetRequiredService<StatusAppContext>();

                List<Service> services = dbContext.Services.ToList();
                List<Task<ServiceInformation>> serviceInformationTasks = new List<Task<ServiceInformation>>();

                foreach (Service service in services)
                    serviceInformationTasks.Add(GetServiceInformationAsync(service));

                // remove all entries that are older than 1 day
                IEnumerable<ServiceInformation> itemsToDelete = dbContext.ServiceInformations.Where(x => x.TimeRequested + TimeSpan.FromDays(1) < DateTime.UtcNow);
                dbContext.RemoveRange(itemsToDelete);

                // store the new data
                ServiceInformation[] serviceInformation = await Task.WhenAll(serviceInformationTasks);
                await dbContext.AddRangeAsync(serviceInformation);
                await dbContext.SaveChangesAsync();
            }

            this._logger.LogInformation("Updated service information");
        }

        private async Task<ServiceInformation> GetServiceInformationAsync(Service service)
        {
            HttpStatusCode httpStatusCode;
            DateTime startTime = DateTime.Now;
            try
            {
                httpStatusCode = (await this._httpClient.GetAsync(service.Url)).StatusCode;

            }
            catch (Exception)
            {
                httpStatusCode = HttpStatusCode.ServiceUnavailable;
            }
            TimeSpan elapsedTime = DateTime.Now - startTime;

            this._logger.LogDebug($"Requested new information for {service.Name}; Got {httpStatusCode} in {elapsedTime.TotalMilliseconds}ms");

            return new ServiceInformation()
            {
                ResponseTime = elapsedTime.TotalMilliseconds,
                ServiceKey = service.Key,
                Service = service,
                StatusCode = (int)httpStatusCode,
                TimeRequested = DateTime.UtcNow
            };
        }
    }
}