using Microsoft.AspNetCore.Mvc;
using ZombieSurvival.DTOs;
using ZombieSurvival.Services;
namespace ZombieSurvival.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // DI injects the auth service registered in Program.cs
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Creates a new Identity user and returns a JWT
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result is null)
                return BadRequest("Registration failed.");

            return Ok(result);
        }

        // Validates credentials and returns a JWT
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result is null)
                return Unauthorized("Invalid email or password.");

            return Ok(result);
        }
    }
}