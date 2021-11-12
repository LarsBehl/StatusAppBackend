using System.ComponentModel.DataAnnotations;

namespace StatusAppBackend.Controllers.DTOs
{
    public class PasswordUpdateDTO
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}