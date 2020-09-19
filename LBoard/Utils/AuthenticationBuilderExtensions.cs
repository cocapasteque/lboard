using System;
using LBoard.Models.Security.ApiKey;
using LBoard.Services.Security.ApiKey;
using Microsoft.AspNetCore.Authentication;

namespace LBoard.Utils
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authBuilder, Action<ApiKeyAuthenticationOptions> options)
        {
            return authBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationOptions.DefaultScheme, options);
        }
    }
}