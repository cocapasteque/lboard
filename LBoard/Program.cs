using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Utils;

namespace LBoard
{
    public class Program
    {
        private static string trace => Environment.GetEnvironmentVariable("TRACE_OUTPUT") ?? "false";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    var config = hostingContext.Configuration.GetSection("Logging");
                    logging.AddConfiguration(config);
                    logging.ClearProviders();
                    logging.AddProvider(new AppLoggerProvider(config, bool.Parse(trace)));
                })
                .UseStartup<Startup>();
    }
}