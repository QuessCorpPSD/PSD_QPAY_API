using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Invoice
{
    public class InvoiceInitiationUI
    {
        public int? Serial_No { get; set; }

        public int? Company_Id { get; set; }
        public int? TaxTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public string? Company_Code { get; set; } = string.Empty;
        public int? Employee_Head_Count { get; set; }
        public decimal? Service_Charge { get; set; }
        public int? Service_Tax { get; set; }
        public int? Service_Tax_Id { get; set; }
        public string? Pay_Period { get; set; } = string.Empty;
        public int? Pay_Period_Id { get; set; }
        public string? Service_Charge_Master { get; set; } = string.Empty;
        public string? Service_Charge_Type { get; set; } = string.Empty;
        public decimal? Net_CTC { get; set; }
        public string? Krushi_Kalyan_CESS { get; set; } = string.Empty;
        public string? Swatch_Bharat { get; set; } = string.Empty;
        public int? Map_Id { get; set; }
        public string? Map_Name { get; set; } = string.Empty;
        public string? Effective_Date { get; set; } = string.Empty;
        public string? Error_Message { get; set; } = string.Empty;
        public int? InvoiceType_Id { get; set; }
        public int? InvoiceCulture_id { get; set; }
        public string? InvoiceCul_Ref_No { get; set; } = string.Empty;
        public int? EBASIC { get; set; }
        public int? Input_No { get; set; }
        public int? GEN_iID { get; set; }
        public string? GEN_vDescription { get; set; } = string.Empty;
        public decimal? ServiceChargeAmount { get; set; }
        public decimal? Sourcing_Fee { get; set; }
        public decimal? Sourcing_Fee_Amount { get; set; }
        public int? Invoice_Category_Id { get; set; }
        public decimal? INCTC { get; set; }
        public decimal? INSCG { get; set; }
        public decimal? NetPay { get; set; }
        public string? PO_Number { get; set; } = string.Empty;
        public decimal? BGVBL { get; set; }
        public decimal? ASTFEE { get; set; }
        public decimal? DISCT1 { get; set; }
        public decimal? DISCT2 { get; set; }
        public decimal? IDCARD { get; set; }
        public decimal? EMAIL { get; set; }
        public decimal? REGFEE { get; set; }
        public decimal? TRNFEE { get; set; }
        public decimal? GGDBT { get; set; }
        public decimal? PPEKIT { get; set; }
        public decimal VMSFEE { get; set; }

        public decimal? EDUFEE { get; set; }
        public decimal? NTPRY { get; set; }
        public decimal? RENMAC { get; set; }
        public decimal? DRADED { get; set; }
        public decimal? OTHDD { get; set; }
        public decimal? MBAPP { get; set; }
        public decimal? CALCRG { get; set; }
        public decimal? CALRT { get; set; }

        public string? Narration { get; set; } = string.Empty;
    }

    public class InitiationReqDetail
    {
        public int? StatusCode {  get; set; }
        public string? Messages {  get; set; }
       public List<InitiationRequestUI>? DraftInvoiceInitation {  get; set; }
    }
    public class InitiationRequestUI
    {
        public int? Serial_No {  get; set; }
        public string? Req_No { get; set; } = "";
        public int? Company_Id { get; set; }
        public int? Pay_Period_Id { get; set; }
        public int? Employee_Id { get; set; }
        public int? Map_Name_Id { get; set; }
        public string? LotNo { get; set; } = "";
        public string? Input_No { get; set; } = "";
        public string? Map_name { get; set; } = "";
        public string? Company_Code { get; set; } = "";
        public string? Pay_Period { get; set; } = "";
        public int? Employee_Head_Count { get; set; }
        public decimal? Service_Charge { get; set; }
        public string? Service_Charge_Master { get; set; } = "";
        public string? Service_Charge_Type { get; set; } = "";
        public decimal? Sourcing_Fee { get; set; }
        public decimal? Sourcing_Fee_Amount { get; set; }
        public int? InvoiceType_Id { get; set; }
        public decimal? Net_CTC { get; set; }
        public int? InvoiceCulture_id { get; set; }
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
        public decimal? DISCT2 { get; set; }
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

        public decimal? EAPCT { get; set; }
        public decimal? HOSAC { get; set; }
        public string? Invoice_Type { get; set; } = "";
        public string? Invoice_Category { get; set; } = "";

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
        public string? Cost_Center_Name { get; set; } = "";

        public string? Client_SPOC_Name { get; set; } = "";
        public string? Work_Order_Number { get; set; } = "";
        public string? Data_From { get; set; } = "";
        public string? InvoiceType { get; set; } = "";
        public string? Invoice_remarks { get; set; } = "";
        public string? Narration { get; set; } = "";
        public int? Group_Detail_Id { get; set; }
        public string? Group_Name { get; set; } = "";
        public string? FI_Document_Number { get; set; } = "";
        public int? Expense_ID { get; set; }
        public string? Service_Description { get; set; } = "";
        public string? Invoice_Number { get; set; } = "";
        public string? PRO_Invoice_Number { get; set; } = "";

        public string? Id{ get; set; } = "";

    }
    public class InvoiceQCDetailUI {
        public string ReqNo { get; set; } = "";
        public string InvoiceNumber { get; set; } = "";
    }
    public class InitiationRequestModel
    {
        public int? Company_Id { get; set; }
        public int? PayPeriod_Id { get; set; }
        public string? PayPeriod { get; set; }
        public int? InvoiceType { get; set; }
        public string ActionType { get; set; } = "";
        public int? Invoice_Billing_Type { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class IntiationExportRequest
    {
        public int? Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public string? Pay_Period { get; set; }
        public int? PayPeriod_Id { get; set; }
        public string? LotNo { get; set; }
        public string? ReqNo { get; set; }
        public string? Data_From { get; set; }
        public string? Invoice_Type { get; set; }

        public string? InvoiceCultureType { get; set; }
    }

    public class InvoiceDetailModel
    {
        public int? InvoiceType { get; set; }
        public string ActionType { get; set; } = "";
        public int? userId { get; set; }
    }

    public class RequestModel
    {
        public string Req_No { get; set; } = "";
    }

    public class RemarksResponse
    {
        public string Req_No { get; set; } = "";
        public string Invoice_remarks { get; set; } = "";

        public string Remarks_GivenBy { get; set; } = "";
        public string InvoiceType { get; set; } = "";
        public string Time { get; set; } = "";
    }


    public class InvoiceDashboardDto
    {
        public int? Serial_No { get; set; }
        public string InvoiceType { get; set; } = "";
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
        public string PO_Number { get; set; } = "";
        public int? RequestedBy { get; set; }
        public string RequestedDate { get; set; } = "";
        public string Initiation_Remarks { get; set; } = "";
        public string AssignedTo { get; set; } = "";
        public string Rejected_On { get; set; } = "";
        public string Rejected_By { get; set; } = "";
        public string InvoiceCreatedOn { get; set; } = "";
        public string Invoice_remarks { get; set; } = "";
    }


    public class ProvisionalInvoiceInitiateRequest
    {
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string PayPeriodId { get; set; } = string.Empty;
        public string PayPeriod { get; set; } = string.Empty;
        public string LotNo { get; set; } = string.Empty;
        public string Input_No { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string ServiceCharge { get; set; } = string.Empty;
        public string State_Name { get; set; } = string.Empty;
        public string State_Id { get; set; } = string.Empty;
        public string Map_Name_Id { get; set; } = string.Empty;
        public string Map_Name { get; set; } = string.Empty;
        public string Invoice_Category_Id { get; set; } = string.Empty;
        public string Invoice_Category { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string Isactive { get; set; } = string.Empty;
        public string Employee_code { get; set; } = string.Empty;
        public string InvoiceCulture_id { get; set; } = string.Empty;
    }

    [XmlRoot("ProvisionalInvoiceInitiate")]
    public class ProvisionalInvoiceInitiateRequestBulk
    {
        [XmlElement("Details")]
        public List<ProvisionalInvoiceInitiateRequest> request { get; set; }
        [XmlIgnore]
        public int CreatedBy { get; set; }
    }

    public class VendorInvoiceInitiateRequest
    {
        public string Company_Id { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public string InvoiceType_Id { get; set; } = string.Empty;
        public string CTC { get; set; } = string.Empty;
        public string Base_Amount { get; set; } = string.Empty;
        public string Map_Name_Id { get; set; } = string.Empty;
        public string Map_Name { get; set; } = string.Empty;
        public string State_Id { get; set; } = string.Empty;
        public string State_Name { get; set; } = string.Empty;
        public string Input_Number { get; set; } = string.Empty;
        public string Group_Detail_Id { get; set; } = string.Empty;
        public string Group_Name { get; set; } = string.Empty;
        public string Pay_Period_Id { get; set; } = string.Empty;
        public string Pay_Period { get; set; } = string.Empty;
        public string ServiceFee { get; set; } = string.Empty;
        public string FI_Document_Number { get; set; } = string.Empty;
        public string Expense_Id { get; set; } = string.Empty;
    }

    [XmlRoot("InitiateDetails")]
    public class VendorInvoiceInitiateRequestBulk
    {
        [XmlElement("Initiate")]
        public List<VendorInvoiceInitiateRequest> request { get; set; }
        [XmlIgnore]
        public int CreatedBy { get; set; }
        //[XmlIgnore]
        //public int Company_Id { get; set; }
        //[XmlIgnore]
        //public string Pay_Period { get; set; } = string.Empty;
        //[XmlIgnore]
        //public string Action { get; set; } = string.Empty;
    }

    public class MiscInvoiceInitiateRequest
    {
            public string? Action { get; set; }
        public string? Created_Mode { get; set; }
            public string? UserId { get; set; }
            public string? Invoice_Id { get; set; }
            public string? Invoice_Number { get; set; }
            public string? Company_Id { get; set; }
            public string? Cost_Center_Mapping_Id { get; set; }
            public string? City_Id { get; set; }
            public string? Financial_Year_Id { get; set; }
            public string? Pay_Period_Id { get; set; }
            public string? Invoice_Type_Id { get; set; }
            public string? Invoice_Date { get; set; }
            public string? Invoice_Due_Date { get; set; }
            public string? Particulars { get; set; }
            public string? Amount { get; set; }
            public string? StateId { get; set; }
             public string? State_Name { get; set; }
        public string? InvoicingStateId { get; set; }
            public string? CGST_Percentage { get; set; }
            public string? SGST_Percentage { get; set; }
            public string? UTGST_Percentage { get; set; }
            public string? IGST_Percentage { get; set; }
            public string? Client_PO { get; set; }
            public string? Purchase_Order_Id { get; set; }
            public string? Input_Date { get; set; }
            public string? Output_Date { get; set; }
            public string? Service_Charge { get; set; }
            public string? Service_Charge_Amount { get; set; }
            public  decimal? Sourcing_Fee { get; set; }
            public decimal? Sourcing_Fee_Amount { get; set; }
            public string? No_Of_Employees { get; set; }
            public string? Absorption_Fee { get; set; }
            public string? Absorption_Amt { get; set; }
            public string? CTC_Amt_Adjusted { get; set; }
            public string? CTC_Amt_NorP { get; set; }
            public string? CTC_Adj_Note { get; set; }
            public string? Net_Amt_Adjusted { get; set; }
            public string? Net_Amt_NorP { get; set; }
            public string? Net_Adj_Note { get; set; }
            public string? Invoice_Culture_Id { get; set; }
            public string? Invoice_Culture_RefNo { get; set; }
            public string? Input_No { get; set; }
            public string? Employee_ESI { get; set; }
            public string? Employer_ESI { get; set; }
            public string? Employee_PF { get; set; }
            public string? Employer_PF { get; set; }
            public string? Mobile_Recovery_Amount { get; set; }
            public string? Personal_Loan_Amount { get; set; }
            public string? Other_Deduction_Amount { get; set; }
            public string? WO_Number { get; set; }
            public string? Pl_Id_No { get; set; }
            public string? Employee_Name { get; set; }
            public string? Markup { get; set; }
            public string? Gri_Msp { get; set; }
            public string? DO_Number { get; set; }
            public string? Remarks { get; set; }
            public string? Status { get; set; }
            public string? IsActive { get; set; }
            public string? WO_Date { get; set; }
            public string? InvoiceNotes { get; set; }
            public string? CreatedBy { get; set; }
            public string? CreatedOn { get; set; }
            public string? ModifiedBy { get; set; }
            public string? ModifiedOn { get; set; }
            public string? Discrepancy_By { get; set; }
            public string? Discrepancy_Reason { get; set; }
            public string? Onboarding_Charge { get; set; }
            public string? Group_Detail_Id { get; set; }
            public string? TaxableAmount1 { get; set; }
            public string? TaxableAmount1_Note { get; set; }
            public string? TaxableAmount2 { get; set; }
            public string? TaxableAmount2_Note { get; set; }
            public string? TaxableAmount3 { get; set; }
            public string? TaxableAmount3_Note { get; set; }
            public string? NonTaxableAmount1 { get; set; }
            public string? NonTaxableAmount1_Note { get; set; }
            public string? NonTaxableAmount2 { get; set; }
            public string? NonTaxableAmount2_Note { get; set; }
            public string? NonTaxableAmount3 { get; set; }
            public string? NonTaxableAmount3_Note { get; set; }
            public string? Billable_Type_Id { get; set; }
            public string? ProvisionalInvoiceNumber { get; set; }
            public string? Compliance_Fee { get; set; }
            public string? Compliance_Fee_Amount { get; set; }
            public string? Ctc_Deduction_Type_Id { get; set; }
            public string? Net_Deduction_Type_Id { get; set; }
            public string? Gratuityinterest { get; set; }
            public string? InsuranceAmount { get; set; }
            public string? NewInvoiceNumber { get; set; }
            public string? BGVBL { get; set; }
            public string? ASTFEE { get; set; }
            public string? DISCT1 { get; set; }
            public string? DISCT2 { get; set; }
            public string? IDCARD { get; set; }
            public string? EMAIL { get; set; }
            public string? REGFEE { get; set; }
            public string? TRNFEE { get; set; }
            public string? GGDBT { get; set; }
            public string? PPEKIT { get; set; }
            public string? VMSFEE { get; set; }
            public string? CALCRG { get; set; }
            public string? CALRT { get; set; }
            public string Req_No { get; set; } = "";
          public string Id { get; set; } = "";

    }

    [XmlRoot("Data")]
    public class MiscInvoiceInitiateRequestBulk
    {
        [XmlElement("GstInvoice")]
        public List<MiscInvoiceInitiateRequest> request { get; set; }
        [XmlIgnore]
        public int CreatedBy { get; set; }
      
    }
    
}
