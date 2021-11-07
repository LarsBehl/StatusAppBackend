using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Controllers.DTOs
{
    public class AuthenticationDTO
    {
        public string Token { get; set; }
        public UserDTO User { get; set; }

        public AuthenticationDTO(string token, User user)
        {
            this.Token = token;
            this.User = new UserDTO(user);
        }
    }
}