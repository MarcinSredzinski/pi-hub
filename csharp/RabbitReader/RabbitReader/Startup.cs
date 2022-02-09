using Microsoft.Extensions.Configuration;
using Serilog;

namespace RabbitReader
{
    internal static class Startup
    {
        internal static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
        internal static void ConfigureLogger()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string logPath = Path.Combine(basePath, "logs", "my_logNew.log");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
