using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ExceptionMiddleware.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Database;
using StatusAppBackend.Database.Model;
using StatusAppBackend.Exceptions;

namespace StatusAppBackend.Services
{
    public class UserService : IUserService
    {
        private static readonly int MIN_PW_LENGTH = 12;

        private readonly StatusAppContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly ISecurityService _securityService;

        public UserService(StatusAppContext context, ILogger<UserService> logger, ISecurityService securityService)
        {
            this._context = context;
            this._logger = logger;
            this._securityService = securityService;
        }

        public async Task<TokenDTO> CreateRegistrationToken(int issuerId)
        {
            User user = await this._context.Users.SingleOrDefaultAsync(u => u.Id == issuerId);
            // this should never happen, as the issuerId is extracted from the JWT
            if (user is null)
                throw new InternalServerErrorException();

            int id;
            string tokenString;
            do
            {
                id = RandomNumberGenerator.GetInt32(0, Int32.MaxValue);
            } while (await this._context.UserCreationTokens.AnyAsync(t => t.Id == id));

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                // prevent duplicate tokens
                do
                {
                    byte[] tokenBuf = new byte[8];
                    rng.GetNonZeroBytes(tokenBuf);

                    tokenString = BitConverter.ToString(tokenBuf).Replace("-", "");
                } while (await this._context.UserCreationTokens.AnyAsync(t => t.Token == tokenString));
            }

            id = id < 0 ? -id : id;

            UserCreationToken token = new UserCreationToken()
            {
                IssuerId = issuerId,
                Id = id,
                Token = tokenString,
                IssuedAt = DateTime.UtcNow
            };

            this._context.Add(token);
            await this._context.SaveChangesAsync();

            return new TokenDTO(id, tokenString, token.IssuedAt.AddDays(7));
        }

        public async Task<UserDTO> CreateUser(UserCreationDTO userCreation)
        {
            // guard statements
            if (string.IsNullOrWhiteSpace(userCreation.Password))
                throw new BadRequestException("Password invalid");

            if (userCreation.Password.Trim().Length < MIN_PW_LENGTH)
                throw new BadRequestException("Password too short; At least 12 characters required");

            UserCreationToken creationToken = await this._context.UserCreationTokens.SingleOrDefaultAsync(uct => uct.Token == userCreation.Token);
            if (creationToken is null)
                throw new UnauthorizedException("Invalid token");

            if (creationToken.CreatedUserId is not null)
                throw new BadRequestException("Token already used");

            // check if the token expired
            // special case: the first token should not expire, it has no issuerId so check for it
            if (creationToken.IssuedAt + TimeSpan.FromDays(7) < DateTime.UtcNow && creationToken.IssuerId is not null)
                throw new BadRequestException("Token expired");

            if (await this._context.Users.AnyAsync(u => u.Username == userCreation.Username))
                throw new ConflictException("The username is already taken");

            // create new userId
            Random r = new Random();
            int userId = r.Next();

            while (await this._context.Users.AnyAsync(u => u.Id == userId))
            {
                userId = r.Next();
            }

            // compute the secure hash of the password
            this._securityService.HashPassword(userCreation.Password, out byte[] hash, out byte[] salt);

            User user = new User()
            {
                Id = userId,
                Username = userCreation.Username,
                Hash = hash,
                Salt = salt
            };

            this._context.Add(user);
            creationToken.CreatedUserId = userId;

            await this._context.SaveChangesAsync();

            return new UserDTO(user);
        }

        public async Task DeleteUser(int userId)
        {
            User user = await this._context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new UserNotFoundException($"Unknown userId \"{userId}\"");

            this._context.Users.Remove(user);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdatePassword(PasswordUpdateDTO passwordUpdate, int userId)
        {
            if (string.IsNullOrWhiteSpace(passwordUpdate.NewPassword))
                throw new BadRequestException("New password invalid");

            if (passwordUpdate.NewPassword.Trim().Length < MIN_PW_LENGTH)
                throw new BadImageFormatException("New password too short; At least 12 characters required");

            User user = await this._context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new UserNotFoundException($"Unknown userId \"{userId}\"");

            if (!this._securityService.VerifyPassword(passwordUpdate.OldPassword, user.Hash, user.Salt))
                throw new UnauthorizedException("Invalid password");

            this._securityService.HashPassword(passwordUpdate.NewPassword, out byte[] hash, out byte[] salt);

            user.Hash = hash;
            user.Salt = salt;

            await this._context.SaveChangesAsync();
        }
    }
}