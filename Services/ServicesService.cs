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
using StatusAppBackend.Exceptions;

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

        public async Task<ServiceDTO> CreateServiceAsync(ServiceConfigurationDTO service)
        {
            if(string.IsNullOrWhiteSpace(service.Name))
                throw new BadRequestException($"Invalid name {service.Name}");
            
            if(string.IsNullOrWhiteSpace(service.Url))
                throw new BadRequestException($"Invlaid service url");

            if(await this._dbContext.Services.AnyAsync(s => s.Name == service.Name))
                throw new ConflictException($"Service {service.Name} already exists");
            
            if(await this._dbContext.Services.AnyAsync(s => s.Url == service.Url))
                throw new ConflictException($"Service with given url already exists");
            
            Service s = new Service()
            {
                Url = service.Url,
                Name = service.Name
            };

            await this._dbContext.AddAsync(s);
            await this._dbContext.SaveChangesAsync();

            return new ServiceDTO(s);
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            Service service = await this._dbContext.Services.SingleOrDefaultAsync(s => s.Key == serviceId);
            if(service is null)
                throw new NotFoundException($"Unknown service id {serviceId}");

            this._dbContext.Remove(service);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<ServiceDTO> GetServiceAsync(int serviceId)
        {
            Service service = await this._dbContext.Services.SingleOrDefaultAsync(s => s.Key == serviceId);
            if(service is null)
                throw new NotFoundException($"Unknown service id {serviceId}");

            return new ServiceDTO(service);
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

        public async Task<IEnumerable<ServiceDTO>> GetServicesAsync() => (await this._dbContext.Services.ToListAsync()).ConvertAll<ServiceDTO>(x => new ServiceDTO(x));

        public async Task<TimeSeriesDTO> GetServiceTimeSeriesAsync(int id)
        {
            Service service = await this._dbContext.Services.SingleOrDefaultAsync(s => s.Key == id);
            if(service is null)
                throw new NotFoundException($"Service with id {id} not found");

            return new TimeSeriesDTO(
                service,
                await this._dbContext.ServiceInformations.Where(s => s.ServiceKey == id)
                                                         .OrderByDescending(s => s.TimeRequested)
                                                         .ToListAsync()
            );
        }

        public async Task<ServiceDTO> UpdateServiceAsync(ServiceConfigurationDTO service, int id)
        {
            if(string.IsNullOrWhiteSpace(service.Name))
                throw new BadRequestException("Invalid service name");
            
            if(string.IsNullOrWhiteSpace(service.Url))
                throw new BadRequestException("Invalid service url");
            
            if(await this._dbContext.Services.AnyAsync(s => s.Name == service.Name && s.Key != id))
                throw new ConflictException($"Service {service.Name} already exists");
            
            if(await this._dbContext.Services.AnyAsync(s => s.Url == service.Url && s.Key != id))
                throw new ConflictException($"Serivce with given url already exists");

            Service s = await this._dbContext.Services.SingleOrDefaultAsync(s => s.Key == id);
            if(s is null)
                throw new NotFoundException($"Unknown service id {id}");

            s.Name = service.Name;
            s.Url = service.Url;
            await this._dbContext.SaveChangesAsync();

            return new ServiceDTO(s);
        }

        private async Task<ServiceInformation> GetServiceInformationFromDbAsync(int id)
        {
            return await this._dbContext.ServiceInformations.Where(s => s.ServiceKey == id)
                                                            .OrderByDescending(s => s.TimeRequested)
                                                            .Include(s => s.Service)
                                                            .FirstOrDefaultAsync();
        }
    }
}