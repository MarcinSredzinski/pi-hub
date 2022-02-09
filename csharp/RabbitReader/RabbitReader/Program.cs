using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitReader;
using RabbitReader.RabbitMQ;
using System.Text;
using System.Text.Json;

IConfiguration config = Startup.BuildConfiguration();
Startup.ConfigureLogger();
HttpClient client = new HttpClient();


var applicationSettings = config.GetSection("ApplicationSettings");
string targetApiUrl = applicationSettings.GetSection("TargetApiUrl").Get<string>();

var queue = new QueueDeclaration(config, OnMessageReceived);


Console.WriteLine(" Press enter to exit.");
Console.ReadLine();


static void OnMessageReceived(object? model, BasicDeliverEventArgs ea)
{
    //ToDo follow acordingly: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var serializedResponse = JsonSerializer.Serialize(body);
    Console.WriteLine("Received {0} at {1}", message, DateTime.Now);
}