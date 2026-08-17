using Task_Manager.Models;
using Task_managment_system.Database;
using Task_managment_system.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Task_managment_system.Repositries
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(ApplicationUser newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        //get all users as IEnumerable  
        public async Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            return _context.Users;
            //materilize the query in the caller => await query.ToListAsync();
        }

        //get user by Id
        public async Task<ApplicationUser?> GetUserById(Guid Id)
        {
            var user = await _context.Users.FindAsync(new object[] { Id });
            return user;
        }

        //get the user by email for password validation
        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
