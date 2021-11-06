using System;
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
        public DateTime RequestedAt { get; set; }

        public ServiceInformationDTO(ServiceInformation serviceInformation)
        {
            this.Name = serviceInformation.Service.Name;
            this.Id = serviceInformation.ServiceKey;
            this.ResponseTime = serviceInformation.ResponseTime;
            this.StatusCode = (HttpStatusCode) serviceInformation.StatusCode;
            this.RequestedAt = serviceInformation.TimeRequested;
        }
    }
}