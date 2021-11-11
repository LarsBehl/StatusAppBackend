using StatusAppBackend.Database.Model;

namespace StatusAppBackend.Controllers.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public UserDTO(User user)
        {
            this.Username = user.Username;
            this.Id = user.Id;
        }
    }
}