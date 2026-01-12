using QPay.UI.Common;
using QPay.UI.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Dashboard
{
    public class DashBoardCompledtedUI
    {
        public string Company_Code { get; set; } = "";
        public string Entity_Name { get; set; } = "";
        public string Location { get; set; } = "";
        public string BusinessUnitLocation => string.Format("{0} - {1}", Entity_Name, Location);
        public string Payroll_Input_Type { get; set; } = "";
        public int? Lot_Number {  get; set; }
        public int? Input_Headcount { get; set; }
        public string Process_Category { get; set; } = "";
        public DateTime? InputSubmittedDate {  get; set; }
        public DateTime? AllottedDateTime { get; set; }
        public DateTime? QC_Verified_DateTime { get; set; }
        public int? EstimateTime {  get; set; }

    }

    public class DashboardRequestModel
    {
        public string FilterType { get; set; } = string.Empty;
        public int? UserId { get; set; } 
        public string? financialyear { get; set; } = string.Empty;
        public DateTime? FromDate {  get; set; }
        public DateTime? ToDate { get; set; }
    }
}
