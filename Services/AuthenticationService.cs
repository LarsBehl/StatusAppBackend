using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExceptionMiddleware.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StatusAppBackend.Controllers.DTOs;
using StatusAppBackend.Database;
using StatusAppBackend.Database.Model;
using StatusAppBackend.Utils;

namespace StatusAppBackend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly StatusAppContext _context;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ISecurityService _securityService;
        private readonly AppSettings _appSettings;

        public AuthenticationService(
            StatusAppContext context,
            ILogger<AuthenticationService> logger,
            ISecurityService securityService,
            IOptions<AppSettings> appSettings
        )
        {
            this._context = context;
            this._logger = logger;
            this._securityService = securityService;
            this._appSettings = appSettings.Value;
        }

        public async Task<AuthenticationDTO> AuthenticateAsync(LoginDTO loginData)
        {
            if (string.IsNullOrWhiteSpace(loginData.Username) || string.IsNullOrWhiteSpace(loginData.Password))
                throw new BadRequestException("Invalid auth credentials");

            User user = await this._context.Users.SingleOrDefaultAsync(u => u.Username == loginData.Username);
            if (user is null)
                throw new BadRequestException("Invalid username or password");

            if (!this._securityService.VerifyPassword(loginData.Password, user.Hash, user.Salt))
                throw new BadRequestException("Invalid username or password");

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(this._appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationDTO(tokenHandler.WriteToken(token), user);
        }
    }
}