using System;
using System.Threading.Tasks;
using ExceptionMiddleware.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Services;

namespace StatusAppBackend.Controllers
{
    [ApiController]
    [Route("authenticate")]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticate a user and retrieve a JWT
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /authenticate
        ///     {
        ///         "username": "max",
        ///         "password": "superSecret"
        ///     }
        ///
        /// </remarks>
        /// <returns>Token and user information for given credentials</returns>
        /// <response code="200">User successfully authenticated</response>
        /// <response code="400">If user credentials are invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticationDTO>> Authenticate([FromBody] LoginDTO userData)
        {
            return Ok(await this._authenticationService.AuthenticateAsync(userData));
        }
    }
}