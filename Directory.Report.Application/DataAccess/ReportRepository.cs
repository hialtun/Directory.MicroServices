using MicroServices.Infrastructure.Repository;
using MicroServices.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace Directory.Report.Application.DataAccess
{
    public class ReportRepository : GenericRepository<Domain.Entities.Report>
    {
        public ReportRepository(IOptions<DatabaseSettings> options) : base(options)
        {

        }
    }
}