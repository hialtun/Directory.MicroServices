using System;
using System.Collections.Generic;
using System.Threading;
using Directory.Report.Application.Handlers.Command;
using Directory.Report.Application.Handlers.Query;
using Directory.Report.Domain.Entities;
using MicroServices.Infrastructure.Repository;
using MicroServices.Test.Mock;
using Moq;
using Xunit;

namespace Directory.Report.Test.Handler
{
    public class HandlersTest : RepositoryMock
    {
        Mock<IRepository<Domain.Entities.Report>> mockRepository = new Mock<IRepository<Domain.Entities.Report>>();
        List<Domain.Entities.Report> reports = new List<Domain.Entities.Report>();
        
        public HandlersTest()
        {
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
            reports[0].ReportDetail.Add(new ReportDetail()
            {
                Location = "Ankara",
                ContactCount = 3,
                ContactPhoneCount = 2
            });
            SetupMock(mockRepository, reports);
        }

        [Fact]
        public void GetReport()
        {
            var getHandler = new GetReportHandler(mockRepository.Object);
            var mockReport = new Mock<GetReportQuery>
            {
                Object =
                {
                    Id = "61d7e470e00320030f6535fb"
                }
            };
            
            var report = getHandler.Handle(mockReport.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.NotNull(report.Model);
            Assert.Equal(EReportStatus.InProgress, report.Model.Status);
            Assert.Equal("61d7e470e00320030f6535fb", report.Model.Id);
            Assert.True(report.Model.DemandDatetime < DateTime.Now);
            Assert.Contains(report.Model.ReportDetail, r => r.Location == "İstanbul");
            Assert.Contains(report.Model.ReportDetail, r => r.ContactCount > 0);
            Assert.Contains(report.Model.ReportDetail, r => r.ContactPhoneCount > 0);
        }
        
        [Fact]
        public void ListReport()
        {
            var listHandler = new ListReportHandler(mockRepository.Object);
            var mockReport = new Mock<ListReportQuery>();
            
            var reportList = listHandler.Handle(mockReport.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsType<List<Domain.Entities.Report>>(reportList.Model);
            Assert.Equal(reports.Count, reportList.Model.Count);
        }
        
        [Fact]
        public void CreateReport()
        {
            var createHandler = new CreateReportHandler(mockRepository.Object);
            var mockReport = new Mock<CreateReportCommand>
            {
                Object =
                {
                    DemandDatetime = DateTime.Now,
                    Status = EReportStatus.InProgress
                }
            };
            
            var report = createHandler.Handle(mockReport.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.Equal(mockReport.Object.Status, report.Model.Status);
            Assert.True(report.Model.DemandDatetime < DateTime.Now);
            Assert.NotEmpty(report.Model.Id);
        }
        
        [Fact]
        public void UpdateReport()
        {
            var updateHandler = new UpdateReportHandler(mockRepository.Object);
            var mockReport = new Mock<UpdateReportCommand>
            {
                Object =
                {
                    Id = "61d7e470e00320030f6535fb",
                    DemandDatetime = DateTime.Now ,
                    Status = EReportStatus.Completed
                }
            };
            
            var report = updateHandler.Handle(mockReport.Object,new CancellationToken()).GetAwaiter().GetResult();
            Assert.Equal(mockReport.Object.Status, report.Model.Status);
            Assert.Equal(mockReport.Object.DemandDatetime, report.Model.DemandDatetime);
            Assert.NotEmpty(report.Model.Id);
        }
    }
}