using System;

namespace LBoard.Models.Config
{
    public static class DbConfig
    {
        public static string MySqlDatabase => Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? string.Empty;
        public static string MySqlUser => Environment.GetEnvironmentVariable("MYSQL_USER") ?? string.Empty;
        public static string MySqlPassword => Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? string.Empty;
    }
}