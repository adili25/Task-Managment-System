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

        public async Task AddUser(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _userRepo.AddUser(user, cancellationToken);
        }

        public async Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            return await _userRepo.GetAllUsers();
        }
        public async Task<ApplicationUser?> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            return await _userRepo.GetUserById(id, cancellationToken);
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            return await _userRepo.GetUserByEmail(email, cancellationToken);
        }

        public async Task CheckRegistedEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserByEmail(email, cancellationToken);

            if (user is not null)
            {
                throw new ConflictException("the email is already taken");
            }

            return;
        }

        public async Task<ApplicationUser> UserVarification(string email, string password, CancellationToken cancellationToken)
        {
            var loginUser = await _userRepo.GetUserByEmail(email, cancellationToken);

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
