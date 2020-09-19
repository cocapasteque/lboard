using System;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pastel;

namespace LBoard.Utils
{
    public static class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
    }

    public class AppLoggerProvider : ILoggerProvider
    {
        private IConfiguration _config;
        private bool _traceEnabled;
        
        public AppLoggerProvider(IConfiguration config, bool trace = false)
        {
            _config = config;
            _traceEnabled = trace;
        }
        
        public void Dispose()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new AppLogger(categoryName, _config, _traceEnabled);
        }

        public class AppLogger : ILogger
        {
            private readonly string _category;
            private readonly IConfiguration _config;
            private readonly bool _traceEnabled;

            public AppLogger(string categoryName, IConfiguration config, bool traceEnabled)
            {
                _category = categoryName;
                _config = config;
                _traceEnabled = traceEnabled;
            }
            
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var foreground = Color.Black;
                var background = Color.White;
                if (!IsEnabled(logLevel))
                {
                    return;
                }
                switch(logLevel)
                {
                    case LogLevel.Trace:
                        foreground = Color.Crimson;
                        background = Color.SkyBlue;
                        break;
                    case LogLevel.Debug:
                        foreground = Color.Black;
                        background = Color.White;
                        break;
                    case LogLevel.Information:
                        foreground = Color.White;
                        background = Color.Green;
                        break;
                    case LogLevel.Warning:
                        foreground = Color.Black;
                        background = Color.Yellow;
                        break;
                    case LogLevel.Error:
                        foreground = Color.White;
                        background = Color.Red;
                        break;
                    case LogLevel.Critical:
                        foreground = Color.White;
                        background = Color.DarkRed;
                        break;
                    case LogLevel.None:
                        break;
                }
                Console.WriteLine($"[{logLevel}]".Pastel(foreground).PastelBg(background) 
                                  + $" - ({_category})[{eventId.Id}] - {formatter(state, exception)}");
                
                if (_traceEnabled && exception != null)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(exception.StackTrace);
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                if (!Enum.TryParse(_config.GetSection("LogLevel").GetSection("System").Value,
                    out LogLevel configLevel)) return false;
                
                if (configLevel == LogLevel.Trace || _traceEnabled) return true;
                if (configLevel == LogLevel.None) return false;
                return logLevel >= configLevel && logLevel < LogLevel.None;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}