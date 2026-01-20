using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Reimbursements
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "LoanAndAdvancescData")]
    [System.Serializable()]
    public class LoanAndAdvancesGridResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("LoanAndAdvanceGrid")]
        public LoanAndAdvancesc[] LoanAndAdvancesGrid { get; set; }
    }

    public class LoanAndAdvancesGridDetailResponse
    {
        //[System.Xml.Serialization.XmlElementAttribute("LoanAndAdvanceGrid")]
        [System.Xml.Serialization.XmlElementAttribute("LoanAndAdvancescData")]
        public LoanAndAdvanceGrid[] LoanAndAdvancesDetailGrid { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Employee_Loan")]
    public class LoanAndAdvancesc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SNo { get; set; }

        public int Employee_Loan_Id { get; set; }
        public DateTime Employee_Loan_Date { get; set; }
        public int Employee_Id { get; set; }
        public int Loan_Type_Id { get; set; }
        public string Loan_Number { get; set; }
        public decimal Opening_Balance { get; set; }
        public decimal Loan_Advance_Amount { get; set; }
        public DateTime Start_Date { get; set; }
        public int Interest_Type { get; set; }
        public decimal Bank_Interest { get; set; }
        public decimal Interest_Rate_Given { get; set; }
        public decimal Perk_Percentage { get; set; }
        public int Number_Of_Installment { get; set; }
        public decimal EMI { get; set; }
        public int Pay_Category_Id { get; set; }
        //public bool IsActive { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int ModifiedBy { get; set; }
        //public DateTime ModifiedOn { get; set; }

        public string Company_Code { get; set; }
        public string Employee_Code { get; set; }
        public string Employee_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Department_Name { get; set; }
        public string Designation_Name { get; set; }
        public string Loan_Type { get; set; }

        // public int Paycode_ID { get; set; }
        public int Company_Id { get; set; }

        public decimal Outstanding_Principal { get; set; }
        public string Pay_Sequence_Number { get; set; }
        public string Pay_Period { get; set; }
        public int Pay_Period_Id { get; set; }
        public decimal Interest { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest_Percentage { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
        public int Paycode_Id { get; set; }

        public string Error_Message { get; set; }
        //  public string Date { get; set; }

        //public String Perk_Name { get; set; }
        //public int Perk_Id { get; set; }
        public int Count { get; set; }
    }

    public class LoanAndAdvanceGrid
    {
        public decimal Outstanding_Principal { get; set; }
        public string Pay_Sequence_Number { get; set; }
        public string Pay_Period { get; set; }
        public int Pay_Period_Id { get; set; }
        public decimal Interest { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest_Percentage { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
        public decimal EMI { get; set; }
        public int Employee_Loan_Detail_Id { get; set; }
        public int Employee_Loan_Id { get; set; }
        public int Count { get; set; }
    }

    public class LoanFileUpload
    {
        [Key]
        public string XML_File { get; set; }

        public int CreatedBy { get; set; }
        public string message { get; set; }
    }
}