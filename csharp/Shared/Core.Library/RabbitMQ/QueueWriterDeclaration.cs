using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;

namespace Core.Library.RabbitMQ;

public interface IQueueWriterDeclaration
{
    void PushMessage(byte[] messageBytes);
}

public class QueueWriterDeclaration : QueueDeclarationBase, IQueueWriterDeclaration
{
    public QueueWriterDeclaration(IConfiguration config, ILogger logger) : base(config, logger)
    {
        Logger = logger;
        if (config == null)
        {
            throw new Exception("Configuration is not loaded properly!");
        }
        Logger.Debug("{0} - instance initialized properly. ", nameof(QueueWriterDeclaration));
    }

    public void PushMessage(byte[] messageBytes)
    {
        using var channel = Connection!.CreateModel();
        channel.QueueDeclare(queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        channel.BasicPublish(exchange: "",
            routingKey: QueueName,
            basicProperties: null,
            body: messageBytes);

        Logger.Debug("{0} - message was sent.", nameof(PushMessage));
    }
}