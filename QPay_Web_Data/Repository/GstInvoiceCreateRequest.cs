using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.DAL.Repository
{
    public class GstInvoiceCreateRequest
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
        public string? Sourcing_Fee { get; set; }
        public string? Sourcing_Fee_Amount { get; set; }
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
    }

    public class GstInvoiceEditRequest
    {
        public string? Action { get; set; }
        public string? UserId { get; set; }
        public string? Invoice_Id { get; set; }
    }
    }
