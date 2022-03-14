using System.Security.Cryptography;
using Core.Library.Models;
using DataStore.Library.Abstractions;
using DataStore.Library.Data;
using JwtTokens.Library.Models;

namespace SensorDataStore.WebApi;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/BMPSensor", Get);
        app.MapPost("/BMPSensor", Post);
        app.MapPost("/register", Register);
        app.MapPost("/login", Login);
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


    public static async Task<IResult> Login(UserDto request)
    {
        if (!user.UserName.Equals(request.UserName))
            return Results.NotFound("User not found.");


        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return Results.NotFound("Invalid password.");

        return Results.Ok("Token here");
    }

    //Still some bugs. Hash and hash salt doesn't verify well. 
    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
    public static User user = new User();
   
    private static async Task<IResult> Register(UserDto request)
    {
        user.UserName = request.UserName;
        (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash(request.Password);
        return Results.Ok(user);
    }
    private static (byte[] passwordSalt, byte[] passwordHash) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        return (hmac.Key, hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
    }
}