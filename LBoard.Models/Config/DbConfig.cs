using System;

namespace LBoard.Models.Config
{
    public static class DbConfig
    {
        public static string MySqlServer => Environment.GetEnvironmentVariable("MYSQL_SERVER") ?? "localhost";
        public static string MySqlDatabase => Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "lboard";
        public static string MySqlUser => Environment.GetEnvironmentVariable("MYSQL_USER") ?? "admin";
        public static string MySqlPassword => Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "admin";
    }
}