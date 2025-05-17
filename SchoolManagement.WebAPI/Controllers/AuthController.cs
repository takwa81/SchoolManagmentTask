using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs.Auth;
using SchoolManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SchoolManagement.Application.Responses;

namespace SchoolManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        //public AuthController(IAuthService authService)
        //{
        //    _authService = authService;
        //}

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return StatusCode(result.Code, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return StatusCode(result.Code, result);
        }
      

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            Console.WriteLine($"Email from token: {email}");

            _logger.LogInformation("Email from token: {Email}", email);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Unauthorized access attempt - email claim missing");
                return Unauthorized(ApiResponse<string>.Fail("Unauthorized", 401));
            }

            var result = await _authService.GetProfileAsync(email);
            _logger.LogInformation("Profile response: {@Result}", result);
            return StatusCode(result.Code, result);
        }


    }
}
