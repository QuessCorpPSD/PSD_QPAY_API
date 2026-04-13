namespace QPay.API.Models
{
    public class ClientModel
    {
        public string Client_Name { get; set; } = "";

        public string Company_Code { get; set; } = "";
    }
    public class PayCodeListUI
    {
        public int? value { get; set; }
        public string? text { get; set; }
    }
}
