namespace QPay.UI.Models.Reports
{
    public class InvoiceLeaveBalanceReportRequest
    {
        public string companyId { get; set; } = "";
        public string siteId { get; set; } = "";
        public string fromMonth { get; set; } = "";
        public string fromYear { get; set; } = "";
        public string toMonth { get; set; } = "";
        public string toYear { get; set; } = "";
    }
}
