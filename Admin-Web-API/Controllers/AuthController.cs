using Admin_Portal.Application.Admin.Interfaces;
using Admin_Portal.Application.Admin.Services;
using Admin_Portal.Contracts.Admin.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Admin_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAdminAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAdminAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new admin user
        /// </summary>
        /// <param name="request">Registration details (username, email, password, role)</param>
        /// <returns>Registration response with user details or errors</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid registration request model state");
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Registration attempt for username: {request.Username}");
            var response = await _authService.RegisterAsync(request);

            if (!response.Success)
            {
                _logger.LogWarning($"Registration failed for username: {request.Username}. Reason: {response.Message}");
                return BadRequest(response);
            }

            _logger.LogInformation($"User registered successfully: {request.Username}");
            return Ok(response);
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        /// <param name="request">Login credentials (username, password)</param>
        /// <returns>Login response with user details or errors</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login request model state");
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Login attempt for username: {request.Username}");
            var response = await _authService.LoginAsync(request);

            if (!response.Success)
            {
                _logger.LogWarning($"Login failed for username: {request.Username}. Reason: {response.Message}");
                return Unauthorized(response);
            }

            _logger.LogInformation($"User logged in successfully: {request.Username}");
            return Ok(response);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _authService.GetAllUsers();

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
