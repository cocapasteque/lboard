using System;

namespace LBoard.Models.Config
{
    public static class ApiConfig
    {
        public static string ApiKey => Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty;
        public static string JwtSecretKey => Environment.GetEnvironmentVariable("JWT_SECRET") ?? "LongDefaultSecretKeyForEncryption";
        public static string JwtAudience => Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? string.Empty;
        public static string JwtIssuer => Environment.GetEnvironmentVariable("JWT_ISSUER") ?? string.Empty;
        public static string JwtExp => Environment.GetEnvironmentVariable("JWT_EXPIRY") ?? "900";
    }
}