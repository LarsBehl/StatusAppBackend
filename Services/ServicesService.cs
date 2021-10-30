using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Database;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Services
{
    public class ServicesService : IServicesService
    {
        private StatusAppContext _dbContext;
        private HttpClient _httpClient;

        public ServicesService(StatusAppContext dbContext)
        {
            this._dbContext = dbContext;
            this._httpClient = new HttpClient();
            this._httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($".NET{Environment.Version}");
        }

        public async Task<IEnumerable<ServiceInformationDTO>> GetServiceInformationAsync()
        {
            List<Service> services = this._dbContext.Services.ToList();
            List<Task<ServiceInformationDTO>> serviceInformationTasks = new List<Task<ServiceInformationDTO>>();

            foreach(Service service in services)
                serviceInformationTasks.Add(GetServiceInformationAsync(service));
            
            return await Task.WhenAll(serviceInformationTasks);
        }

        public IEnumerable<ServiceDTO> GetServices() => this._dbContext.Services.ToList().ConvertAll<ServiceDTO>(x => new ServiceDTO(x));

        private async Task<ServiceInformationDTO> GetServiceInformationAsync(Service service)
        {
            HttpStatusCode httpStatusCode;
            DateTime startTime = DateTime.Now;
            try
            {
                httpStatusCode = (await this._httpClient.GetAsync(service.Url)).StatusCode;

            }
            catch(Exception)
            {
                httpStatusCode = HttpStatusCode.ServiceUnavailable;
            }
            TimeSpan elapsedTime = DateTime.Now - startTime;


            return new ServiceInformationDTO(service, elapsedTime.TotalMilliseconds, httpStatusCode);
        }
    }
}