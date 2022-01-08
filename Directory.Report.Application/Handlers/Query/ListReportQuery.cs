using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroServices.Core.Handler;
using MicroServices.Infrastructure.Repository;

namespace Directory.Report.Application.Handlers.Query
{
    public class ListReportQuery : IRequest<ListReportResponse>
    {

    }

    public class ListReportResponse : ResponseBase<List<Domain.Entities.Report>>
    {

    }

    public class ListReportHandler : IRequestHandler<ListReportQuery, ListReportResponse>
    {
        public ListReportHandler(IRepository<Domain.Entities.Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }
        private readonly IRepository<Domain.Entities.Report> _reportRepository;

        public async Task<ListReportResponse> Handle(ListReportQuery request, CancellationToken cancellationToken)
        {
            var response = new ListReportResponse() { Success = true };
            try
            {
                var reportQuery =  _reportRepository.Get();
                response.Model = reportQuery.ToList();
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