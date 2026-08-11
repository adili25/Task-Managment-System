namespace Task_managment_system.DTO
{
    public class LoginDto
    {
        public string Email { set; get; }
        public string Password { set; get; }

        public LoginDto(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            }

            Email = email;
            Password = password;
        }
    }
}
