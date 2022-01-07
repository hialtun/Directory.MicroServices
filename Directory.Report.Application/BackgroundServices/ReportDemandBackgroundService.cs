using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicroServices.Infrastructure.MessageBroker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Directory.Report.Application.BackgroundServices
{
    public class ReportDemandBackgroundService: BackgroundService
    {
        private readonly RabbitMQClient _client;
        private const string Queue = "Report";
        private readonly ILogger<ReportDemandBackgroundService> _logger;

        public ReportDemandBackgroundService(RabbitMQClient client, ILogger<ReportDemandBackgroundService> logger)
        {
            _client = client;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"message will be consumed from: {Queue}");

                string message = "";
                using IConnection connection = _client.Factory().CreateConnection();
                using IModel channel = connection.CreateModel();
                channel.QueueDeclare(queue: Queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    
                    _logger.LogInformation($"message has been consumed: {message}");
                };
                channel.BasicConsume(queue: Queue,
                    autoAck: true,
                    consumer: consumer);

                await Task.CompletedTask;
            }
        }
    }
}
