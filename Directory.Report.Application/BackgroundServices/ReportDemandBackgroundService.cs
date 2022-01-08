using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Directory.Report.Application.Handlers.Command;
using Directory.Report.Application.Handlers.Event;
using Directory.Report.Application.RestClients;
using Directory.Report.Application.RestClients.Model;
using Directory.Report.Domain.Entities;
using MediatR;
using MicroServices.Infrastructure.MessageBroker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Directory.Report.Application.BackgroundServices
{
    public class ReportDemandBackgroundService: BackgroundService
    {
        private readonly RabbitMQClient _client;
        private const string Queue = "Report";
        private readonly ILogger<ReportDemandBackgroundService> _logger;
        private readonly IContactApiClient _contactApiClient;
        private readonly IMediator _mediator;
        
        public ReportDemandBackgroundService(RabbitMQClient client, ILogger<ReportDemandBackgroundService> logger,
            IContactApiClient contactApiClient, IMediator mediator)
        {
            _client = client;
            _logger = logger;
            _contactApiClient = contactApiClient;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"message will be consumed from: {Queue}");

                var message = "";
                using var connection = _client.Factory().CreateConnection();
                using var channel = connection.CreateModel();
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
                    HandleEvent();
                };
                channel.BasicConsume(queue: Queue,
                    autoAck: true,
                    consumer: consumer);

                await Task.Delay(2000, stoppingToken);
            }
        }

        private void HandleEvent()
        {
            var createReportCmd = new CreateReportCommand()
            {
                Status = EReportStatus.InProgress,
                DemandDatetime = DateTime.Now
            };
            var createReportResponse = _mediator.Send(createReportCmd).GetAwaiter().GetResult();
            
            var contactList = _contactApiClient.All().GetAwaiter().GetResult();
            _logger.LogInformation($"contacts: {JsonConvert.SerializeObject(contactList)}");  
            
            var locationList = contactList
                .Where(c => c.ContactInfoList != null)
                .SelectMany(c => c.ContactInfoList)
                .Where(i => i.InfoType == EInfoType.Location)
                .Select(i => i.Value)
                .Distinct().ToList();
            _logger.LogInformation($"locations: {JsonConvert.SerializeObject(locationList)}");
            
            var reportDetails = new List<ReportDetail>();
            foreach (var location in locationList)
            {
                var contactsInLocation = contactList
                    .Where(c => c.ContactInfoList != null && c.ContactInfoList.Any(o => o.Value == location))
                    .Select(c => c).Distinct().ToList();
                _logger.LogInformation($"contactsInLocation: {JsonConvert.SerializeObject(contactsInLocation)}");

                var phoneCount = contactsInLocation
                    .SelectMany(c => c.ContactInfoList)
                    .Where(i => i.InfoType == EInfoType.Phone)
                    .Select(i => i).Distinct().Count();
                
                var reportItem = new ReportDetail()
                {
                    Location = location,
                    ContactCount = contactsInLocation.Count,
                    ContactPhoneCount = phoneCount
                };
                reportDetails.Add(reportItem);
            }

            var updateReportCmd = new UpdateReportCommand()
            {
                Id = createReportResponse.Model.Id,
                Status = EReportStatus.Completed,
                DemandDatetime = createReportCmd.DemandDatetime,
                ReportDetail = reportDetails
            };
            
            _mediator.Send(updateReportCmd).GetAwaiter().GetResult();
        }
    }
}
