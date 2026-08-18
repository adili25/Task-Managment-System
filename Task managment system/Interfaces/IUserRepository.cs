using Task_Manager.Models;

namespace Task_managment_system.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(ApplicationUser user, CancellationToken cancellationToken);
        public Task<IQueryable<ApplicationUser>> GetAllUsers();
        public Task<ApplicationUser?> GetUserById(Guid Id, CancellationToken cancellationToken);
        public Task<ApplicationUser?> GetUserByEmail(string email, CancellationToken cancellationToken);
    }
}
