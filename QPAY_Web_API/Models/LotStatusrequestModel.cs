namespace QPay.API.Models
{
    public class LotStatusrequestModel
    {
        public int Company_Id { get; set; }
        public int pay_period_id { get; set; }
        public int lotnumber { get; set; }
        public string InputLotUpdateType { get; set; }

        public string Payroll_Input_Type { get; set; }

        public string createdon { get; set; }
        public int userId { get; set; }
    }
}
