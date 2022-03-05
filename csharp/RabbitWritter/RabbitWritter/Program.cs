using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitBase.Library.RabbitMQ;
using RabbitWriter;
using RabbitWriter.Writers;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(Startup.BuildConfiguration)
    .ConfigureLogging(x => x.ConfigureLogger())
    .ConfigureServices((_, services) => Startup.ConfigureServices(services))
    .Build();

var queueWriter = host.Services.GetService<IQueueWriterDeclaration>();
var logger = host.Services.GetService<ILogger>();

var dataWriter = new SimpleDataWriter(queueWriter, logger);
dataWriter.WriteToQueue();