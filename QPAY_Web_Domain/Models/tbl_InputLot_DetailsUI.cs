using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
  public  class tbl_InputLot_DetailsUI
    {
        public int InputLot_Id { get; set; }
        public int Company_Id { get; set; }
        public int Pay_Period_Id { get; set; }
        public int Lot_Number { get; set; }
        public int Payroll_Input_Type { get; set; }
        public int Revised { get; set; }
        public string CreatedOn { get; set; }
        public string Input_Category { get; set; }
        public DateTime Input_Headcount { get; set; }
        public int Output_Headcount { get; set; }
        public DateTime Output_Datetime { get; set; }
        public string QC_Verified_Status { get; set; }
        public string QC_Verified_DateTime { get; set; }
        public string Report_Status { get; set; }
        public string Report_DateTime { get; set; }
        public string Customer_Confirmation_Status { get; set; }

        public string Customer_Confirmation_DateTime { get; set; }
        public string Invoice_Status { get; set; }
        public string Invoice_DateTime { get; set; }
        public string Remarks { get; set; }
        public int P1_User_Id { get; set; }
        public int P2_User_Id { get; set; }
        public int P3_User_Id { get; set; }
        public int P4_User_Id { get; set; }
      
    }
}
