using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Directory.Report.Domain.Entities;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Report.Application.Handlers.Command
{
    public class UpdateReportCommand : IRequest<UpdateReportResponse>
    {
        public string Id { get; set; }
        public DateTime DemandDatetime { get; set; }
        public EReportStatus Status { get; set; }
        public List<ReportDetail> ReportDetail { get; set; }
    }

    public class UpdateReportResponse : ResponseBase<Domain.Entities.Report>
    {

    }

    public class UpdateReportHandler : IRequestHandler<UpdateReportCommand, UpdateReportResponse>
    {
        public UpdateReportHandler(IRepository<Domain.Entities.Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }
        private readonly IRepository<Domain.Entities.Report> _reportRepository;

        public async Task<UpdateReportResponse> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateReportResponse() { Success = true };
            try
            {
                var report = new Domain.Entities.Report()
                {
                    Id = request.Id,
                    Status = request.Status,
                    DemandDatetime = request.DemandDatetime,
                    ReportDetail = request.ReportDetail
                };
                await _reportRepository.UpdateAsync(request.Id, report);
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