namespace QPay.API.Models
{
    public class FlexiRequestModel
    {
        public string mode { get; set; } = "";
        public string FlexiDetails { get; set; } = "";
        public int? Company_Id { get; set; }
        public int? Band_Id {  get; set; }

    }

    public class FlexiModelRequest
    {
        public int stateId { get; set; }
        //public int paycode_Id { get; set; }

        //public string paycode_code { get; set; } = "";

        public List<selectedPayCode> selectedpaycode { get; set; }
        public int FlexiId { get; set; }
        public int CreatedBy { get; set; }
        public string mode { get; set; } = "";
    }
    public class selectedPayCode
    {
        public string value { get; set; } = "";
        public string text { get; set; } = "";
    }

    public class FlexiAddRequestModel
    {
        public string mode { get; set; } = "";
        public string xml { get; set; } = "";
        public int? userId { get; set; }
        

    }
    public class SearchRequestModel
    {
        public int? Company_Id { get; set; }
        public int? Band_Id { get; set; }
        public int? param { get; set; }
        public int? Flexi_Rule_Id {  get; set; }

    }
}
