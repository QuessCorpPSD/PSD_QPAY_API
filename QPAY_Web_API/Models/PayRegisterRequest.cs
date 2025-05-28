namespace QPay.API.Models
{
    public class PayRegisterRequest
    {
        public int companycode { get; set; }

        public int pay_period_Id { get; set; }
        public string pay_period { get; set; }

        public int lotNumber { get; set; }

        public string payroll_input_type { get; set; }
    }
}
