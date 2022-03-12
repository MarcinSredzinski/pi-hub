using Core.Library.Models;
using DataStore.Library.Abstractions;
using DataStore.Library.Data;

namespace SensorDataStore.WebApi;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/BMPSensor", Get);
        app.MapPost("/BMPSensor", Post);
    }

    private static async Task<IResult> Get(ISensorData sensorData)
    {
        var results = await sensorData.GetAsync();
        return results == null ? Results.NotFound() : Results.Ok(results);
    }

    private static async Task<IResult> Post(BmpMeasurementDto measurement, ISensorData sensorData)
    {
        await sensorData.InsertAsync(measurement);
        return Results.Ok();
    }
}

