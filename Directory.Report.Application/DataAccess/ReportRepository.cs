using System;
using MicroServices.Infrastructure.Repository;
using MicroServices.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace Directory.Report.Application.DataAccess
{
#pragma warning disable CA1041
    [Obsolete]
#pragma warning restore CA1041
    public class ReportRepository : GenericRepository<Domain.Entities.Report>
    {
        public ReportRepository(IOptions<DatabaseSettings> options) : base(options)
        {

        }
    }
}