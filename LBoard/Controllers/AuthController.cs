using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace LBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Auth.RegisterRequest request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return new BadRequestObjectResult(new {Message = "User Registration Failed"});
            }

            var identityUser = new IdentityUser() {UserName = request.Username, Email = request.Email};
            var result = await userManager.CreateAsync(identityUser, request.Password);
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (var error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new {Message = "User Registration Failed", Errors = dictionary});
            }

            return Ok(new {Message = "User Registration Successful"});
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Auth.LoginRequest request)
        {
            IdentityUser identityUser;

            if (!ModelState.IsValid || request == null || (identityUser = await ValidateUser(request)) == null)
            {
                return new BadRequestObjectResult(new {Message = "Login failed"});
            }

            var token = GenerateToken(identityUser);
            return Ok(new {Token = token, Message = "Success"});
        }

        private async Task<IdentityUser> ValidateUser(Auth.LoginRequest credentials)
        {
            var identityUser = await userManager.FindByNameAsync(credentials.Username);
            if (identityUser == null) return null;

            var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash,
                credentials.Password);
            return result == PasswordVerificationResult.Failed ? null : identityUser;
        }

        private object GenerateToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ApiConfig.JwtSecretKey);

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