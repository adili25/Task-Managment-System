using Task_Manager.Models;

/* 
 * here we define the UserRepository ot act like the database with shown methods 
*/

namespace Task_managment_system.Repositries
{
    public class UserRepository
    {
        //acting as the actuall database
        private readonly List<ApplicationUser> Users;

        public UserRepository()
        {
            Users = [];
        }

        //add user to the database (Users)
        public void AddUser(ApplicationUser user)
        {
            Users.Add(user);
        }

        //get all users as IEnumerable  
        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return Users.AsQueryable();
        }

        //get user by Id
        public ApplicationUser? GetUserById(Guid Id)
        {
            //fetching the user from Users
            ApplicationUser? user = Users.FirstOrDefault(u => u.Id == Id);
            return user;
        }

        //get the user by email for password validation
        public ApplicationUser? GetUserByEmail(string email)
        {
            //fetching the user from Users
            ApplicationUser? user = Users.FirstOrDefault(u => u.Email == email);
            return user;
        }
    }
}
