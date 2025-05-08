using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using tracking_app.Dto;
using tracking_app.Repository;

namespace tracking_app.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Authenticate(string userName, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public async Task<AuthResponseDto> Authenticate(string userName, string password)
        {
            var user = await userRepository.GetByUserName(userName);

            if (user != null)
            {
                bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (validPassword)
                {
                    var claims = new List<Claim>
                        {
                            new("Id", user.Id.ToString()),
                            new("UserName", user.Username ?? ""),
                            new("Name", user.Name ?? ""),
                        };

                    // Encrypt credentials
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var auth = new JwtSecurityToken(configuration["Jwt:Issuer"],
                        configuration["Jwt:Issuer"],
                        claims,
                        expires: DateTime.Now.AddDays(59),
                        signingCredentials: credentials);

                    // Generate JWT
                    var token = new JwtSecurityTokenHandler().WriteToken(auth);

                    return new AuthResponseDto(true, token);
                }
            }

            // Invalid user credential or failed authentication
            return new AuthResponseDto(false, "Invalid user name or password");
        }
    }
}