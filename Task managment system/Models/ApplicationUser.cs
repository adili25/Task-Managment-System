using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_Manager.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public Roles Role { get; set; }
        
        public ApplicationUser(string fullName, string email, string passwordHash, Roles role)
        {
            if (fullName == default)
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

            Role = role;
            Id = Guid.NewGuid();
        }
    
    }
}
