using Task_managment_system.Repositries;
using Task_Manager.Models;
using System;
using Task_managment_system.Interfaces;

namespace Task_managment_system.Services
{
    public class UserServices
    {
        private readonly IUserRepository _userRepo;

        public UserServices(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task AddUser(ApplicationUser user)
        {
            _userRepo.AddUser(user);
        }

        public async Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            return await _userRepo.GetAllUsers();
        }
        public async Task<ApplicationUser?> GetUserById(Guid id)
        {
            return await _userRepo.GetUserById(id);
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _userRepo.GetUserByEmail(email);
        }
    }
}
