using System.Security.Cryptography;
using JwtTokens.Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SensorDataStore.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class AuthController : ControllerBase
    //{
    //    public static User user = new User();

    //    [HttpPost("register")]
    //    public async Task<IActionResult> Register(UserDto request)
    //    {
    //        user.UserName = request.UserName;
    //        //   (user.Password, user.PasswordSalt) =  CreatePasswordHash(request.Password);
    //        var pwd = CreatePasswordHash(request.Password);
    //        user.Password = pwd.passwordHash;
    //        user.PasswordSalt = pwd.passwordSalt;
    //        return Ok(user);
    //    }

    //    private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
    //    {
    //        using var hmac = new HMACSHA512();
    //        return (hmac.Key, hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
    //    }
    //}
}
