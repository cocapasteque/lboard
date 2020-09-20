using System;

namespace LBoard.Models.Config
{
    public static class RedisConfig
    {
        public static string Address => Environment.GetEnvironmentVariable("REDIS_ADDRESS") ?? "localhost";
        public static string Port => Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
        public static string Password => Environment.GetEnvironmentVariable("REDIS_PASSWORD") ?? "admin";
        public static int Database => 
            int.TryParse(Environment.GetEnvironmentVariable("REDIS_DB"), out int db) ? db : 0;
    }
}