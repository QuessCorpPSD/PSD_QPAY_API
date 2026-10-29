using QPay.UI.Models;

namespace QPay.API.Models
{
    public class UserOTPValidatedRequest
    {
        public MailTokenContent mailTokenContent { get; set; }
        public int userId { get; set; }
        public int otp { get; set; }

    }
    public class UserOTPValidatedResponse
    {
        public string message { get; set; }
    }
}
