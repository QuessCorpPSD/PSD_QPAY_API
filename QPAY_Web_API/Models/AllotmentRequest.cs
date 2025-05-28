namespace QPay.API.Models
{
    public class AllotmentRequest
    {
        public string companyCode { get; set; }
        public string payPeriod { get; set; }
        public int lot { get; set; }
    }
}
