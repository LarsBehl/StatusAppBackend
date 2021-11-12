using System.Collections.Generic;
using System.Threading.Tasks;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Services
{
    public interface IServicesService
    {
        /// <summary>
        /// Get a <see cref="List{ServiceDTO}"/> of all registered services 
        /// </summary>
        /// <returns>A <see cref="List{ServiceDTO}"/> of all registered services</returns>
        Task<IEnumerable<ServiceDTO>> GetServicesAsync();

        /// <summary>
        /// Get the <see cref="ServiceDTO" /> with the given id
        /// </summary>
        /// <returns>A <see cref="ServiceDTO" /> representing the service</returns>
        Task<ServiceDTO> GetServiceAsync(int serviceId);

        /// <summary>
        /// Get a <see cref="List{ServiceInformationDTO}" /> containing current information about the registered services
        /// </summary>
        /// <returns>A <see cref="List{ServiceInformationDTO}"/> containing the current information about the registered services</returns>
        Task<IEnumerable<ServiceInformationDTO>> GetServiceInformationAsync();

        /// <summary>
        /// Get a <see cref="ServiceInformationDTO" /> containing current information about the given service
        /// </summary>
        /// <returns>A <see cref="ServiceInformationDTO" /> containing current information about the given service</returns>
        Task<ServiceInformationDTO> GetServiceInformationAsync(int id);

        /// <summary>
        /// Get a <see cref="List{ServiceInformationDTO}" /> containing a time series about the given service
        /// </summary>
        /// <returns> A <see cref="List{ServiceInformationDTO}" /> containing a time series about the given service</returns>
        Task<TimeSeriesDTO> GetServiceTimeSeriesAsync(int id);

        /// <summary>
        /// Creates a <see cref="Service" /> which will be queried by the cron service automatically
        /// </summary>
        /// <returns>A <see cref="ServiceDTO" /> representing the created object</returns>
        Task<ServiceDTO> CreateServiceAsync(ServiceDTO service);

        /// <summary>
        /// Update the <see cref="Service" /> with the given id
        /// </summary>
        /// <returns>The updated service</returns>
        Task<ServiceDTO> UpdateServiceAsync(ServiceDTO service, int id);

        /// <summary>
        /// Deletes the service with the given id
        /// </summary>
        Task DeleteServiceAsync(int serviceId);
    }
}