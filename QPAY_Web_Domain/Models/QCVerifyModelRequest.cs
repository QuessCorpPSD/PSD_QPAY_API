using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class QCVerifyModelRequest
    {
        public int InputLot_Id { get; set; } = 0;
        public int Company_Id { get; set; }
        public string CompanyCode { get; set; }
        public string Pay_Period { get; set; }
        public int pay_period_id { get; set; }
        public int lotnumber { get; set; }
        public string UpdateStatus { get; set; }
        public string Payroll_Input_Type { get; set; }
        public string createdon { get; set; }        
        public string Remarks { get; set; }
        public bool RequestForModification { get; set; }
    }
    public class QCVerifyModelResponse
    {
        public string StatusCode { get; set; }

        public string Messages { get; set; }
    }
}
