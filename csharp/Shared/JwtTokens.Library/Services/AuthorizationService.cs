using JwtAuth.Library.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net.Http.Json;

namespace JwtAuth.Library.Services
{
    public interface IAuthorizationService
    {
        Task<string> Authorize();
    }

    public class AuthorizationService : IAuthorizationService
    {

        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly UserDto _userDto;
        private readonly string authUrl;

        public AuthorizationService(HttpClient httpClient, IConfiguration config, ILogger logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            authUrl = config.GetSection("ApplicationSettings")
                .GetSection("AuthorizationApiUrl").Value;

            _userDto = new UserDto()
            {
                UserName = config.GetSection("ApplicationSettings")
                .GetSection("UserName").Value,
                Password = config.GetSection("ApplicationSettings")
                .GetSection("Password").Value
            };

            _logger.Debug("{0} - instance initialized properly. ", nameof(AuthorizationService));
        }

        /*
         1. You don't have a token:
            - you send your login and password and retrive token
            - you send your request with the token 
         2. You get the response: 
            - if response code = 200, proceed, 
            - if response code = 403
                - go to step 1
         */
        public async Task<string> Authorize()
        {
            var httpContent = JsonContent.Create(_userDto);
            var response = await _httpClient.PostAsync(authUrl, httpContent);
            if (response == null)
            {
                throw new Exception("Remote server failed to respond.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jwt = await response.Content.ReadFromJsonAsync<string>();
                if (string.IsNullOrEmpty(jwt))
                {
                    throw new Exception($"{nameof(Authorize)} - Failed to obtain JWT Token. ");
                }
                _logger.Debug("{0} - Obtained JWT Token. ", nameof(Authorize));
                return jwt;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Provided wrong address to remote server. ");
            }
            throw new Exception("Something went wrong while trying to authorize. Check your username and password. ");
        }
    }
}
