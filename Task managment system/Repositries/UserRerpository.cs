using Task_Manager.Models;

/* 
 * here we define the UserRepository ot act like the database with shown methods 
*/

namespace Task_managment_system.Repositries
{
    public class UserRepository
    {
        //acting as the actuall database
        private readonly List<ApplicationUser> Users = [];

        //add user to the database (Users)
        public void AddUser(ApplicationUser user)
        {
            //check null user
            if (user == default)
            {
                throw new NullReferenceException("USER_IS_NULL");
            }

            //if its not null add it to the database
            Users.Add(user);
        }

        //get all users as IEnumerable
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            //--here should me return it using yield return?--
            return Users.AsEnumerable();
        }

        //get user by Id
        public ApplicationUser GetUserById(Guid Id)
        {
            //fetching the user from Users
            ApplicationUser? user = Users.FirstOrDefault(u => u.Id == Id);

            //check if the user exist or not
            if (user == default)
            {
                throw new NullReferenceException("USER_IS_NULL");
            }

            return user;
        }
    }
}
