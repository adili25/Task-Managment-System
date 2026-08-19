using Microsoft.AspNetCore.Mvc;
using Task_Manager.Models;
using Task_managment_system.DTO;
using Task_managment_system.Exceptions;
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
        public async Task<ActionResult> RegisterNewUser([FromBody] RegisterDto registerRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--> statring the registeration");
            await _userServices.CheckRegistedEmail(registerRequest.Email, cancellationToken);

            _logger.LogInformation("--> creating new User");
            var newUser = new ApplicationUser(
                registerRequest.FullName,
                registerRequest.Email,
                BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                registerRequest.Role
            );

            _logger.LogInformation("--> adding the User");
            await _userServices.AddUser(newUser, cancellationToken);
            
            return Ok(new 
            {
                message = "---registration succussful---",
                userId = newUser.Id,
                fullName = newUser.FullName,
                role = newUser.Role
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginDto loginRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"--> statring the login with email = {loginRequest.Email}");

            var user = await _userServices.UserVarification(loginRequest.Email, loginRequest.Password, cancellationToken); 

            var JwtToken = _authServices.GenerateJwtToken(user.Id.ToString(), user.Email, user.Role);

            var rawRefreshToken = await _authServices.IssueRefreshToken(user.Id, cancellationToken);

            return Ok(new
            {
                message = "---Authentication Succussful---",
                token = JwtToken,
                refresh_token = rawRefreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshAccessToken([FromBody] RefreshDto refreshRequest, CancellationToken cancellationToken)
        {
            var (userId, newRawToken) = await _authServices.RotateRefreshToken(refreshRequest.RefreshToken, cancellationToken);

            var user = await _userServices.GetUserById(userId, cancellationToken)
                ?? throw new NotFoundException("the user was not found");

            var jwtToken = _authServices.GenerateJwtToken(user.Id.ToString(), user.Email, user.Role);

            return Ok(new
            {
                token = jwtToken,
                refresh_token = newRawToken
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromBody] RefreshDto refreshRequest, CancellationToken cancellationToken)
        {
            await _authServices.RevokeRefreshToken(refreshRequest.RefreshToken, cancellationToken);
            return Ok(new { message = "---logged out---" });
        }

    }
}
