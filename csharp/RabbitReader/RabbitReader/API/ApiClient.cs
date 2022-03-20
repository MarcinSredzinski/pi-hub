using System.Net.Http.Json;
using Core.Library.Api;
using Core.Library.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace RabbitReader.API
{
    internal class ApiClient : IApiClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly Uri _requestUri;
        public ApiClient(HttpClient httpClient, IConfiguration config, ILogger logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJleHAiOjE2NDc3OTAzNzUsImlzcyI6IlNlcnZlciIsImF1ZCI6IkNsaWVudCJ9.knbBZXe_WLMJ1C50rH5RnxGS-nJz0ACcSRoM0ndfIi7c7Sn7TQ6yWECLPEpXGzHAjw0GTT9U5a3m7OiRRwinig");
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
