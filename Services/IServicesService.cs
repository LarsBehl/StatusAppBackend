using System.Collections.Generic;
using System.Threading.Tasks;
using StatusAppBackend.Controllers.DTOs;

namespace StatusAppBackend.Services
{
    public interface IServicesService
    {
        /// <summary>
        /// Get a <see cref="List{ServiceDTO}"/> of all registered services 
        /// </summary>
        /// <returns>A <see cref="List{ServiceDTO}"/> of all registered services</returns>
        IEnumerable<ServiceDTO> GetServices();

        /// <summary>
        /// Get a <see cref="List{ServiceInformationDTO}" /> containing current information about the registered services
        /// </summary>
        /// <returns>A <see cref="List{ServiceInformationDTO}"/> containing the current information about the registered services</returns>
        Task<IEnumerable<ServiceInformationDTO>> GetServiceInformationAsync();
    }
}