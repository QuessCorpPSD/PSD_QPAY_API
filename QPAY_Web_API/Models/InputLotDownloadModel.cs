namespace QPay.API.Models
{
    public class InputLotDownloadModel
    {
        public int companycode { get; set; }
        
        public int pay_period_id { get; set; }
        public int lotNumber { get; set; }
        public string InputType { get; set; }
    }
}
