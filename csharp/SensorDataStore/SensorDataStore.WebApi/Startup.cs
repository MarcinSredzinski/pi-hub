using DataStore.Library.DbAccess;

namespace SensorDataStore.WebApi
{
    internal static class Startup
    {
        internal static void ConfigureServices(this IServiceCollection services)
        {
            services
                .AddScoped<ICouchbaseDataAccess, CouchbaseDataAccess>();
        }
    }
}
