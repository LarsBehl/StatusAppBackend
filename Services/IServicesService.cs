using System.Collections.Generic;
using StatusAppBackend.Controllers.DTOs;

namespace StatusAppBackend.Services
{
    public interface IServicesService
    {
        /// <summary>
        /// Get a <see cref="List{ServiceDTO}"/> of all registered services 
        /// </summary>
        /// <returns>A <see cref="List{ServiceDTO}"/>of all registered services</returns>
        IEnumerable<ServiceDTO> GetServices();
    }
}