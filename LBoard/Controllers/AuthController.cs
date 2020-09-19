using System.Linq;
using System.Threading.Tasks;
using LBoard.Models;
using LBoard.Services.Security.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace LBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenProvider<IdentityUser> _tokenProvider;
        private readonly ILogger _logger;

        public AuthController(UserManager<IdentityUser> userManager, IJwtTokenProvider<IdentityUser> tokenProvider,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _tokenProvider = tokenProvider;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Auth.RegisterRequest request)
        {
            _logger.LogInformation($"Received register request from {request.Username}");
            if (!ModelState.IsValid || request == null)
            {
                return new BadRequestObjectResult(new {Message = "User Registration Failed"});
            }

            var identityUser = new IdentityUser() {UserName = request.Username, Email = request.Email};
            var result = await _userManager.CreateAsync(identityUser, request.Password);
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (var error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                _logger.LogError($"Registration failed for {request.Username} : " +
                                 $"{string.Join(',', result.Errors.Select(x => x.Description))}");

                return new BadRequestObjectResult(new {Message = "User Registration Failed", Errors = dictionary});
            }

            _logger.LogInformation($"Registration successful for {request.Username}");
            return Ok(new {Message = "User Registration Successful"});
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Auth.LoginRequest request)
        {
            IdentityUser identityUser;

            _logger.LogInformation($"Login request from {request.Username}");

            if (!ModelState.IsValid || request == null || (identityUser = await ValidateUser(request)) == null)
            {
                _logger.LogError($"Login failed for {request.Username}");
                return new BadRequestObjectResult(new {Message = "Login failed"});
            }

            var newToken = _tokenProvider.GenerateToken(identityUser);

            _logger.LogInformation($"Login successful for {request.Username}");
            return Ok(new {Token = newToken, Message = "Success"});
        }

        private async Task<IdentityUser> ValidateUser(Auth.LoginRequest credentials)
        {
            var identityUser = await _userManager.FindByNameAsync(credentials.Username);
            if (identityUser == null) return null;

            var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash,
                credentials.Password);
            return result == PasswordVerificationResult.Failed ? null : identityUser;
        }
    }
}