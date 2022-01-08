using System;
using System.Collections.Generic;
using MicroServices.Core.Entity;

namespace Directory.Report.Domain.Entities
{
    public class Report : DocumentEntity
    {
        public DateTime DemandDatetime { get; set; }
        public EReportStatus Status { get; set; }
        public List<ReportDetail> ReportDetail { get; set; }
    }

    public enum EReportStatus
    {
        InProgress = 1,
        Completed = 2
    }

    public class ReportDetail
    {
        public string Location { get; set; }
        public int ContactCount { get; set; }
        public int ContactPhoneCount { get; set; }
    }
}