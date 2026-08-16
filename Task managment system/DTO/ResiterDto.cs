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
        [RegularExpression(@"^(?=(?:.*\d){2})(?=.*[!@#$%^&*])(?=.{8,})")]
        public string Password { get; set; }

        [Required]
        public Roles Role { get; set; }
    }
}