using EBookStore.Data;
using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RequestDto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EBookStore.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<string> GenerateJwtToken(Users user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                    }),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Users> AuthenticateUsers(LoginRequestDto request)
        {
            try
            {
                var user = await _userService.GetUserByUsername(request.Username);

                // Check if user exists and password is correct
                if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                    return null;

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            // Use BCrypt.NET or another library for password verification
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
