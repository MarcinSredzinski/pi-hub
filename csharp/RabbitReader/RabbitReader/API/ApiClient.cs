using RabbitReader.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace RabbitReader.API
{
    internal interface IApiClient
    {
        Task<HttpResponseMessage> PostSensorDataAsync(BmpMeasurementDto measurement);
    }

    internal class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _requestUri;
        public ApiClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            string targetUrl = config.GetSection("ApplicationSettings")
                .GetSection("TargetApiUrl").Get<string>();
            _requestUri = new Uri(targetUrl);
        }

        public Task<HttpResponseMessage> PostSensorDataAsync(BmpMeasurementDto measurement)
        {
            var httpContent = JsonContent.Create(measurement);
            return _httpClient.PostAsync(_requestUri, httpContent);
        }
    }
}
