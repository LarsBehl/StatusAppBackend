using System.ComponentModel.DataAnnotations;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Controllers.DTOs
{
    public class ServiceDTO
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Name { get; set; }

        public ServiceDTO(Service service)
        {
            this.Url = service.Url;
            this.Name = service.Name;
            this.Id = service.Key;
        }

        public ServiceDTO()
        {

        }
    }
}