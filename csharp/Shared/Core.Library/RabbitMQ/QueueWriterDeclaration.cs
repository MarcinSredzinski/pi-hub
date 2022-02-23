using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;

namespace Core.Library.RabbitMQ
{
    public interface IQueueWriterDeclaration
    {
        void PushMessage(byte[] messageBytes);
    }


    public class QueueWriterDeclaration : IQueueWriterDeclaration
    {
        private readonly ILogger _logger;
        public string HostName { get; }
        public string QueueName { get; }
        public IConnection? Connection { get; set; }

        public QueueWriterDeclaration(IConfiguration? config, ILogger logger)
        {
            _logger = logger;
            if (config == null)
            {
                throw new Exception("Configuration is not loaded properly!");
            }
            var applicationSettings = config.GetSection("ApplicationSettings");
            HostName = applicationSettings.GetSection("QueueHostName").Get<string>();
            QueueName = applicationSettings.GetSection("QueueName").Get<string>();
            _logger.Debug("{0} - instance initialized properly. ", nameof(QueueReaderDeclaration));
            var factory = new ConnectionFactory() { HostName = HostName };
            Connection = factory.CreateConnection();
        }

        public void PushMessage(byte[] messageBytes)
        {
            using var channel = Connection!.CreateModel();
            channel.QueueDeclare(queue: "hello",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.BasicPublish(exchange: "",
                routingKey: "hello",
                basicProperties: null,
                body: messageBytes);

            _logger.Debug("{0} - message was sent.", nameof(PushMessage));

        }
    }
}
