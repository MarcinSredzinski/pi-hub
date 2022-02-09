using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitReader;
using RabbitReader.RabbitMQ;
using System.Text;

IConfiguration config = Startup.BuildConfiguration();
Startup.ConfigureLogger();


var applicationSettings = config.GetSection("ApplicationSettings");
string targetApiUrl = applicationSettings.GetSection("TargetApiUrl").Get<string>();

var queue = new QueueDeclaration(config, OnMessageReceived);


Console.WriteLine(" Press enter to exit.");
Console.ReadLine();


static void OnMessageReceived(object? model, BasicDeliverEventArgs ea)
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Received {0} at {1}", message, DateTime.Now);
}