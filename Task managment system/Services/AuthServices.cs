using System.IdentityModel.Tokens.Jwt;
using Task_managment_system.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Task_managment_system.Services
{
    public class AuthServices (IConfiguration config)
    {
        private readonly string _secretKey = config["JwtSettings:Secret"];
        private readonly string _audience = config["JwtSettings:Audience"];
        private readonly string _issuer = config["JwtSettings:Issuer"];
        public string GenerateJwtToken(string UserId, string Email, Roles Role)
        {
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
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: cred
                );

            //build the whole object to single string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
