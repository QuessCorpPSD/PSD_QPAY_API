using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class BankAdvice
    {

        public string invoice_no { get; set; } = "";
        public int? SNo { get; set; }
        public string Company_Code { get; set; } = "";
        public string Total_emp { get; set; } = "";
        public string Batch_Count { get; set; } = "";
        public string Pay_Period { get; set; } = "";
        public int? Pay_Period_Id { get; set; }
        public int? Company_Id { get; set; }
        public string STATUS { get; set; } = "";
        public decimal Net_Pay { get; set; }
        public string Isapproved { get; set; } = "";
        public int Bank_Advice_Approvals_Id { get; set; }
        public int Cost_Center_Mapping_Id { get; set; }
        public string Map_Name { get; set; } = "";
        public string InvoiceType { get; set; } = "";
    }

    public class InvoiceCommon
    {
        public int Company_Id { get; set; }

        public int Pay_Period_Id { get; set; }

        public string Action { get; set; } = "";

        public string QZoneUserName { get; set; } = "";
    }

    public class BankAdviceApprovalRequest
    {
        public List<BankAdviceApproval> requestdata { get; set; }

        public int Company_id { get; set; }
        public int Pay_Period_id { get; set; }
        public int CreatedBy { get; set; }
        public string Mode { get; set; }
        public int Bank_Advice_Approvals_Id { get; set; }
        public string QZoneUserName { get; set; }

    }

    public class BankAdviceApproval
    {
        public string Invoice_No { get; set; } = "";
        public string Net_Pay { get; set; } = "";
    }


    public class ErrorMessage
    {
        public string Error_Message { get; set; } = "";

    }

    public class CommonDropDownBA
    {
        public string value { get; set; }
        public string name { get; set; }
    }

    public class BankAdviceRequest
    {
        public int QZoneUserName { get; set; }
        public List<Invoice_No> InvoiceList { get; set; }

    }

    public class Invoice_No
    {
        public string InvoiceNumber { get; set; }

    }


}
