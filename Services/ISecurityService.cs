namespace StatusAppBackend.Services
{
    public interface ISecurityService
    {
        /// <summary>
        /// Hashes the given password
        /// </summary>
        void HashPassword(string password, out byte[] hash, out byte[] salt);

        /// <summary>
        /// Verifies that the given password is equal to the stored one
        /// </summary>
        /// <returns><see langword="true" /> if the passwords match, <see langword="false" /> otherwise</returns>
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}