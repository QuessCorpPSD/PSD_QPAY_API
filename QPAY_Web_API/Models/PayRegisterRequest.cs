namespace QPay.API.Models
{
    public class PayRegisterRequest
    {
        public int companycode { get; set; }

        public int pay_period_Id { get; set; }
        public string pay_period { get; set; }

        public int lotNumber { get; set; }

        public int revised { get; set; }

        public string payroll_input_type { get; set; }

        public string process_category { get; set; }
    }

    public class PayRegisterQzoneRequest
    {
        public int companyId { get; set; }
        public string companycode { get; set; } = string.Empty;

        public int pay_period_Id { get; set; }
        public string pay_period { get; set; } = string.Empty; 

        public int lotNumber { get; set; }

        public string payroll_input_type { get; set; } =string.Empty;
    }

   
}
