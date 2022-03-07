using Core.Library.Models;

namespace SensorDataStore.WebApi
{
    public static class Api
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapGet("/BMPSensor", Get);
            app.MapPost("/BMPSensor", Post);
        }

        private static async Task<IEnumerable<BmpMeasurementDto>> Get()
        {

        }

        private static async Task Post()
        {

        }


    }
}
