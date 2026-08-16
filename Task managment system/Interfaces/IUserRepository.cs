using Task_Manager.Models;

namespace Task_managment_system.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(ApplicationUser user);
        public Task<IQueryable<ApplicationUser>> GetAllUsers();
        public Task<ApplicationUser?> GetUserById(Guid Id);
        public Task<ApplicationUser?> GetUserByEmail(string email);
    }
}
