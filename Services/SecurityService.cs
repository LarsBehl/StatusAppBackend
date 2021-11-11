using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace StatusAppBackend.Services
{
    public class SecurityService : ISecurityService
    {
        public void HashPassword(string password, out byte[] hash, out byte[] salt)
        {
            salt = new byte[32];
            using (RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider())
            {
                csp.GetBytes(salt);
            }

            hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 100000, 64);
        }

        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            if(storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash; Expected 64 bytes");
            if(storedSalt.Length != 32)
                throw new ArgumentException("Invalid length of password salt; Expected 32 bytes");
            
            byte[] computedHash = KeyDerivation.Pbkdf2(password, storedSalt, KeyDerivationPrf.HMACSHA512, 100000, 64);

            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
    }
}