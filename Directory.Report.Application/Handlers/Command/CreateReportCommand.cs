using System;
using System.Threading;
using System.Threading.Tasks;
using Directory.Report.Domain.Entities;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Report.Application.Handlers.Command
{
    public class CreateReportCommand : IRequest<CreateReportResponse>
    {
        public DateTime DemandDatetime { get; set; }
        public EReportStatus Status { get; set; }
    }

    public class CreateReportResponse : ResponseBase<Domain.Entities.Report>
    {

    }

    public class CreateReportHandler : IRequestHandler<CreateReportCommand, CreateReportResponse>
    {
        public CreateReportHandler(IRepository<Domain.Entities.Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }
        private readonly IRepository<Domain.Entities.Report> _reportRepository;

        public async Task<CreateReportResponse> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateReportResponse() { Success = true };
            try
            {
                var report = new Domain.Entities.Report()
                {
                    Status = request.Status,
                    DemandDatetime = request.DemandDatetime
                };
                report = await _reportRepository.CreateAsync(report);
                response.Model = report;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Success = false;
            }
            return response;
        }
    }
}