using System.Collections.Generic;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Controllers.DTOs
{
    public class TimeSeriesDTO
    {
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public List<TimeSeriesEntryDTO> Data { get; set; }

        public TimeSeriesDTO(Service service, List<ServiceInformation> serviceInformation)
        {
            this.ServiceId = service.Key;
            this.ServiceName = service.Name;
            this.Data = serviceInformation.ConvertAll(s => new TimeSeriesEntryDTO(s));
        }
    }
}