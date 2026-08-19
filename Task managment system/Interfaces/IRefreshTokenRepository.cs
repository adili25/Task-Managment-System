using Task_managment_system.Models;

namespace Task_managment_system.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task Add(RefreshToken token, CancellationToken ct);
        Task<RefreshToken?> GetByHash(string tokenHash, CancellationToken ct);
        Task SaveChanges(CancellationToken ct);
    }
}
