using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Invoice
{
    public class InitiationRequestUI
    {
        public int? Serial_No {  get; set; }
        public string? Req_No { get; set; } = "";
        public int? Company_Id { get; set; }
        public int? Pay_Period_Id { get; set; }
        public  int? Employee_Id { get; set; }
        public int? Map_Name_Id { get; set; }
        public string? LotNo { get; set; } = "";
        public string? Input_No { get; set; } = "";
        public string? Map_name { get; set; } = "";
        public string? Company_Code { get; set; } = "";
        public string? Pay_Period { get; set; } = "";
        public int? Employee_Head_Count { get; set; }
        public decimal? Service_Charge { get; set; }
        public string? Service_Charge_Master { get; set; }= "";
        public string? Service_Charge_Type { get; set; } = "";
        public int? InvoiceType_Id { get; set; }
        public decimal? Net_CTC { get; set; }
        public int InvoiceCulture_id { get; set; }
        public string? InvoiceCul_Ref_No { get; set; } = "";
        public int? Invoice_Category_Id { get; set; }
        public string? PO_Number { get; set; } = "";
        public decimal? ServiceChargeAmount { get; set; }
        public decimal? INCTC { get; set; }
        public decimal? INSCG { get; set; }
        public decimal? NetPay { get; set; }
        public decimal? BGVBL { get; set; }
        public decimal? ASTFEE { get; set; }
        public decimal? DISCT1 { get; set; }
        public decimal?  DISCT2 { get; set; }
        public decimal? IDCARD { get; set; }
        public decimal? EMAIL { get; set; } 
        public decimal? REGFEE { get; set; }
        public decimal? TRNFEE { get; set; }
        public decimal? GGDBT { get; set; }
        public decimal? PPEKIT { get; set; }
        public decimal? VMSFEE { get; set; }
        public decimal? CALCRG { get; set; }
        public decimal? CALRT { get; set; }
        public decimal? EDUFEE { get; set; }
        public decimal? NTPRY { get; set; }
        public decimal? DRADED { get; set; }
        public decimal? RENMAC { get; set; }
        public decimal? OTHDD { get; set; }
        public decimal? STCTC { get; set; }
        public decimal? BFIN35 { get; set; }
        public decimal? MBAPP { get; set; }
        public string Invoice_Type { get; set; } = "";
        public string Invoice_Category { get; set; } = "";

        public string? State_name { get; set; } = "";
        public bool? IsInitiation { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? Created_On { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Modify_On { get; set; }
        public int? Modify_By { get; set; }
        public DateTime? Cancelled_On { get; set; }
        public int? Cancelled_By { get; set; }
        public DateTime? Approved_On { get; set; }
        public int? Approved_By { get; set; }
        public DateTime? Rejected_On { get; set; }
        public int? Rejected_By { get; set; }
        public string? Initiation_Remarks { get; set; } = "";
        public string? GL_Code { get; set; } = "";
        public  string? Cost_Center_Name { get; set; } = "";

        public string? Client_SPOC_Name { get; set; } = "";
        public string? Work_Order_Number { get; set; } = "";

    }

    public class InitiationRequestModel
    {
        public int? Company_Id { get; set; }
        public int? PayPeriod_Id { get; set; }
        public int? InvoiceType { get; set; }
        public string ActionType { get; set; } = "";
    }

    public class InvoiceDetailModel
    {
        public int? InvoiceType { get; set; }
        public string ActionType { get; set; } = "";
        public int? userId { get; set; }
    }

    public class InvoiceDashboardDto
    {
        public int? Serial_No { get; set; }
        public string Req_No { get; set; } = "";
        public string Company_Code { get; set; } = "";
        public string Pay_Period { get; set; } = "";
        public string Map_name { get; set; } = "";
        public int? Employee_Head_Count { get; set; }
        public string Net_CTC { get; set; } = "";
        public string NetPay { get; set; } = "";
        public string Invoice_Category { get; set; } = "";
        public string Invoice_Type { get; set; } = "";
        public string State_name { get; set; } = "";
        public int? RequestedBy { get; set; }
        public string RequestedDate { get; set; } = "";
        public string Initiation_Remarks { get; set; } = "";
        public string AssignedTo { get; set; } = "";
        public string Rejected_On { get; set; } = "";
        public string Rejected_By { get; set; } = "";
        public string InvoiceCreatedOn { get; set; } = "";
    }

}
