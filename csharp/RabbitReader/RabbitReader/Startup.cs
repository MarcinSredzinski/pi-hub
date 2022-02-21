using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitReader.API;
using RabbitReader.RabbitMQ;
using Serilog;
using Serilog.Core;
using ILogger = Serilog.ILogger;

namespace RabbitReader
{
    internal static class Startup
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<ILogger>(x=> Log.Logger)
                .AddSingleton<HttpClient>()
                .AddSingleton<IApiClient, ApiClient>()
                .AddSingleton<IApiHandler, ApiHandler>()
                .AddSingleton<IQueueDeclaration, QueueDeclaration>();
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
                .MinimumLevel.Debug()                                               //ToDo make dependent on the current environment.
            .CreateLogger();
            loggingBuilder.AddSerilog(Log.Logger);
            return loggingBuilder;
        }
    }
}
