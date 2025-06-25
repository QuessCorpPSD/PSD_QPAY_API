namespace QPay.API.Models
{
    public class PayRegisterResponse
    {
        public int statusCode { get; set; }
        public string Qzone { get; set; }

        public string Reference { get; set; }
    }
    public class PayRegisterQzoneResponse
    {
        public int companyId { get; set; }

        public int pay_period_Id { get; set; }
        public int lotNumber { get; set; }

        public string FileName { get; set; } = string.Empty;
    }
}
