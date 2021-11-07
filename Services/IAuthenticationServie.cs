using System.Threading.Tasks;
using StatusAppBackend.Controllers.DTOs;

namespace StatusAppBackend.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate the given user
        /// </summary>
        /// <returns>An object representing the authenticated user</returns>
        Task<AuthenticationDTO> AuthenticateAsync(LoginDTO loginData);
    }
}