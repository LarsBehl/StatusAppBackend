using System;
using ExceptionMiddleware.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusAppBackend.Controllers.DTOs;

namespace StatusAppBackend.Controllers
{
    [ApiController]
    [Route("users")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {

        /// <summary>
        /// Create a new user for administration
        /// </summary>
        /// <returns>A newly created User</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /users
        ///     {
        ///         "username": "Max",
        ///         "password": "sUp3rS3cR37"
        ///         "token": "1234567891234567"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created User in form of a DTO</response>
        /// <response code="400">If the given password is too short</response>
        /// <response code="401">If the given token is not valid</response>
        /// <response code="409">If the username is already taken</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public ActionResult<UserDTO> CreateUser([FromBody] UserCreationDTO userCreation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update password of a given user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /users/1337
        ///     {
        ///         "oldPassword": "sUp3rS3cR37",
        ///         "newPassword": "gIg4s3cR37"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">The password has successfully been updated</response>
        /// <response code="400">If the given password is too short</response>
        /// <response code="401">If the old password does not match</response>
        /// <response code="403">If the authenticated user is not the user to update the password for</response>
        /// <response code="404">If the user could not be found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public ActionResult UpdatePassword([FromRoute] int id, [FromBody] PasswordUpdateDTO passwordUpdate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the user with the given id
        /// </summary>
        /// <response code="204">The user has successfully been deleted</response>
        /// <response code="404">If the user could not be found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}