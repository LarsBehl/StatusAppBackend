using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Services;

namespace StatusAppBackend.Controllers
{
    [ApiController]
    [Route("services")]
    [Produces("application/json")]
    public class ServiceController : ControllerBase
    {
        private IServicesService _servicesService;

        public ServiceController(IServicesService servicesService)
        {
            this._servicesService = servicesService;
        }

        /// <summary>
        /// Get all registered services
        /// </summary>
        /// <returns>A list of all registered services</returns>
        /// <response code="200">Returns a list of all created services</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ServiceDTO>> GetServices()
        {
            return Ok(this._servicesService.GetServices());
        }
    }
}