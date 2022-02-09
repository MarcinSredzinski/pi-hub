using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitReader.RabbitMQ
{
    internal class QueueDeclaration
    {
        public QueueDeclaration(IConfiguration config, EventHandler<BasicDeliverEventArgs> onReceivedMessageHandler)
        {
            var applicationSettings = config.GetSection("ApplicationSettings");
            HostName = applicationSettings.GetSection("QueueHostName").Get<string>();
            QueueName = applicationSettings.GetSection("QueueName").Get<string>();
            Declare(onReceivedMessageHandler);
        }

        public string HostName { get; }
        public string QueueName { get; }
        public IConnection? Connection { get; set; }


        private void Declare(EventHandler<BasicDeliverEventArgs> onReceivedMessageHandler)
        {
            var factory = new ConnectionFactory() { HostName = HostName };
            Connection = factory.CreateConnection();
            var channel = Connection.CreateModel();
            {
                channel.QueueDeclare(queue: QueueName, durable: true, //ToDo set back to false after testing
                               exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += onReceivedMessageHandler;
                channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
            }
        }
    }
}
