using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class AllotmentUI
    {

        public int InputLot_Id { get; set; } = 0;
        public string Input_Category { get; set; }
        public int? Input_Headcount { get; set; } = 0;
        public string Input_Datetime { get; set; }
        public int? Output_Headcount { get; set; } = 0;
        public string QC_Verified_Status { get; set; }
        public string QC_Verified_DateTime { get; set; }
        public string Report_Status { get; set; }
        public string Customer_Confirmation_Status { get; set; }
        public string Customer_Confirmation_DateTime { get; set; }
        public string Invoice_DateTime { get; set; }
        public string Remarks { get; set; }        
        public string Invoice_Status { get; set; }
 public string FileNames { get; set; }
        public bool Ismatching => Input_Headcount == Output_Headcount;
      

    }
}
