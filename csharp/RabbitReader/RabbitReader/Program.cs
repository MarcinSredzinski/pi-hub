using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitReader;
using RabbitReader.RabbitMQ;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => Startup.ConfigureServices(services))
    .ConfigureAppConfiguration(Startup.BuildConfiguration)
    .Build();

Startup.ConfigureLogger(); //ToDo change Logger configuration/use default. 

var apiHandler = host.Services.GetService<IApiHandler>();
var queue = host.Services.GetService<IQueueDeclaration>();
if (apiHandler == null || queue == null)
{
    throw new Exception("Queue or api handler did not initialize properly.");
}

queue.Declare(apiHandler.OnMessageReceived);

Console.WriteLine(" Press enter to exit.");
Console.ReadLine();
