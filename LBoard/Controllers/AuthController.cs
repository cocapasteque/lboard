using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private ILogger _logger;

        public AuthController(UserManager<IdentityUser> userManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
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

            //var token = GenerateToken(identityUser);
            _logger.LogInformation("New token generation");
            await _userManager.RemoveAuthenticationTokenAsync(identityUser, "lboard", "login");
            var newToken = await _userManager.GenerateUserTokenAsync(identityUser, "lboard", "login");
            await _userManager.SetAuthenticationTokenAsync(identityUser, "lboard", "login", newToken);
            
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

        private object GenerateToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(ApiConfig.JwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddSeconds(double.Parse(ApiConfig.JwtExp)), //TODO: Try parse
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = ApiConfig.JwtAudience,
                Issuer = ApiConfig.JwtIssuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}