using System.ComponentModel.DataAnnotations;

namespace StatusAppBackend.Controllers.DTOs
{
    public class UserCreationDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }
    }
}