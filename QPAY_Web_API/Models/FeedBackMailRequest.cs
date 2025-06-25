namespace QPay.API.Models
{
    public class FeedBackMailRequest
    {
        public string email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public string mobileNumber { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
    }

    public class FeedBackMailResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public string errorMessage { get; set; } = string.Empty;
    }
}
