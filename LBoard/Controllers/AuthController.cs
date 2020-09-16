using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    }
}