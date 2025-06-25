namespace QPay.API.Models
{
    public class AutoMailRequest
    {
        public string email { get; set; } = string.Empty;
        public string mobileNumber { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
        public string subject { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
    }
}
