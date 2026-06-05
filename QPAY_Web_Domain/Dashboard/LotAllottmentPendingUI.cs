using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Dashboard
{
    public class LotAllottmentPendingUI
    {
        public string Company_Code { get; set; } = "";
        public string Entity_Name { get; set; } = "";

        public string Location { get; set; } = "";
        public string Company_Name { get; set; } = "";
        public string Pay_period { get; set; } = "";
        public int Lot_Number { get; set; } 
        public string Process_Category { get; set; } = "";
        public string Payroll_Input_Type { get; set; } = "";
        public int? Input_Headcount { get; set; } 
        public DateTime? InputSubmittedDate { get; set; } 
        public DateTime? IntegratedDatetime { get; set; }
        public DateTime? ProcessDatetime { get; set; } 
        public DateTime? ReconDatetime { get; set; } 
        public DateTime? AllottedDateTime { get; set; } 
        public DateTime? QC_Verified_DateTime { get; set; }
        public int? User_Id { get; set; } 
        public string ReportingManager { get; set; } = "";
        public string AsssignedTo { get; set; } = "";
        public int? TimeTaken { get; set; }
        public string Score { get; set; } = "";
        public string Payroll_Type { get; set; } = "";



    }
}
