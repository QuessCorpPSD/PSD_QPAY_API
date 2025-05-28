using System.Net;

namespace QPAY_Web_API.Models
{
    public class APIResponses
    {
       public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public object Data { get; set; } = new object();

        public ErrorDetails Error { get; set; } = new ErrorDetails();


        public APIResponses(HttpStatusCode statusCode, string message, object data, ErrorDetails error)
        {
            StatusCode=statusCode;
            Message=message;
            Data=data;
            Error=error;
        }
    }

    public class ErrorDetails
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class OAuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenEndpoint { get; set; }
        public string GrantType { get; set; }
    }

    public class ApiSettings
    {
        public string BaseUrl { get; set; }
        public string ProtectedEndpoint { get; set; }
    }
}
