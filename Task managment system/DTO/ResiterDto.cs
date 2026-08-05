using System.ComponentModel.DataAnnotations;
using Task_managment_system.Enums;

namespace Task_managment_system.DTO
{
    public class RegisterDto
    {
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Roles Role { get; set; }

        public RegisterDto(string fullName, string email, string passwordHash, Roles role)
        {
            if (fullName == default)
            {
                throw new NullReferenceException("FULLNAME_IS_NULL");
            }
            FullName = fullName;


            if (email == default)
            {
                throw new NullReferenceException("EMAIL_IS_NULL");
            }
            Email = email;


            if (fullName == default)
            {
                throw new NullReferenceException("PASSWORD_IS_NULL");
            }
            PasswordHash = passwordHash;


            if (role == default)
            {
                throw new NullReferenceException("ROLE_IS_NULL");
            }
            Role = role;
        }
    }
}
