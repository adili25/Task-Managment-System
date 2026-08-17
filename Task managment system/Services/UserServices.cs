using Microsoft.AspNetCore.Identity.Data;
using System;
using Task_Manager.Models;
using Task_managment_system.Exceptions;
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
            await _userRepo.AddUser(user);
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

        public async Task CheckRegistedEmail(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);

            if (user is not null)
            {
                throw new ConflictException("the email is already taken");
            }

            return;
        }

        public async Task<ApplicationUser> UserVarification(string email, string password)
        {
            var loginUser = await _userRepo.GetUserByEmail(email);

            if (loginUser is null)
            {
                throw new NotFoundException($"the user email:{email} not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, loginUser.PasswordHash))
            {
                throw new UnauthorizedException("the user entered invalid password");
            }

            return loginUser;
        }
    }
}
