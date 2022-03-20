using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Library.Models;
using DataStore.Library.Abstractions;
using DataStore.Library.Data;
using JwtTokens.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace SensorDataStore.WebApi;

public static class Api
{
    public static void ConfigureApi(this WebApplication app)
    {
        app.MapGet("/GetAuthorized", GetAuthorized);//.RequireAuthorization();
        app.MapPost("/PostAuthorized", PostAuthorized);
        app.MapPost("/register", Register);
        app.MapPost("/login", Login);

        app.MapGet("/BMPSensor", Get);
        app.MapPost("/BMPSensor", Post);
    }


    [Authorize(Policy = "GetAccess")]
    private static async Task<IResult> GetAuthorized()//ISensorData sensorData)
    {
        // var results = await sensorData.GetAsync();
        return Results.Ok("Dzieki za pomoc ziomeczku");
            //results == null ? Results.NotFound() : Results.Ok(results);
    }

    [Authorize(Policy = "GetAccess")]
    private static async Task<IResult> PostAuthorized(BmpMeasurementDto measurement)
    {
        return Results.Ok(measurement);
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

        var token = CreateToken(user);
        return Results.Ok(token);
    }

    private static string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "admin")
        };

        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes("GetTheKeyFromTheConfigurationAfterExtraction"));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            "Server", 
            "Client",
            claims: claims, 
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials:credentials
            );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }

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
        (user.PasswordSalt, user.PasswordHash) = CreatePasswordHash(request.Password);
        return Results.Ok(user);
    }
    private static (byte[] passwordSalt, byte[] passwordHash) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        return (hmac.Key, hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
    }
}