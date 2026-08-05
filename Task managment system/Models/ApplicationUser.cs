using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_Manager.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Roles Role { get; set; }
        
        public ApplicationUser(string fullName, string email, string passwordHash, Roles role)
        {
            if (FullName == default)
            {
                throw new ArgumentNullException("Null_FullName: must be a string");
            }
            FullName = fullName;
            
            if (email == default)
            {
                throw new ArgumentNullException("Null_Email: must be a string");
            }
            Email = email;
            
            if (passwordHash == default)
            {
                throw new ArgumentNullException("Null_Password: must be a string");
            }
            PasswordHash = passwordHash;

            if (role == default)
            {
                throw new ArgumentNullException("Null_Role: Must Be A String");
            }
            Role = role;

            Id = Guid.NewGuid();
        }
    
    }
}
