using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Services;

namespace Task_managment_system.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _authServices;
        private readonly UserServices _userServices;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthServices authServices, UserServices userServices, ILogger<AuthController> logger)
        {
            _userServices = userServices;
            _authServices = authServices;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterDto registerRequest)
        {
            _logger.LogInformation("--> statring the registeration");

            if(registerRequest == default)
            {
                _logger.LogWarning("<-- the payload is not valid");
                return BadRequest("the registerRequest is empty(null)");
            }

            _logger.LogInformation("--> creating new User");
            var newUser = new ApplicationUser(
                registerRequest.FullName,
                registerRequest.Email,
                BCrypt.Net.BCrypt.HashPassword(registerRequest.PasswordHash),
                registerRequest.Role
            );

            _logger.LogInformation("--> adding the User");

            _userServices.AddUser(newUser);
            
            return Ok(new 
            {
                message = "registration succussful",
                userId = newUser.Id,
                fullName = newUser.FullName,
                role = newUser.Role
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginRequest)
        {
            _logger.LogInformation("--> statring the login with email = {email}", loginRequest.Email);

            if (loginRequest == default)
            {
                _logger.LogError("<-- the payload is not valid");
                return BadRequest("the loginRequest is empty(null)");
            }

            var loginUser = _userServices.GetUserByEmail(loginRequest.Email);

            if (loginUser == default || !BCrypt.Net.BCrypt.Verify(loginRequest.PasswordHash, loginUser.PasswordHash))
            {
                return Unauthorized("Unauthorized User");
            }    

            var JwtToken = _authServices.GenerateJwtToken(loginUser.Id.ToString(), loginUser.Email, loginUser.Role);

            if (JwtToken == null)
            {
                return BadRequest("missing information");
            }
            
            return Ok(new
            {
                message = "Authentication Succussful",
                token = JwtToken
            });
        }
    }
}
