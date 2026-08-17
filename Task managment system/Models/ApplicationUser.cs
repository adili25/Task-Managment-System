using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_Manager.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Roles Role { get; set; }
        
        public ApplicationUser(string fullName, string email, string passwordHash, Roles role)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            Id = Guid.NewGuid();
        }

        //---> add param less constructor for EF
        public ApplicationUser() { } 
    
    }
}
