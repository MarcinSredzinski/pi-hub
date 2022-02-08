using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var applicationSettings = config.GetSection("ApplicationSettings");
string hostName = applicationSettings.GetSection("QueueHostName").Get<string>();
string queueName = applicationSettings.GetSection("QueueName").Get<string>();

var factory = new ConnectionFactory() { HostName = hostName };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Received {0} at {1}", message, DateTime.Now);
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine(" Press enter to exit.");
Console.ReadLine();