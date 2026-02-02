namespace QPay.UI.Models.Reports
{
    public class TimesheetSummaryReportRequest
    {
        public string companyId { get; set; } = "";
        public string siteId { get; set; } = "";
        public string location { get; set; } = "";
        public string payPeriodId { get; set; } = "";
        public string status { get; set; } = "";
    }
}
