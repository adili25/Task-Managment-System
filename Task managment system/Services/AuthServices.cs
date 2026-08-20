using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_managment_system.Enums;
using Task_managment_system.Exceptions;
using Task_managment_system.Interfaces;
using Task_managment_system.Models;

namespace Task_managment_system.Services
{
    public class AuthServices (IConfiguration _config, IRefreshTokenRepository _refreshTokenRepo)
    {
        private readonly string _secretKey = _config["JwtSettings:Secret"] ?? throw new InvalidOperationException("---missing secret key in config file---");
        private readonly string _audience = _config["JwtSettings:Audience"] ?? throw new InvalidOperationException("---missing audience in config file---");
        private readonly string _issuer = _config["JwtSettings:Issuer"] ?? throw new InvalidOperationException("---missing issuer in config file---");
        private readonly int _expiryInMinutes = _config.GetValue<int?>("JwtSettings:ExpiryInMinutes") ?? throw new InvalidOperationException("---missing expiry in minutes in config file---");

        public string? GenerateJwtToken(string UserId, string Email, Roles Role)
        {
            //bilding the claims for the JWT
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
                    expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                    signingCredentials: cred
                );

            //build the whole object to single string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> IssueRefreshToken(Guid userId, CancellationToken ct)
        {
            var token = RefreshToken.Create(userId, out var rawToken);
            await _refreshTokenRepo.Add(token, ct);
            return rawToken;
        }

        public async Task<(Guid userId, string rawToken)> RotateRefreshToken(string rawToken, CancellationToken cancellationToken)
        {
            var stored = await _refreshTokenRepo.GetByHash(RefreshToken.Hash(rawToken), cancellationToken)
                ?? throw new UnauthorizedException("invalid refresh token");

            if (!stored.IsActive)
                throw new UnauthorizedException("the refresh token is revoked or expired");

            var newToken = RefreshToken.Create(stored.UserId, out var newRawToken);

            stored.RevokedAt = DateTimeOffset.UtcNow;
            stored.ReplacedByTokenHash = newToken.TokenHash;

            try
            {
                await _refreshTokenRepo.Add(newToken, cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new UnauthorizedException("the refresh token is revoked or expired");
            }

            return (stored.UserId, newRawToken);
        }

        public async Task RevokeRefreshToken(string rawToken, CancellationToken ct)
        {
            var stored = await _refreshTokenRepo.GetByHash(RefreshToken.Hash(rawToken), ct);

            if (stored is null || !stored.IsActive) return;

            stored.RevokedAt = DateTimeOffset.UtcNow;
            await _refreshTokenRepo.SaveChanges(ct);
        }


    }
}
