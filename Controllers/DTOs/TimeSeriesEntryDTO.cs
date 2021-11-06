using System;
using System.Net;
using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Controllers.DTOs
{
    public class TimeSeriesEntryDTO
    {
        public double ResponseTime { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public DateTime RequestedAt { get; set; }

        public TimeSeriesEntryDTO(ServiceInformation serviceInformation)
        {
            this.ResponseTime = serviceInformation.ResponseTime;
            this.RequestedAt = serviceInformation.TimeRequested;
            this.StatusCode = (HttpStatusCode) serviceInformation.StatusCode;
        }
    }
}