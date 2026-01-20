namespace QPay.UI.Models.Reports
{
    public class BillingReportRequest
    {
        public string companyCode { get; set; } = "";
        public string siteId { get; set; } = "";
        public string payPeriodId { get; set; } = "";
    }
}
