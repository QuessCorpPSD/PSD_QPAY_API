namespace QPay.API.Models
{
    public class IRNModelRequest
    {
        public List<int> invoiceIds { get; set; }
        public string Mode { get; set; }
        public string userId { get; set; }
    }
    public class IRNModelResponse
    {
        public string Error_Message { get; set; }
    }
    public class ClearTaxResponse
    {
        public string Status { get; set; } = "";
        public string Error_Message { get; set; } = "";
    }
    public class ClearTaxRequest
    {
        public string InvoiceId { get; set; }
        public int userId { get; set; }
        public string Mode { get; set; }


    }
}
