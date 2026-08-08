using Task_managment_system.Repositries;
using Task_Manager.Models;
using System;

namespace Task_managment_system.Services
{
    public class UserServices
    {
        private readonly UserRepository _userRepo;

        public UserServices(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public void AddUser(ApplicationUser user)
        {
            _userRepo.AddUser(user);
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }
        public ApplicationUser? GetUserById(Guid id)
        {
            return _userRepo.GetUserById(id);
        }

        public ApplicationUser? GetUserByEmail(string email)
        {
            return _userRepo.GetUserByEmail(email);
        }
    }
}
