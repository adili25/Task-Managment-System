using Task_Manager.Models;
using Task_managment_system.Interfaces;

/* 
 * here we define the UserRepository ot act like the database with shown methods 
*/

namespace Task_managment_system.Repositries
{
    public class UserRepository : IUserRepository
    {
        //acting as the actuall database
        private readonly List<ApplicationUser> Users;

        public UserRepository()
        {
            Users = [];
        }

        //add user to the database (Users)
        public async Task AddUser(ApplicationUser user)
        {
            Users.Add(user);
        }

        //get all users as IEnumerable  
        public async Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            return Users.AsQueryable();
        }

        //get user by Id
        public async Task<ApplicationUser?> GetUserById(Guid Id)
        {
            //fetching the user from Users
            ApplicationUser? user = Users.FirstOrDefault(u => u.Id == Id);
            return user;
        }

        //get the user by email for password validation
        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            //fetching the user from Users
            ApplicationUser? user = Users.FirstOrDefault(u => u.Email == email);
            return user;
        }
    }
}
