using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QPay.UI.Dashboard
{
    public class AdminDashboardDetailUI
    {
        public int? Lot_Number {  get; set; }
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }= string.Empty;
        public string CompanyShortName { get; set; } = string.Empty;
        public string Payroll_Input_Type {  get; set; }=string.Empty;
        public string Company_Code {  get; set; }= string.Empty;
        public string Pay_period {  get; set; }= string.Empty;
        public int Pay_Period_Id { get; set; }

        public int? HeadCount { get; set; }
        
        public int? NetPay { get; set; }

        public DateTime CreatedOn {  get; set; }
        public DateTime? AllottedDateTime { get; set; }
        public DateTime? QC_Verified_DateTime { get; set; }
        public int? EstimateTime { get; set; }
        public string? AssignedTo { get; set; } = string.Empty;
        public string? ReportingManager { get; set; } = string.Empty;
        public string? InvoiceGenerated { get; set; } = string.Empty;
        public DateTime? InvoiceGeneratedDate { get; set; }

        public string? Customer_Confirmation_Status { get; set; } = string.Empty;

        public DateTime? Customer_Confirmation_DateTime { get; set; } 


    }

    public class AdminDashboardParameterlUI
    {
        public string? FilterType { get; set; } = string.Empty;
        public string? FinancialYear  { get; set; }
        public int? UserId { get; set; } = 0;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }

    public class DashboardDetailUI
    {
        
        public int Company_Id { get; set; }
        public string Company_Name { get; set; } = string.Empty;
        public string CompanyShortName { get; set; } = string.Empty;
        public int? Lot_Number { get; set; }

        public int? HeadCount { get; set; }

        public int? CTC { get; set; }

        public int? NetPay { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? AllottedDateTime { get; set; }

        public string? AssignedTo { get; set; } = string.Empty;

        public int? EstimateTime { get; set; }

        public DateTime? QC_Verified_DateTime { get; set; }

        public DateTime? ProcessDatetime { get; set; }

        public string Score { get; set; } = string.Empty;

        public string? ReportingManager { get; set; } = string.Empty;
        public string? InvoiceGenerated { get; set; } = string.Empty;
        public DateTime? InvoiceGeneratedDate { get; set; }

        public string? SalaryPayout { get; set; } = string.Empty;
        public string? Customer_Confirmation_Status { get; set; } = string.Empty;

        public DateTime? Customer_Confirmation_DateTime { get; set; }

        public string? Process_Category { get; set; } = string.Empty;

        public string Payroll_Input_Type { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string Pay_period { get; set; } = string.Empty;
        public int Pay_Period_Id { get; set; }

        

        

        
        
        
        
        
       

    }
}

