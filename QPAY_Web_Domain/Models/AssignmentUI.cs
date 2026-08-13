using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{

    public class AssignmentLots
    {

        public int StatusCode {  get; set; }
        public string Error_Message {  get; set; }
        public List<AssignmentUI> PendingLots { get; set; }
        public List<AssignmentUI> TodayLots { get; set; }
    }
   public class AssignmentUI
    {
        public int InputLot_Id { get; set; }
        public string Company_code { get; set; }
        public string Company_name { get; set; }
        public int Company_Id { get; set; }
        public string Pay_period { get; set; }

        public int pay_period_id { get; set; }
        public int Lot_Number { get; set; }
        public string Payroll_Input_Type { get; set; }
        public int Revisedtime { get; set; }
        public string Assignment { get; set; }
        public int? HeadCount { get; set; } = 0;
        
        public string Estimate_time { get; set; } 

        public string CreatedOn { get; set; }

        public string Process_Category { get; set; }

        public int? P1_HeadCount { get; set; } = 0;
        public int? P2_HeadCount { get; set; } = 0;
        public int? P3_HeadCount { get; set; } = 0;
        public int? P4_HeadCount { get; set; } = 0;
    }
}
