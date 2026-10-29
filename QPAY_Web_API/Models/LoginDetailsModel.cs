namespace QPAY_Web_API.Models
{
    public class LoginDetailsModel
    {
        public int username { get; set; } 

        public string password { get; set; } = "";

        public string ipAddress { get; set; } = "";
        public string Cname { get; set; } = "";

    }
    public class MailValidationRequest
    {
        public string userId {  get; set; }
        public string otp {  get; set; }
    }
}
