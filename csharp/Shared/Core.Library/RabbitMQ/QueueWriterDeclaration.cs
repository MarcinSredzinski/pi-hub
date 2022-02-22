using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Core.Library.RabbitMQ
{
    public interface IQueueWriterDeclaration
    {

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
        }

        //ToDo Put here the writing logic. Consult with https://www.rabbitmq.com/channels.html
        //public void Declare(EventHandler<BasicDeliverEventArgs> onReceivedMessageHandler)
        //{
        //    var factory = new ConnectionFactory() { HostName = HostName };
        //    Connection = factory.CreateConnection();
        //    var channel = Connection.CreateModel();
        //    {
        //        channel.QueueDeclare(queue: QueueName, durable: true, //ToDo set back to false after testing
        //            exclusive: false, autoDelete: false, arguments: null);
        //        var consumer = new EventingBasicConsumer(channel);
        //        consumer.Received += onReceivedMessageHandler;
        //        channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        //    }
        //    _logger.Debug("{0} - Queue declared properly. ", nameof(Declare));
        //}
    }
}
