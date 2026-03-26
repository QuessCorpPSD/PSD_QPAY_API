namespace QPay.API.Models
{
    public class SezModelRequest
    {
        public int Company_Id { set; get; }
        public int PayPeriod_Id { set; get; }
        public int Year { set; get; }
        public string? InvoiceNumbers { set; get; } = "";
    }
}
