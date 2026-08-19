using Microsoft.EntityFrameworkCore;
using Task_managment_system.Database;
using Task_managment_system.Interfaces;
using Task_managment_system.Models;

namespace Task_managment_system.Repositries
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(RefreshToken token, CancellationToken cancellationToken)
        {
            _context.Refresh_token.Add(token);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken?> GetByHash(string tokenHash, CancellationToken cancellationToken)
        {
            var token = await _context.Refresh_token.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);
            return token;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
