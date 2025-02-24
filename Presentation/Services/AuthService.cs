using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Infrastructure.Database;
using Presentation.Interfaces;
using Presentation.Model.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly SecurityAppDbContext _context;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryInHours;

        public AuthService(IEmailService emailService, SecurityAppDbContext context, IConfiguration configuration)
        {
            _emailService = emailService;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            var jwtSettings = configuration.GetSection("JwtSettings");

            _jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing.");
            _issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing.");
            _audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is missing.");
            _expiryInHours = int.Parse(jwtSettings["ExpiryInHours"]);
        }

        public async Task<string?> RequestLoginAsync(string email)
        {
            var user = await _context.UserConfigurations.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null) return null; /*return false;*/
            var exists = VerifyLogin(user);
            if(!exists)
            {
                return null;
            }

            return GenerateJwtToken(user);
            //var token = GenerateJwtToken(user);
            //return await _emailService.SendEmailAsync(email, token);
        }

        public bool VerifyLogin(UserConfiguration user)
        {
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public bool VerifyLogin(string token)
        {
            return false;
            //return _users.Values.Contains(token);
        }

        private string GenerateJwtToken(UserConfiguration user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("name", user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_expiryInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
