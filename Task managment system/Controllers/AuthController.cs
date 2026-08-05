using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;
using Task_managment_system.Repositries;
using System.Threading.Tasks;
using Task_managment_system.DTO;
using BCrypt.Net;
using Task_managment_system.Services;

namespace Task_managment_system.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices authServices;
        private readonly UserRepository _userRepo;
        private readonly ILogger<AuthController> logger;

        public AuthController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterDto request)
        {
            logger.LogInformation("--> statring the registeration");

            if(request == default)
            {
                logger.LogError("<-- the request payload is not valid");
                return BadRequest("the request is Null");
            }

            logger.LogInformation("creating new User");
            var newUser = new ApplicationUser(
                request.FullName,
                request.Email,
                BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                request.Role
            );

            logger.LogInformation("adding the User");
            _userRepo.AddUser(newUser);

            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto request)
        {
            if (request == default)
            {
                logger.LogError("<-- the request payload is not valid");
                return BadRequest("the request is Null");
            }

            var LoginUser = _userRepo.GetUserByEmail(request.Email);

            if (LoginUser == default)
            {
                return BadRequest("Unauthrized User");
            }    

            if (LoginUser.PasswordHash != request.PasswordHash)
            {
                BadRequest("Password Not Valid");
            }

            var JwtToken = authServices.GenerateJwtToken(LoginUser.Id.ToString(), LoginUser.Email, LoginUser.Role);

            return Ok(new
            {
                token = JwtToken,
                message = "Authentication Succussful"
            });
        }
    }
}
