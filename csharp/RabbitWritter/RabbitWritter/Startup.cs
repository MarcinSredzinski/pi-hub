using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace RabbitWritter
{
    internal static class Startup
    {
        internal static void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .AddJsonFile("appsettings.json");
        }
        internal static ILoggingBuilder ConfigureLogger(this ILoggingBuilder loggingBuilder)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logPath = Path.Combine(basePath, "logs", "my_logNew.log");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .MinimumLevel.Debug()                                               //ToDo make dependent on the current environment.
                .CreateLogger();
            loggingBuilder.AddSerilog(Log.Logger);
            return loggingBuilder;
        }
    }
}
