using System.Threading.Tasks;
using StatusAppBackend.Controllers.DTOs;

namespace StatusAppBackend.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <returns>The newly created user</returns>
        Task<UserDTO> CreateUser(UserCreationDTO userCreation);
        
        /// <summary>
        /// Updates the password for the given user
        /// </summary>
        Task UpdatePassword(PasswordUpdateDTO passwordUpdate, int userId);
        
        /// <summary>
        /// Deletes the given user
        /// </summary>
        Task DeleteUser(int userId);
    }
}