using Core.Library.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace RabbitWriter
{
    internal static class Startup
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<ILogger>(x => Log.Logger)
                .AddSingleton<IQueueWriterDeclaration, QueueWriterDeclaration>();
        }

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
                .MinimumLevel.Debug()                                               
                .CreateLogger();
            loggingBuilder.AddSerilog(Log.Logger);
            return loggingBuilder;
        }
    }
}
