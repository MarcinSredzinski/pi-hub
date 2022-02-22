using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core.Library.RabbitMQ;
using RabbitWriter;
using Core.Library.RabbitMQ;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(Startup.BuildConfiguration)
    .ConfigureLogging(x => x.ConfigureLogger())
    .ConfigureServices((_, services) => Startup.ConfigureServices(services))
    .Build();

var queueWriter = host.Services.GetService<IQueueWriterDeclaration>();