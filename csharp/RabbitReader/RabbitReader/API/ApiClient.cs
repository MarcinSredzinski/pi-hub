using Core.Library.Api;
using Core.Library.Models;
using JwtAuth.Library.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net.Http.Json;

namespace RabbitReader.API
{
    internal class ApiClient : IApiClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly Uri _requestUri;
        private readonly IClientService _clientService;
        private string JWTToken = string.Empty;
        private int TimesRetried = 0;
        private int RetriesLimit = 10;

        public ApiClient(HttpClient httpClient, IConfiguration config, ILogger logger, IClientService clientService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _clientService = clientService;
            AuthorizeHttpClient();
            string targetUrl = config.GetSection("ApplicationSettings")
                .GetSection("TargetApiUrl").Get<string>();
            _requestUri = new Uri(targetUrl);
            _logger.Debug("{0} - instance initialized properly. ", nameof(ApiClient));
        }

        private void AuthorizeHttpClient()
        {
            TimesRetried++;
            JWTToken = _clientService.Authorize().Result;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTToken.ToString());
        }

        public async Task<HttpResponseMessage> PostSensorDataAsync(BmpMeasurementDto measurement)
        {
            var httpContent = JsonContent.Create(measurement);
            _logger.Debug("{0} - http content created properly. ", nameof(PostSensorDataAsync));
            HttpResponseMessage response = await _httpClient.PostAsync(_requestUri, httpContent);

            if (TimesRetried == RetriesLimit)
                throw new Exception("Client got stuck in endless loop. Credentials invalid or server malfunctioning");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.Debug("{0} - Token expired. ", nameof(PostSensorDataAsync));
                AuthorizeHttpClient();
                response = await PostSensorDataAsync(measurement);
            }

            return response;
        }
    }
}
