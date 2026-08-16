using System.ComponentModel.DataAnnotations;

namespace Task_managment_system.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }
    }
}
