using System;

namespace LBoard.Models
{
    public static class RedisConfig
    {
        public static string Address => Environment.GetEnvironmentVariable("REDIS_ADDRESS") ?? "localhost";
        public static string Port => Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
        public static int Database => int.TryParse(Environment.GetEnvironmentVariable("REDIS_DB"), out int db) ? db : 0;
        public static string BoardKey => Environment.GetEnvironmentVariable("REDIS_BKEY") ?? "lboard";
        public static bool AllowMultiple => bool.TryParse(Environment.GetEnvironmentVariable("REDIS_LBOARD_MULTI"), out bool multi) && multi;
    }
}