using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_managment_system.DTO
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public Roles Role { get; set; }
    }
}