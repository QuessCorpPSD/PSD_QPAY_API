using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Dashboard
{
    public class PendingLotsUI
    {
        public string CompanyShortName { get; set; } = string.Empty;
        public string Company_Name { get; set; } = string.Empty;
        public string Pay_period { get; set; } = string.Empty;
        public int Lot_Number { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public DateTime? AllottedDateTime { get; set; } 
        public int? headCount { get; set; } 
        public string QCStatus { get; set; } = string.Empty;
        public int? Estimate_time { get; set; } 
        public int? TakenTime { get; set; } 
        public double? score { get; set; }
        public string Process_Category { get; set; }= string.Empty;
        public string Name { get; set;} = string.Empty;
        public string TeamLead { get; set; } = string.Empty;
        public string Head { get; set; } = string.Empty;


    }
}
