using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroServices.Core.Event;
using MicroServices.Infrastructure.MessageBroker;

namespace Directory.Report.Application.Handlers.Event
{
    public class ReportDemandEvent : EventMessage, IRequest<ReportDemandResponse>
    {
        public ReportDemandEvent()
        {
            DateOfDemand = DateTime.Now;
        }
        public DateTime DateOfDemand { get; set; }
    }

    public class ReportDemandResponse
    {
        public bool Success { get; set; }
    }

    public class ReportDemandHandler : EventPublisher<ReportDemandEvent>,  IRequestHandler<ReportDemandEvent, ReportDemandResponse>
    {
        public ReportDemandHandler(RabbitMQClient client) : base(client)
        {
            Queue = "Report";
        }

        public async Task<ReportDemandResponse> Handle(ReportDemandEvent request, CancellationToken cancellationToken)
        {
            ReportDemandResponse response = new ReportDemandResponse() { Success = true };
            try
            {
                Publish(request);
            }
            catch (Exception)
            {
                response.Success = false;
            }
            return response;
        }
    }
}