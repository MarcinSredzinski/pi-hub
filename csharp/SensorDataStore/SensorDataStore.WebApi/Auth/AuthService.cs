using DataStore.Library.Abstractions;
using JwtAuth.Library.Models;
using JwtAuth.Library.Services;

namespace SensorDataStore.WebApi.Auth
{
    public interface IAuthService
    {
        IResult ValidateUser(UserDto providedUser);
        Task<IResult> Register(UserDto request);
    }

    public class AuthService : IAuthService
    {
        private readonly IIssuerService _issuerService;
        private readonly IUsersStore _usersStore;

        public AuthService(IIssuerService issuerService, IUsersStore usersStore)
        {
            _issuerService = issuerService;
            _usersStore = usersStore;
        }

        public IResult ValidateUser(UserDto providedUser)
        {
            var user = _usersStore.TryGetUser(providedUser.UserName).Result;
            
            if (user == null) return Results.NotFound("User not found.");


            if (!_issuerService.VerifyPasswordHash(providedUser.Password, user.PasswordHash, user.PasswordSalt))
                return Results.NotFound("Invalid password.");

            var token = _issuerService.CreateToken(user);

            return Results.Ok(token);
        }

        public async Task<IResult> Register(UserDto request)
        {
            if (await _usersStore.UserExistsAsync(request.UserName)) return Results.Conflict("User already exists."); 
           
            User user = new User();
            
            user.UserName = request.UserName;
            (user.PasswordSalt, user.PasswordHash) = _issuerService.CreatePasswordHash(request.Password);

            await _usersStore.SaveUser(user);

            return Results.Ok("User created successfuly");
        }
    }
}
