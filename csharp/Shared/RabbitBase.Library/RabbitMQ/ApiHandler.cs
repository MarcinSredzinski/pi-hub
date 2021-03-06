using System.Text;
using System.Text.Json;
using Core.Library.Api;
using Core.Library.Models;
using RabbitMQ.Client.Events;
using Serilog;

namespace RabbitBase.Library.RabbitMQ;

public interface IApiHandler
{
    void OnMessageReceived(object? model, BasicDeliverEventArgs ea);
}

public class ApiHandler : IApiHandler
{
    private readonly IApiClient _apiClient;
    private readonly ILogger _logger;

    public ApiHandler(IApiClient apiClient, ILogger logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public void OnMessageReceived(object? model, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        _logger.Debug("{0} - Received {1} at {2}", nameof(ApiHandler), message, DateTime.Now);
        var measurement = JsonSerializer.Deserialize<BmpMeasurementDto>(body);
        _logger.Debug("{0} - Message deserialized properly", nameof(ApiHandler));
        if (measurement != null)
        {
            var res = _apiClient.PostSensorDataAsync(measurement);
        }
    }
}