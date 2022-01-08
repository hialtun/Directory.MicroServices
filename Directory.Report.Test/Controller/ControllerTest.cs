using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Directory.Report.API.Controllers;
using Directory.Report.Application.Handlers.Command;
using Directory.Report.Application.Handlers.Query;
using Directory.Report.Application.RestClients.Model;
using Directory.Report.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Directory.Report.Test.Controller
{
    public class ControllerTest
    {
        Mock<IMediator> mockMediator = new Mock<IMediator>();
        ReportController mockController;
        List<Domain.Entities.Report> reports = new List<Domain.Entities.Report>();
        public ControllerTest()
        {
            mockController = new ReportController(mockMediator.Object);
            reports.Add(new Domain.Entities.Report()
            {
                Id = "61d7e470e00320030f6535fb",
                Status = EReportStatus.InProgress,
                DemandDatetime = DateTime.Now
            });
            reports[0].ReportDetail.Add(new ReportDetail()
            {
                Location = "İstanbul",
                ContactCount = 1,
                ContactPhoneCount = 1
            });
            reports.Add(new Domain.Entities.Report()
            {
                Id = "61d7e470e00320030f653w2b",
                Status = EReportStatus.InProgress,
                DemandDatetime = DateTime.Now
            });
            reports[1].ReportDetail.Add(new ReportDetail()
            {
                Location = "Ankara",
                ContactCount = 3,
                ContactPhoneCount = 2
            });
        }
        
        [Fact]
        public void Get()
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<GetReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetReportQuery cmd, CancellationToken token) =>
                {
                    var reportModel = reports.FirstOrDefault(c => c.Id == cmd.Id);
                    var report = new GetReportResponse()
                    {
                        Success = true,
                        Model = reportModel
                    };
                    return report;
                });

            var res = mockController.Get("61d7e470e00320030f6535fb")
                .GetAwaiter().GetResult() as OkObjectResult;
            
            Assert.Equal((int)HttpStatusCode.OK, res?.StatusCode);
            Assert.NotNull(res?.Value);
            var reportRes = res?.Value as GetReportResponse;
            
            Assert.NotNull(reportRes?.Model);
            Assert.Equal("61d7e470e00320030f6535fb",reportRes.Model.Id);
            Assert.Equal(EReportStatus.InProgress,reportRes.Model.Status);
        }
        
        [Fact]
        public void List()
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<ListReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ListReportQuery cmd, CancellationToken token) =>
                {
                    var resList = new ListReportResponse()
                    {
                        Success = true,
                        Model = reports
                    };
                    return resList;
                });

            var res = mockController.List()
                .GetAwaiter().GetResult() as OkObjectResult;
            
            Assert.Equal((int)HttpStatusCode.OK, res?.StatusCode);
            Assert.NotNull(res?.Value);
            var reportRes = res?.Value as ListReportResponse;

            if (reportRes?.Model == null) return;
            Assert.NotEmpty(reportRes?.Model);
            Assert.Equal(2, reportRes.Model.Count);
        }
    }
}