using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitReader;
using RabbitReader.API;
using RabbitReader.RabbitMQ;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => Startup.ConfigureServices(services))
    .ConfigureAppConfiguration(Startup.BuildConfiguration)
    .Build();

Startup.ConfigureLogger(); //ToDo change Logger configuration/use default. 

var config = host.Services.GetService<IConfiguration>();
var apiHandler = host.Services.GetService<IApiHandler>();


if (config == null)
{
    throw new Exception("Configuration is not loaded properly!");
}

if (apiHandler == null)
{
    throw new Exception("Api handler not initialized properly!");
}

var queue = new QueueDeclaration(config, apiHandler.OnMessageReceived);


Console.WriteLine(" Press enter to exit.");
Console.ReadLine();
