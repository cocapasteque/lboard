using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using LBoard.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LBoard.Services
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseApiKey(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                if (context.Request.Headers["X-API-Key"].Any())
                {
                    // Get API Key from header
                    var headerKey = context.Request.Headers["X-API-Key"].FirstOrDefault();
                    if (!ApiConfig.ApiKey.Equals(headerKey, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Invalid
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("Invalid API Key");
                    }
                    else
                    {
                        // Create API Identity User
                        var identity = new GenericIdentity("API");
                        identity.AddClaim(new Claim("origin", "API"));
                        identity.AddClaim(new Claim("role", "admin"));
                        var principal = new GenericPrincipal(identity, new[] {"admin", "user"});
                        context.User = principal;
                        await next();
                    }
                }
            });
        }
    }
}