using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Application.DTOs;
using TaskTrackerAPI.Application.Services;

namespace TaskTrackerAPI.API.Controllers
{
    /// <summary>
    /// User Management (Register, Login)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a role
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.Register(model);
            if (result == null)
                return BadRequest("Registration failed.");
            return Ok(new { message = "Role has been created successfully", result });
        }

        /// <summary>
        /// User login
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _authService.Login(model);
            if (response == null)
                return Unauthorized("Invalid username or password.");
            return Ok(new { message = "Login Successful", response });
        }
    }
}
