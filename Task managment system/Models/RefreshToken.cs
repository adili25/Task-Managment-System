using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography;
using System.Text;

namespace Task_managment_system.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string TokenHash { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? RevokedAt { get; set; } = null;
        public string? ReplacedByTokenHash { get; set; } = null;
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;
        public bool IsActive => RevokedAt == null && !IsExpired;

        public RefreshToken(Guid userId)
        {
            Id = Guid.NewGuid();
            TokenHash = GenerateRefreshToken();
            UserId = userId;
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7);
            CreatedAt = DateTimeOffset.UtcNow;
        }

        private RefreshToken() { }

        private string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(randomBytes);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
            byte[] hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToHexString(hashBytes);
        }

    }
}
