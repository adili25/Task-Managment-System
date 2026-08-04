using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;
using Task_managment_system.Repositries;
using System.Threading.Tasks;
using Task_managment_system.DTO;
using BCrypt.Net;

namespace Task_managment_system.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
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

            var newUser = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                Role = request.Role
            };

            _userRepo.AddUser(request);

            return Ok(request);

        }
    }
}
