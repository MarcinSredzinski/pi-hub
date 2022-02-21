using RabbitReader.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace RabbitReader.API
{
    internal interface IApiClient
    {
        Task<HttpResponseMessage> PostSensorDataAsync(BmpMeasurementDto measurement);
    }

    internal class ApiClient : IApiClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly Uri _requestUri;
        public ApiClient(HttpClient httpClient, IConfiguration config, ILogger logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            string targetUrl = config.GetSection("ApplicationSettings")
                .GetSection("TargetApiUrl").Get<string>();
            _requestUri = new Uri(targetUrl);
            _logger.Debug("{0} - instance initialized properly. ", nameof(ApiClient));
        }

        public Task<HttpResponseMessage> PostSensorDataAsync(BmpMeasurementDto measurement)
        {
            var httpContent = JsonContent.Create(measurement);
            _logger.Debug("{0} - http content created properly. ", nameof(PostSensorDataAsync));
            return _httpClient.PostAsync(_requestUri, httpContent);
        }
    }
}
