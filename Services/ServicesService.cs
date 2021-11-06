using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExceptionMiddleware.Exceptions;
using Microsoft.EntityFrameworkCore;
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
            List<ServiceInformation> serviceInformations = new List<ServiceInformation>();
            List<Service> services = await this._dbContext.Services.ToListAsync();
            foreach (Service service in services)
                serviceInformations.Add(await this.GetServiceInformationFromDbAsync(service.Key));

            return serviceInformations.ConvertAll(s => new ServiceInformationDTO(s));
        }

        public async Task<ServiceInformationDTO> GetServiceInformationAsync(int id)
        {
            ServiceInformation service = await this.GetServiceInformationFromDbAsync(id);
            if (service is null)
                throw new NotFoundException($"Service with id {id} not found");

            return new ServiceInformationDTO(service);
        }

        public IEnumerable<ServiceDTO> GetServices() => this._dbContext.Services.ToList().ConvertAll<ServiceDTO>(x => new ServiceDTO(x));

        private async Task<ServiceInformation> GetServiceInformationFromDbAsync(int id)
        {
            return await this._dbContext.ServiceInformations.Where(s => s.ServiceKey == id)
                                                            .OrderByDescending(s => s.TimeRequested)
                                                            .Include(s => s.Service)
                                                            .FirstOrDefaultAsync();
        }
    }
}