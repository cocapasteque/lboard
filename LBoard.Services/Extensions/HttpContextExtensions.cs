using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace LBoard.Services.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserName(this HttpContext context)
        {
            return context.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}