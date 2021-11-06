using System;

namespace StatusAppBackend.Database.Model
{
    public class ServiceInformation
    {
        public int Key { get; set; }
        public DateTime TimeRequested { get; set; }
        public double ResponseTime { get; set; }
        public int StatusCode { get; set; }

        public Service Service { get; set; }
        public int ServiceKey { get; set; }
    }
}