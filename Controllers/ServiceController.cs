using System.Collections.Generic;
using System.Threading.Tasks;
using ExceptionMiddleware.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Services;

namespace StatusAppBackend.Controllers
{
    [ApiController]
    [Route("services")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        /// <response code="401">User is not authorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetServices()
        {
            return Ok(await this._servicesService.GetServicesAsync());
        }

        /// <summary>
        /// Get service by id
        /// </summary>
        /// <returns>The service with the given id</returns>
        /// <response code="200">Returns the service with the given id</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="404">If no service with given id exists</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceDTO>> GetService([FromRoute] int id)
        {
            return Ok(await this._servicesService.GetServiceAsync(id));
        }

        /// <summary>
        /// Create new service to query information for
        /// </summary>
        /// <returns>A newly created service</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /services
        ///     {
        ///         "name": "GoogleDNS",
        ///         "url": "https://dns.google/resolve?name=google.com&amp;type=255"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created service in form of a DTO</response>
        /// <response code="400">The given request has invalid content</response>
        /// <response code="401">If the given token is not valid</response>
        /// <response code="409">If there is already a service with the same url or name</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ServiceDTO>> CreateService([FromBody] ServiceDTO service)
        {
            ServiceDTO createdService = await this._servicesService.CreateServiceAsync(service);
            return Created(this.Url.RouteUrl(createdService.Id), createdService);
        }

        /// <summary>
        /// Update the given service
        /// </summary>
        /// <returns>The updated service</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /services/3
        ///     {
        ///         "name": "Google",
        ///         "url": "https://dns.google/resolve?name=google.com&amp;type=255"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the updated service in form of a DTO</response>
        /// <response code="400">The given request contains invalid content</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="404">If no service with the given id exists</response>
        /// <response code="409">If there is already a service with the same url or name</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ServiceDTO>> UpdateService([FromBody] ServiceDTO service, [FromRoute] int id)
        {
            return Ok(await this._servicesService.UpdateServiceAsync(service, id));
        }

        /// <summary>
        /// Deletes the service with the given id
        /// </summary>
        /// <response code="204">If the service was successfully deleted</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="404">If no service with the given id exists</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteService([FromRoute] int id)
        {
            await this._servicesService.DeleteServiceAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get service information for registered services
        /// </summary>
        /// <returns>A list with current status information about the services</returns>
        /// <response code="200">Returns a list with status information for all registred services</response>
        [HttpGet("information")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServiceInformationDTO>>> GetServiceInformation()
        {
            return Ok(await this._servicesService.GetServiceInformationAsync());
        }

        /// <summary>
        /// Get service information for given service
        /// </summary>
        /// <returns>Status information about the given service</returns>
        /// <response code="200">Status information for the service with the given id</response>
        /// <response code="404">An error response object for the service that could not be found</response>
        [HttpGet("information/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceInformationDTO>> GetServiceInformation([FromRoute] int id)
        {
            return Ok(await this._servicesService.GetServiceInformationAsync(id));
        }

        /// <summary>
        /// Get a time series for a given service
        /// </summary>
        /// <returns>Time series of status information about a given service</returns>
        /// <response code="200">Time series of status information of the given service</response>
        /// <response code="404">An error response object for the service thath could not be found</response>
        [HttpGet("information/{id}/timeseries")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TimeSeriesDTO>> GetSerivceInformationTimeSeries([FromRoute] int id)
        {
            return Ok(await this._servicesService.GetServiceTimeSeriesAsync(id));
        }
    }
}