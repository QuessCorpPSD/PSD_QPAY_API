namespace QPay.API.Models
{
    public class PayRegisterResponse
    {
        public int statusCode { get; set; }
        public string Qzone { get; set; }

        public string Reference { get; set; }
    }
}
