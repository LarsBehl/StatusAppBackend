using System;

namespace StatusAppBackend.Controllers.DTOs
{
    public class TokenDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public TokenDTO(int id, string token, DateTime expiresAt)
        {
            this.Id = id;
            this.Token = token;
            this.ExpiresAt = expiresAt;
        }
    }
}