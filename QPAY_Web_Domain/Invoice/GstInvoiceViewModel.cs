using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Invoice
{
    public class GstInvoiceViewModel : BaseModel
    {
        public GstInvoiceViewModel()
        {
            IsActive = true;
            CreatedOn = DateTime.Now;
            //Invoice_Date = DateTime.Now;
            Invoice_Due_Date = DateTime.Now;
            Input_Date = DateTime.Now;
            Output_Date = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }

        [Required]
        [Display(Name = "Invoice Id")]
        public Int32 Invoice_Id { get; set; }

        [Required]
        [Display(Name = "Invoice Number")]
        public String Invoice_Number { get; set; }

        [Required]
        //[Display(Name = "Company Id")]
        public Int32 Company_Id { get; set; }

        [Required]
        //[Display(Name = "Cost Center Mapping Id")]
        public Int32 Cost_Center_Mapping_Id { get; set; }

        //[Display(Name="City  Id")]
        public Int32? City_Id { get; set; }

        [Required]
        //[Display(Name="Financial  Year  Id")]
        public Int32 Financial_Year_Id { get; set; }

        [Required]
        //[Display(Name="Pay  Period  Id")]
        public Int32 Pay_Period_Id { get; set; }

        [Required]
        //[Display(Name="Invoice  Type  Id")]
        public Int32 Invoice_Type_Id { get; set; }

        [Display(Name = "Invoice  Date")]
        public DateTime? Invoice_Date { get; set; }

        //[Display(Name="Invoice  Due  Date")]
        public DateTime? Invoice_Due_Date { get; set; }

        [Display(Name = "Particulars")]
        public String Particulars { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public Decimal Amount { get; set; }

        [Required]
        [Display(Name = "State Id")]
        public Int32 StateId { get; set; }

        [Display(Name = "Invoicing State Id")]
        public Int32? InvoicingStateId { get; set; }

        [Required]
        [Display(Name = "CGST  Percentage")]
        public Decimal CGST_Percentage { get; set; }

        [Required]
        [Display(Name = "SGST  Percentage")]
        public Decimal SGST_Percentage { get; set; }

        [Required]
        [Display(Name = "UTGST  Percentage")]
        public Decimal UTGST_Percentage { get; set; }

        [Required]
        [Display(Name = "IGST  Percentage")]
        public Decimal IGST_Percentage { get; set; }

        //[Display(Name="Client PO")]
        public Boolean? Client_PO { get; set; }

        //[Display(Name="Purchase  Order  Id")]
        public Int32? Purchase_Order_Id { get; set; }

        [Display(Name = "Input  Date")]
        public DateTime? Input_Date { get; set; }

        [Display(Name = "Output  Date")]
        public DateTime? Output_Date { get; set; }
        [Display(Name = "Service  Charge @")]
        public Decimal? Service_Charge { get; set; }

        [Display(Name = "Service  Charge  Amount")]
        public Decimal? Service_Charge_Amount { get; set; }

        [Display(Name = "Sourcing  Fee @")]
        public Decimal? Sourcing_Fee { get; set; }

        [Display(Name = "Sourcing  Fee  Amount")]
        public Decimal? Sourcing_Fee_Amount { get; set; }

        [Display(Name = "No  Of  Employees")]
        public Int32? No_Of_Employees { get; set; }

        [Display(Name = "Absorption  Fee @")]
        public Decimal? Absorption_Fee { get; set; }
        [Display(Name = "Absorption  Amt")]
        public Decimal? Absorption_Amt { get; set; }

        [Display(Name = "CTC  Amount  Adjusted")]
        public Decimal? CTC_Amt_Adjusted { get; set; }

        [Display(Name = "CTC Amount  Plus Or Minus")]
        public Boolean? CTC_Amt_NorP { get; set; }

        [Display(Name = "CTCAdj Note")]
        public String CTC_Adj_Note { get; set; }

        [Display(Name = "Net  Amount  Adjusted")]
        public Decimal? Net_Amt_Adjusted { get; set; }
        [Display(Name = "Net  Amount  Plus Or Minus")]
        public Boolean? Net_Amt_NorP { get; set; }
        [Display(Name = "Net Adj Note")]
        public String Net_Adj_Note { get; set; }

        [Required]
        //[Display(Name = "Invoice Culture Id")]
        public Int32 Invoice_Culture_Id { get; set; }

        //[Display(Name = "Invoice Culture Ref No")]
        public String Invoice_Culture_RefNo { get; set; }

        [Display(Name = "Input  No")]
        public Int32? Input_No { get; set; }

        [Display(Name = "Employee ESI")]
        public Decimal? Employee_ESI { get; set; }

        [Display(Name = "Employer ESI")]
        public Decimal? Employer_ESI { get; set; }

        [Display(Name = "Employee PF")]
        public Decimal? Employee_PF { get; set; }

        [Display(Name = "Employer PF")]
        public Decimal? Employer_PF { get; set; }

        [Display(Name = "Mobile Recovery Amount")]
        public Decimal? Mobile_Recovery_Amount { get; set; }

        [Display(Name = "Personal Loan Amount")]
        public Decimal? Personal_Loan_Amount { get; set; }

        [Display(Name = "Other Deduction Amount")]
        public Decimal? Other_Deduction_Amount { get; set; }

        [Display(Name = "WONumber")]
        public String WO_Number { get; set; }

        [Display(Name = "Pl Id No")]
        public String Pl_Id_No { get; set; }

        [Display(Name = "Employee Name")]
        public String Employee_Name { get; set; }

        [Display(Name = "Markup")]
        public Decimal? Markup { get; set; }

        [Display(Name = "Gri Msp")]
        public Decimal? Gri_Msp { get; set; }

        [Display(Name = "DONumber")]
        public String DO_Number { get; set; }

        [Display(Name = "Created  Mode")]
        public String Created_Mode { get; set; }
        [Display(Name = "Remarks")]
        public String Remarks { get; set; }

        [Display(Name = "Status")]
        public String Status { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        [Display(Name = "WO  Date")]
        public DateTime? WO_Date { get; set; }

        [Display(Name = "Invoice Notes")]
        public String InvoiceNotes { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public Int32 CreatedBy { get; set; }

        [Required]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified By")]
        public Int32? ModifiedBy { get; set; }

        [Display(Name = "Modified On")]
        public DateTime? ModifiedOn { get; set; }

        [Display(Name = "Is Locked")]
        public Boolean IsLocked { get; set; }

        [Display(Name = "Discrepancy  By")]
        public String Discrepancy_By { get; set; }

        [Display(Name = "Discrepancy  Reason")]
        public String Discrepancy_Reason { get; set; }
        [Display(Name = "Onboarding Charge")]
        public Decimal? Onboarding_Charge { get; set; }
        [Display(Name = "Group  Detail  Id")]
        public Int32? Group_Detail_Id { get; set; }
        [Required]
        [Display(Name = "Group  Name")]
        public String Group_Name { get; set; }

        [Display(Name = "Taxable Amount1")]
        public Decimal? TaxableAmount1 { get; set; }

        [Display(Name = "Taxable Amount1  Note")]
        public String TaxableAmount1_Note { get; set; }

        [Display(Name = "Taxable Amount2")]
        public Decimal? TaxableAmount2 { get; set; }

        [Display(Name = "Taxable Amount2  Note")]
        public String TaxableAmount2_Note { get; set; }

        [Display(Name = "Taxable Amount3")]
        public Decimal? TaxableAmount3 { get; set; }

        [Display(Name = "Taxable Amount3  Note")]
        public String TaxableAmount3_Note { get; set; }

        [Display(Name = "Non Taxable Amount1")]
        public Decimal? NonTaxableAmount1 { get; set; }

        [Display(Name = "Non Taxable Amount1  Note")]
        public String NonTaxableAmount1_Note { get; set; }

        [Display(Name = "Non Taxable Amount2")]
        public Decimal? NonTaxableAmount2 { get; set; }
        [Display(Name = "Non Taxable Amount2  Note")]
        public String NonTaxableAmount2_Note { get; set; }

        [Display(Name = "Non Taxable Amount3")]
        public Decimal? NonTaxableAmount3 { get; set; }

        [Display(Name = "Non Taxable Amount3  Note")]
        public String NonTaxableAmount3_Note { get; set; }

        [Display(Name = "CGST  Amount")]
        public Decimal? CGST_Amount { get; set; }

        [Display(Name = "SGST  Amount")]
        public Decimal? SGST_Amount { get; set; }

        [Display(Name = "UTGST  Amount")]
        public Decimal? UTGST_Amount { get; set; }

        [Display(Name = "IGST  Amount")]
        public Decimal? IGST_Amount { get; set; }

        [Display(Name = "Net  Amount")]
        public Decimal? Net_Amount { get; set; }

        [Display(Name = "Company Code")]
        public String Company_Code { get; set; }

        [Display(Name = "Invoice Type")]
        public String InvoiceType { get; set; }

        [Required]
        [Display(Name = "Location")]
        public String City_Name { get; set; }

        [Required]
        [Display(Name = "Map Name")]
        public String Map_Name { get; set; }

        [Required]
        [Display(Name = "PO Number")]
        public String Purchase_Request_No { get; set; }

        [Required]
        //[Display(Name = "Financial Year")]
        public String Financial_Year_Name { get; set; }

        [Required]
        [Display(Name = "State")]
        public String State_Name { get; set; }

        [Required]
        [Display(Name = "Pay Period")]
        public String Pay_Period { get; set; }

        [Required]
        [Display(Name = "Billable Type")]
        public int Billable_Type_Id { get; set; }
        public string Billable_Type { get; set; }

        [Display(Name = "Compliance Fee @")]
        public Decimal? Compliance_Fee { get; set; }

        [Display(Name = "Compliance Fee Amount")]
        public Decimal? Compliance_Fee_Amount { get; set; }

        [Display(Name = "CTC Deduction Type")]
        public Int32? Ctc_Deduction_Type_Id { get; set; }
        [Display(Name = "CTC Deduction Type")]
        public String Ctc_Deduction_Type { get; set; }

        [Display(Name = "Net Deduction Type")]
        public Int32? Net_Deduction_Type_Id { get; set; }
        [Display(Name = "Net Deduction Type")]
        public String Net_Deduction_Type { get; set; }

        [Display(Name = "IRN Status")]
        public String IRN_Status { get; set; }

        [Display(Name = "IRN Number")]
        public String IRN_Number { get; set; }

        [Display(Name = "IRN Cancelled Date")]
        public DateTime? IRN_Cancel_Date { get; set; }

        [Display(Name = "Credit Note Number")]
        public String Crn_Number { get; set; }

        [Display(Name = "Credit Note IRN Status")]
        public String Crn_IRN_Status { get; set; }

        [Display(Name = "Credit Note IRN Number")]
        public String Crn_IRN_Number { get; set; }

        [Display(Name = "Credit Note IRN Cancelled Date")]
        public DateTime? Crn_IRN_Cancel_Date { get; set; }

        [Display(Name = "SAP Invoice No")]
        public String Sap_Invoice_Number { get; set; }

        [Display(Name = "SAP Account No")]
        public String Sap_Account_Number { get; set; }

        [Display(Name = "SAP Cancellation Doc No")]
        public String Sap_Cancel_Document { get; set; }

        [Display(Name = "SAP Credit Note No")]
        public String Sap_Credit_Note_Document { get; set; }
    }

    [DataContract]
    public class BaseModel
    {
        public BaseModel()
        {
            PageNo = 1;
            PageSize = 10;
        }

        // Default parameters
        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string SearchText { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string XmlData { get; set; }

        // Paging parameters
        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public int PageNo { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public string SortField { get; set; }
        [DataMember]
        public string SortDirection { get; set; }
    }
}
