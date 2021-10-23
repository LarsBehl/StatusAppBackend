using System.Collections.Generic;
using System.Linq;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Database;

namespace StatusAppBackend.Services
{
    public class ServicesService : IServicesService
    {
        private StatusAppContext _dbContext;

        public ServicesService(StatusAppContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<ServiceDTO> GetServices() => this._dbContext.Services.ToList().ConvertAll<ServiceDTO>(x => new ServiceDTO(x));
    }
}