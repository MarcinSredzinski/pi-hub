using DataStore.Library.Abstractions;
using DataStore.Library.Data;
using DataStore.Library.DbAccess;
using Serilog;

namespace SensorDataStore.WebApi;

internal static class Startup
{
    internal static void ConfigureServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddLogging(x => x.ConfigureLogger());

        services
            .AddSingleton<ICouchbaseDataAccess, CouchbaseDataAccess>()
            .AddScoped<ISensorData, SensorData>();
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
