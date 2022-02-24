using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;

namespace Core.Library.RabbitMQ;

public abstract class QueueDeclarationBase
{
    protected ILogger Logger;

    protected QueueDeclarationBase(IConfiguration config, ILogger logger)
    {
        this.Logger = logger;
        var applicationSettings = config.GetSection("ApplicationSettings");
        HostName = applicationSettings.GetSection("QueueHostName").Get<string>();
        QueueName = applicationSettings.GetSection("QueueName").Get<string>();
        Logger.Debug("{0} - instance initialized properly. ", nameof(QueueReaderDeclaration));
        var factory = new ConnectionFactory() { HostName = HostName };
        Connection = factory.CreateConnection();
    }

    public string HostName { get; }
    public string QueueName { get; }
    public IConnection? Connection { get; set; }

    ~QueueDeclarationBase()
    {
        Connection?.Close();
    }
}