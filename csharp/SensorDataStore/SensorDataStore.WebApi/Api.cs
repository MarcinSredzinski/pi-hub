using Core.Library.Models;
using DataStore.Library.Abstractions;
using DataStore.Library.Data;
using JwtAuth.Library.Models;
using Microsoft.AspNetCore.Authorization;
using SensorDataStore.WebApi.Auth;

namespace SensorDataStore.WebApi;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
      //  app.MapPost("/register", Register);
        app.MapPost("/login", Login);

        app.MapGet("/BMPSensor", Get);
        app.MapPost("/BMPSensor", Post);
    }

    private static async Task<IResult> Get(ISensorData sensorData)
    {
        var results = await sensorData.GetAsync();
        return results == null ? Results.NotFound() : Results.Ok(results);
    }

    [Authorize(Policy = "GetAccess")]
    private static async Task<IResult> Post(BmpMeasurementDto measurement, ISensorData sensorData)
    {
        await sensorData.InsertAsync(measurement);
        return Results.Ok();
    }


    public static async Task<IResult> Login(IAuthService authService, UserDto request)
    {
        return authService.ValidateUser(request);
    }


    private static async Task<IResult> Register(IAuthService authService, UserDto request)
    {
        return await authService.Register(request);
    }
}