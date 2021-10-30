using System.Net;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Controllers.DTOs
{
    public class ServiceInformationDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public double ResponseTime { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ServiceInformationDTO(Service service, double responseTime, HttpStatusCode statusCode)
        {
            this.Id = service.Key;
            this.Name = service.Name;
            this.ResponseTime = responseTime;
            this.StatusCode = statusCode;
        }
    }
}