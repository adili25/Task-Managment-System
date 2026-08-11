using System.IdentityModel.Tokens.Jwt;
using Task_managment_system.Enums;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Task_managment_system.Services
{
    public class AuthServices (IConfiguration _config)
    {
        private readonly string _secretKey = _config["JwtSettings:Secret"] ?? throw new InvalidOperationException("---missing secret key in config file---");
        private readonly string _audience = _config["JwtSettings:Audience"] ?? throw new InvalidOperationException("---missing audience in config file---");
        private readonly string _issuer = _config["JwtSettings:Issuer"] ?? throw new InvalidOperationException("---missing issuer in config file---");

        public string? GenerateJwtToken(string UserId, string Email, Roles Role)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Email))
            {
                return null;
            }

            //building the claims for the JWT
            var claim = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, Role.ToString()),
                new Claim(ClaimTypes.Email, Email)
            };

            //creating a secrity key and sign it with HmacSha256
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create the actual Jwt security token
            var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claim,
                    expires: DateTime.UtcNow.AddMinutes(120),
                    signingCredentials: cred
                );

            //build the whole object to single string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
