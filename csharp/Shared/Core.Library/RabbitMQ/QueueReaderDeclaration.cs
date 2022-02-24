using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Core.Library.RabbitMQ;

public interface IQueueReaderDeclaration
{
    void Declare(EventHandler<BasicDeliverEventArgs> onReceivedMessageHandler);
}

public class QueueReaderDeclaration : QueueDeclarationBase, IQueueReaderDeclaration
{
    public QueueReaderDeclaration(IConfiguration config, ILogger logger) : base(config, logger)
    {
        Logger = logger;
        if (config == null)
        {
            throw new Exception("Configuration is not loaded properly!");
        }
        Logger.Debug("{0} - instance initialized properly. ", nameof(QueueReaderDeclaration));
    }

    public void Declare(EventHandler<BasicDeliverEventArgs> onReceivedMessageHandler)
    {
        if (Connection == null) throw new ArgumentException($"{nameof(QueueReaderDeclaration)} - {nameof(Declare)}- Connection is null!");

        var channel = Connection.CreateModel();
        {
            channel.QueueDeclare(queue: QueueName, durable: true,            //ToDo set back to false after testing
                exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += onReceivedMessageHandler;
            channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        }
        Logger.Debug("{0} - Queue declared properly. ", nameof(Declare));
    }
}