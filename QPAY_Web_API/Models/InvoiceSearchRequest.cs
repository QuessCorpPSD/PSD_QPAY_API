namespace QPay.API.Models
{
    public class InvoiceSearchRequest
    {
        public int companyId { get; set; }
        public string Pay_Period { get; set; } = string.Empty;
        public int Pay_Period_Id { get; set; }
        public int? taxtypeId { get; set; }
    }
    public class InvoiceQCModelRequest
    {
        public int Req_No { get; set; }
        public string Invoice_Number { get; set; } = "";
    }

    public class BulkInvoiceQCModelRequest
    {
        public List<InvoiceQCModelRequest> invoiceQCModels { get; set; }
        public int CreatedBy { get; set; }
    }
}
