using System;
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
        private readonly StatusAppContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly ISecurityService _securityService;

        public UserService(StatusAppContext context, ILogger<UserService> logger, ISecurityService securityService)
        {
            this._context = context;
            this._logger = logger;
            this._securityService = securityService;
        }

        public async Task<UserDTO> CreateUser(UserCreationDTO userCreation)
        {
            // guard statements
            if(string.IsNullOrWhiteSpace(userCreation.Password))
                throw new BadRequestException("Password invalid");
            
            if(userCreation.Password.Trim().Length <= 12)
                throw new BadRequestException("Password too short; At least 12 characters required");

            UserCreationToken creationToken = await this._context.UserCreationTokens.SingleOrDefaultAsync(uct => uct.Token == userCreation.Token);
            if(creationToken is null)
                throw new UnauthorizedException("Invalid token");
            
            if(creationToken.CreatedUserId is not null)
                throw new BadRequestException("Token already used");
            
            // check if the token expired
            // special case: the first token should not expire, it has no issuerId so check for it
            if(creationToken.IssuedAt + TimeSpan.FromDays(7) < DateTime.UtcNow && creationToken.IssuerId is not null)
                throw new BadRequestException("Token expired");
            
            if(await this._context.Users.AnyAsync(u => u.Username == userCreation.Username))
                throw new ConflictException("The username is already taken");
            
            // create new userId
            Random r = new Random();
            int userId = r.Next();

            while(await this._context.Users.AnyAsync(u => u.Id == userId))
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

        public Task DeleteUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdatePassword(PasswordUpdateDTO passwordUpdate, int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}