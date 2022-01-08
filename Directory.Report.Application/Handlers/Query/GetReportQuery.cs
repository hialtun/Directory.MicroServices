using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Report.Application.Handlers.Query
{
    public class GetReportQuery : IRequest<GetReportResponse>
    {
        public string Id { get; set; }
    }

    public class GetReportResponse : ResponseBase<Domain.Entities.Report>
    {

    }

    public class GetReportHandler : IRequestHandler<GetReportQuery, GetReportResponse>
    {
        public GetReportHandler(IRepository<Domain.Entities.Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }
        private readonly IRepository<Domain.Entities.Report> _reportRepository;

        public async Task<GetReportResponse> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            var response = new GetReportResponse() { Success = true };
            try
            {
                var report = await _reportRepository.GetByIdAsync(request.Id);
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