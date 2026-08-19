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

        private RefreshToken(Guid userId, string tokenHash)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7);
            CreatedAt = DateTimeOffset.UtcNow;
        }
        private RefreshToken() { }

        public static RefreshToken Create(Guid userId, out string rawToken)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            rawToken = Convert.ToBase64String(randomBytes);

            return new RefreshToken(userId, Hash(rawToken));
        }

        public static string Hash(string rawToken)
        {
            byte[] tokenBytes = Encoding.UTF8.GetBytes(rawToken);
            byte[] hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToHexString(hashBytes);
        }

    }
}
