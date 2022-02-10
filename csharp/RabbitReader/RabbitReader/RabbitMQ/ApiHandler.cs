using RabbitMQ.Client.Events;
using RabbitReader.API;
using RabbitReader.Models;
using System.Text;
using System.Text.Json;

namespace RabbitReader.RabbitMQ;

internal interface IApiHandler
{
    void OnMessageReceived(object? model, BasicDeliverEventArgs ea);
}

internal class ApiHandler : IApiHandler
{
    private readonly IApiClient _apiClient;
    public ApiHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public void OnMessageReceived(object? model, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var measurement = JsonSerializer.Deserialize<BmpMeasurementDto>(body);
        Console.WriteLine("Received {0} at {1}", message, DateTime.Now);
        if (measurement != null)
        {
            var res = _apiClient.PostSensorDataAsync(measurement);
        }
    }
}