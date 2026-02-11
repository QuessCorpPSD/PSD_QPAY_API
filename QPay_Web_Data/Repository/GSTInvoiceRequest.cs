namespace QPay.DAL.Repository
{
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
        public DateTime Invoice_Date { get; set; }
        public DateTime Invoice_Due_Date { get; set; }
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

    public class FileJson
    {
        public string? FilePath { get; set; }
        //public string? FileName { get; set; }
    }
}