using DataStore.Library.Abstractions;
using DataStore.Library.Data;
using DataStore.Library.DbAccess;

namespace SensorDataStore.WebApi;

internal static class Startup
{
    internal static void ConfigureServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ICouchbaseDataAccess, CouchbaseDataAccess>()
            .AddScoped<ISensorData, SensorData>();
    }
}
