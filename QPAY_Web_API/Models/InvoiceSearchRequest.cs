namespace QPay.API.Models
{
    public class InvoiceSearchRequest
    {
        public int companyId { get; set; }
        public string Pay_Period { get; set; } = string.Empty;
        public int Pay_Period_Id { get; set; }
        public int? taxtypeId { get; set; }
    }
}
