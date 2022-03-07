using Core.Library.Models;
using DataStore.Library.DbAccess;

namespace SensorDataStore.WebApi
{
    public static class Api
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapGet("/BMPSensor", Get);
            app.MapPost("/BMPSensor", Post);
        }

        private static async Task<IResult> Get(ICouchbaseDataAccess couchbaseDataAccess)
        {
            var results = couchbaseDataAccess.LoadData();
            return results == null ? Results.NotFound() : Results.Ok(results);
        }

        private static async Task Post()
        {
            throw new NotImplementedException();
        }


    }
}
