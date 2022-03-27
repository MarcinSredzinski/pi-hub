using Core.Library.Models;
using System.Text;
using System.Text.Json;
using RabbitBase.Library.RabbitMQ;
using ILogger = Serilog.ILogger;

namespace RabbitWriter.Writers
{
    internal class SimpleDataWriter
    {
        private readonly IQueueWriterDeclaration _queueWriterDeclaration;
        private readonly ILogger _logger;

        public SimpleDataWriter(IQueueWriterDeclaration queueWriterDeclaration, ILogger logger)
        {
            _queueWriterDeclaration = queueWriterDeclaration;
            _logger = logger;
        }

        internal void WriteToQueue()
        {
            for (int i = 0; i < 2; i++)
            {
                var message = JsonSerializer.Serialize(new BmpMeasurementDto()
                {
                    DateTime = DateTime.Now,
                    Pressure = 1010 + i,
                    Temperature = 27
                });
                var body = Encoding.UTF8.GetBytes(message);
                _logger.Debug("{0} - created a message: {1} ready to be pushed.", nameof(WriteToQueue), message);

                _queueWriterDeclaration.PushMessage(body);
            }
        }
    }
}
