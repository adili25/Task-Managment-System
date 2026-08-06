namespace Task_managment_system.DTO
{
    public class LoginDto
    {
        public string Email { set; get; }
        public string PasswordHash { set; get; }

        public LoginDto(string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(passwordHash));
            }

            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
