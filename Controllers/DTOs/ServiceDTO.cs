using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Controllers.DTOs
{
    public class ServiceDTO
    {
        public string Url { get; set; }
        public string Name { get; set; }

        public ServiceDTO(Service service)
        {
            this.Url = service.Url;
            this.Name = service.Name;
        }
    }
}