using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
string queueName = "hello";

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: queueName , durable: false, exclusive: false, autoDelete: false, arguments: null);

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