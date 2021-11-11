using System;

namespace StatusAppBackend.Database.Model
{
    public class UserCreationToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime IssuedAt { get; set; }

        public int? IssuerId { get; set; }
        public User Issuer { get; set; }
        public int? CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
    }
}