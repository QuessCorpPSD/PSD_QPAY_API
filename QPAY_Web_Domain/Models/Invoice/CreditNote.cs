using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Invoice
{
    public class CreditNote
    {
        public int? CreditNote_Id { get; set; }
        public int? SNo { get; set; }
        public string? CreditNote_No { get; set; }
        public int? Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public string? Company_Name { get; set; }
        public int? Invoice_Id { get; set; }
        public int? Pay_Period_Id { get; set; }
        public string? Pay_Period { get; set; }
        public int? Employee_Id { get; set; }
        public string? First_Name { get; set; }
        public string? Employee_Code { get; set; }
        public string? Invoice_Number { get; set; }
        public string? Ref_Id { get; set; }
        public decimal? Collection_Amount { get; set; }
        public decimal? Credit_Note_Amount { get; set; }
        public int? Credit_Note_Type { get; set; }
        public string? Credit_Note_Type_Text { get; set; }
        public string? Credit_Note_Status { get; set; }
        public int? Is_TdsApplicable { get; set; }
        public decimal? CreditNote_TdsAmount { get; set; }
        public decimal? CreditNote_Value { get; set; }
        public string? Remarks { get; set; }
        public string? Is_GST_Applicable { get; set; }
        public string? SAPRefNumber { get; set; }
        public string? Billing_Address { get; set; }
        public string? Shipping_Address { get; set; }
        public string? GSTN_Number { get; set; }
        public DateTime? Credit_Note_Dates { get; set; }
        public string? Credit_Note_Date { get; set; }
        public decimal? Actual_Amount { get; set; }
        public decimal? Adjusted_amount { get; set; }
        public string?  SAC_Code { get; set; }
        public string? Employee_Sap_Code { get; set; }
        public decimal? GstAmount { get; set; }
        public decimal? Actual_Credit_Note_Amount { get; set; }
        public string? Sap_Reference_Number { get; set; }
        public decimal? Tds_Percentage { get; set; }
        public decimal? Tds_Amount { get; set; }
        public string? User_Name { get; set; }
        public string? Posted_Date { get; set; }
        public string? IRNStatus { get; set; }
        public string? IRNNumber { get; set; }
        public string? DBNIRNStatus { get; set; }
        public string? DBNIRNNumber { get; set; }

    }

    public class CreditNotePurpose
    {
        public int? Credit_Note_Type_Id { get; set; }
        public string? Credit_Note_Type { get; set; }
    }

    public class CreditNoteSearch
    {
        public string? Purpose { get; set; }
        public int? Company_id { get; set; }
        public string? RefId { get; set; }
        public int? Pay_period_id { get; set; }
        public string? screentype { get; set; }
    }

    public class CreditNoteSearchApprove
    {
        public int? companyId { get; set; }
        public string? fromdate { get; set; }
        public string? todate { get; set; }
    }
}
