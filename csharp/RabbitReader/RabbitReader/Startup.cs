using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitReader.API;
using RabbitReader.RabbitMQ;
using Serilog;

namespace RabbitReader
{
    internal static class Startup
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<HttpClient>()
                .AddSingleton<IApiClient, ApiClient>()
                .AddSingleton<IApiHandler, ApiHandler>()
                .AddSingleton<IQueueDeclaration, QueueDeclaration>();
        }
        internal static void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .AddJsonFile("appsettings.json");
        }
        internal static void ConfigureLogger() //ToDo start using logger (and check if it really works)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logPath = Path.Combine(basePath, "logs", "my_logNew.log");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
          .CreateLogger();
        }
    }
}
