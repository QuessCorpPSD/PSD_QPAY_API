using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Invoice
{
    public class GstInvoiceGrid
    {
            public int? Invoice_Id { get; set; }
            public string Invoice_Number { get; set; } = "";

        public string IRN_Status { get; set; } = "";
        public int? Company_Id { get; set; }
            public string Company_Code { get; set; } = "";
            public int? Cost_Center_Mapping_Id { get; set; }
            public string Map_Name { get; set; } = "";
            public int? City_Id { get; set; }
            public string City_Name { get; set; } = "";
            public int? Financial_Year_Id { get; set; }
            public int? Pay_Period_Id { get; set; }
            public string Pay_Period { get; set; } = "";
            public int? Invoice_Type_Id { get; set; }
            public string InvoiceType { get; set; } = "";
            public string? Invoice_Date { get; set; }
            public string Particulars { get; set; } = "";

            public decimal? Amount { get; set; }
            public decimal? IGST_Percentage { get; set; }
            public decimal? IGST_Amount { get; set; }
            public decimal? Service_Charge { get; set; }
            public decimal? Service_Charge_Amount { get; set; }
            public decimal? Sourcing_Fee { get; set; }
            public decimal? Sourcing_Fee_Amount { get; set; }
            public int? No_Of_Employees { get; set; }
            public decimal? Net_Amount { get; set; }
            public int? Input_No { get; set; }
            public decimal? Employee_PF { get; set; }
            public decimal? Employer_PF { get; set; }

            public string DO_Number { get; set; } = "";
            public string Status { get; set; } = "";
            public bool? IsActive { get; set; }
            public bool? IsLocked { get; set; }
            public int? Group_Detail_Id { get; set; }
            public string Group_Name { get; set; } = "";
        public string? Irn_Number { get; set; }
        public string? Crn_Number { get; set; }
            public string Crn_IRN_Status { get; set; } = "";
            public string Crn_IRN_Number { get; set; } = "";
            public DateTime? Crn_IRN_Cancel_Date { get; set; }
            public string Sap_Invoice_Number { get; set; } = "";
            public string Sap_Account_Number { get; set; } = "";
            public string Sap_Cancel_Document { get; set; } = "";
            public string Sap_Credit_Note_Document { get; set; } = "";

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
    }
    public class BulkInvoices
    {
        public List<int> invoiceIds { get; set; }
    }

    public class InvoiceTypeUI
    {
        public string Invoice_Type_Id { get; set; } = "";
        public string InvoiceType { get; set; } = "";
    }

    public class BillingTypeUI
    {
        public string Billable_Type_Id { get; set; } = "";
        public string Billable_Type { get; set; } = "";
    }

    public class CtcDeductionUI
    {
        public string Ctc_Deduction_Type_Id { get; set; } = "";
        public string Ctc_Deduction_Type { get; set; } = "";
    }

    public class NewDeductionUI
    {
        public string Net_Deduction_Type_Id { get; set; } = "";
        public string Net_Deduction_Type { get; set; } = "";
    }


    //public class GstInvoiceCreateRequest
    //{
    //    public string? Action { get; set; }
    //    public string? Created_Mode { get; set; }
    //    public string? UserId { get; set; }
    //    public string? Invoice_Id { get; set; }
    //    public string? Invoice_Number { get; set; }
    //    public string? Company_Id { get; set; }
    //    public string? Cost_Center_Mapping_Id { get; set; }
    //    public string? City_Id { get; set; }
    //    public string? Financial_Year_Id { get; set; }
    //    public string? Pay_Period_Id { get; set; }
    //    public string? Invoice_Type_Id { get; set; }
    //    public string? Invoice_Date { get; set; }
    //    public string? Invoice_Due_Date { get; set; }
    //    public string? Particulars { get; set; }
    //    public string? Amount { get; set; }
    //    public string? StateId { get; set; }
    //    public string? InvoicingStateId { get; set; }
    //    public string? CGST_Percentage { get; set; }
    //    public string? SGST_Percentage { get; set; }
    //    public string? UTGST_Percentage { get; set; }
    //    public string? IGST_Percentage { get; set; }
    //    public string? Client_PO { get; set; }
    //    public string? Purchase_Order_Id { get; set; }
    //    public string? Input_Date { get; set; }
    //    public string? Output_Date { get; set; }
    //    public string? Service_Charge { get; set; }
    //    public string? Service_Charge_Amount { get; set; }
    //    public string? Sourcing_Fee { get; set; }
    //    public string? Sourcing_Fee_Amount { get; set; }
    //    public string? No_Of_Employees { get; set; }
    //    public string? Absorption_Fee { get; set; }
    //    public string? Absorption_Amt { get; set; }
    //    public string? CTC_Amt_Adjusted { get; set; }
    //    public string? CTC_Amt_NorP { get; set; }
    //    public string? CTC_Adj_Note { get; set; }
    //    public string? Net_Amt_Adjusted { get; set; }
    //    public string? Net_Amt_NorP { get; set; }
    //    public string? Net_Adj_Note { get; set; }
    //    public string? Invoice_Culture_Id { get; set; }
    //    public string? Invoice_Culture_RefNo { get; set; }
    //    public string? Input_No { get; set; }
    //    public string? Employee_ESI { get; set; }
    //    public string? Employer_ESI { get; set; }
    //    public string? Employee_PF { get; set; }
    //    public string? Employer_PF { get; set; }
    //    public string? Mobile_Recovery_Amount { get; set; }
    //    public string? Personal_Loan_Amount { get; set; }
    //    public string? Other_Deduction_Amount { get; set; }
    //    public string? WO_Number { get; set; }
    //    public string? Pl_Id_No { get; set; }
    //    public string? Employee_Name { get; set; }
    //    public string? Markup { get; set; }
    //    public string? Gri_Msp { get; set; }
    //    public string? DO_Number { get; set; }
    //    public string? Remarks { get; set; }
    //    public string? Status { get; set; }
    //    public string? IsActive { get; set; }
    //    public string? WO_Date { get; set; }
    //    public string? InvoiceNotes { get; set; }
    //    public string? CreatedBy { get; set; }
    //    public string? CreatedOn { get; set; }
    //    public string? ModifiedBy { get; set; }
    //    public string? ModifiedOn { get; set; }
    //    public string? Discrepancy_By { get; set; }
    //    public string? Discrepancy_Reason { get; set; }
    //    public string? Onboarding_Charge { get; set; }
    //    public string? Group_Detail_Id { get; set; }
    //    public string? TaxableAmount1 { get; set; }
    //    public string? TaxableAmount1_Note { get; set; }
    //    public string? TaxableAmount2 { get; set; }
    //    public string? TaxableAmount2_Note { get; set; }
    //    public string? TaxableAmount3 { get; set; }
    //    public string? TaxableAmount3_Note { get; set; }
    //    public string? NonTaxableAmount1 { get; set; }
    //    public string? NonTaxableAmount1_Note { get; set; }
    //    public string? NonTaxableAmount2 { get; set; }
    //    public string? NonTaxableAmount2_Note { get; set; }
    //    public string? NonTaxableAmount3 { get; set; }
    //    public string? NonTaxableAmount3_Note { get; set; }
    //    public string? Billable_Type_Id { get; set; }
    //    public string? ProvisionalInvoiceNumber { get; set; }
    //    public string? Compliance_Fee { get; set; }
    //    public string? Compliance_Fee_Amount { get; set; }
    //    public string? Ctc_Deduction_Type_Id { get; set; }
    //    public string? Net_Deduction_Type_Id { get; set; }
    //    public string? Gratuityinterest { get; set; }
    //    public string? InsuranceAmount { get; set; }
    //    public string? NewInvoiceNumber { get; set; }
    //    public string? BGVBL { get; set; }
    //    public string? ASTFEE { get; set; }
    //    public string? DISCT1 { get; set; }
    //    public string? DISCT2 { get; set; }
    //    public string? IDCARD { get; set; }
    //    public string? EMAIL { get; set; }
    //    public string? REGFEE { get; set; }
    //    public string? TRNFEE { get; set; }
    //    public string? GGDBT { get; set; }
    //    public string? PPEKIT { get; set; }
    //    public string? VMSFEE { get; set; }
    //    public string? CALCRG { get; set; }
    //    public string? CALRT { get; set; }
    //}

    public class GSTInvoiceRequest
    {
        public string Action { get; set; } = "";
        public string Created_Mode { get; set; } = "";
        public string UserId { get; set; } = "";
        public int Invoice_Id { get; set; }
        public int Invoice_Number { get; set; }
        public int Company_Id { get; set; }
        public int Cost_Center_Mapping_Id { get; set; }
        public int? City_Id { get; set; }
        public string City_Name { get; set; } = "";
        public int Financial_Year_Id { get; set; }
        public int Pay_Period_Id { get; set; }
        public int Invoice_Type_Id { get; set; }
        public string Invoice_Date { get; set; }
        public string Invoice_Due_Date { get; set; }
        public string Particulars { get; set; } = "";
        public decimal Amount { get; set; }
        public int StateId { get; set; }
        public decimal CGST_Percentage { get; set; }
        public decimal SGST_Percentage { get; set; }
        public decimal UTGST_Percentage { get; set; }
        public decimal IGST_Percentage { get; set; }
        public Boolean Client_PO { get; set; }
        public int? Purchase_Order_Id { get; set; }
        public DateTime Input_Date { get; set; }
        public DateTime Output_Date { get; set; }
        public decimal Service_Charge { get; set; }
        public decimal Service_Charge_Amount { get; set; }
        public decimal Sourcing_Fee { get; set; }
        public decimal Sourcing_Fee_Amount { get; set; }
        public int No_Of_Employees { get; set; }
        public decimal Absorption_Fee { get; set; }
        public decimal Absorption_Amt { get; set; }
        public decimal CTC_Amt_Adjusted { get; set; }
        public Boolean CTC_Amt_NorP { get; set; }
        public string CTC_Adj_Note { get; set; } = "";
        public decimal Net_Amt_Adjusted { get; set; }
        public Boolean Net_Amt_NorP { get; set; }
        public string Net_Adj_Note { get; set; } = "";
        public int Invoice_Culture_Id { get; set; }
        public string Invoice_Culture_RefNo { get; set; } = "";
        public int? Input_No { get; set; }
        public decimal Employee_ESI { get; set; }
        public decimal Employer_ESI { get; set; }
        public decimal Employee_PF { get; set; }
        public decimal Employer_PF { get; set; }
        public decimal Mobile_Recovery_Amount { get; set; }
        public decimal Personal_Loan_Amount { get; set; }
        public decimal Other_Deduction_Amount { get; set; }
        public string WO_Number { get; set; } = "";
        public string Pl_Id_No { get; set; } = "";
        public string Employee_Name { get; set; } = "";
        public decimal Markup { get; set; }
        public decimal Gri_Msp { get; set; }
        public string DO_Number { get; set; } = "";
        public string Remarks { get; set; } = "";
        public string Status { get; set; } = "";
        public Boolean IsActive { get; set; }
        public DateTime? WO_Date { get; set; }
        public string InvoiceNotes { get; set; } = "";
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Boolean? IsLocked { get; set; }
        public string Discrepancy_By { get; set; } = "";
        public decimal Onboarding_Charge { get; set; }
        public int? Group_Detail_Id { get; set; }
        public decimal TaxableAmount1 { get; set; }
        public string TaxableAmount1_Note { get; set; } = "";
        public decimal NonTaxableAmount1 { get; set; }
        public decimal NonTaxableAmount2 { get; set; }
        public string NonTaxableAmount2_Note { get; set; } = "";
        public decimal NonTaxableAmount3 { get; set; }
        public string NonTaxableAmount3_Note { get; set; } = "";
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal UTGST_Amount { get; set; }
        public decimal IGST_Amount { get; set; }
        public decimal Net_Amount { get; set; }
        public decimal BGVBL { get; set; }
        public decimal ASTFEE { get; set; }
        public decimal DISCT1 { get; set; }
        public decimal DISCT2 { get; set; }
        public decimal IDCARD { get; set; }
        public decimal EMAIL { get; set; }
        public decimal REGFEE { get; set; }
        public decimal TRNFEE { get; set; }
        public decimal GGDBT { get; set; }
        public decimal PPEKIT { get; set; }
        public decimal VMSFEE { get; set; }
        public decimal EDUFEE { get; set; }
        public decimal NTPRY { get; set; }
        public decimal DRADED { get; set; }
        public decimal RENMAC { get; set; }
        public decimal OTHDD { get; set; }
        public decimal CALCRG { get; set; }
        public decimal CALRT { get; set; }
    }

}
