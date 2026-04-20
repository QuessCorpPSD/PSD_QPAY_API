namespace QPay.UI.Models.AccountReceivableMod
{
    public class InvoiceCollectionReport
    {
        public int DateTypeId { get; set; }  
        public int CompanyId { get; set; }
        public int PayPeriodId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}