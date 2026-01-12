using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class AllotmentLotStatusRequest
    {
        public int Company_Id { get; set; }
        public int pay_period_id { get; set; }
        public int lotnumber { get; set; }
        public string UpdateStatus { get; set; } = string.Empty;
        public string Payroll_Input_Type { get; set; } = string.Empty;
        public string createdon { get; set; } = string.Empty;
        public int userId { get; set; }

        public string QC_RaiseQuery { get; set; } = string.Empty;



    }

    public class Allotmemet
    {
        public int InputLot_Id { get; set; }
        public string CategoryListName { get; set; } = string.Empty;
        public int Input { get; set; }
        public int Output { get; set; }
        public string Mismatch { get; set; } = string.Empty;
        public string? Remarks { get; set; } = string.Empty;


    }

    public class QCApprovedRequest
    {
        
        public int Company_Id { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string Pay_Period { get; set; } = string.Empty;
        public int pay_period_id { get; set; }
        public int lotnumber { get; set; }
        public string UpdateStatus { get; set; } = string.Empty;
        public string Payroll_Input_Type { get; set; } = string.Empty;
        public string createdon { get; set; } = string.Empty;
        public int userId { get; set; }       
        public int revised { get; set; }
        public string? RaiseQuery { get; set; } = string.Empty;

        public string? CheckinFile { get; set; } = string.Empty;
        public List<Allotmemet>? allotments { get; set; } = new List<Allotmemet>();
      
    }
}
