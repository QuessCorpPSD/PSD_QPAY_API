using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Reimbursements
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "LoanPreClosureDetails")]
    [System.Serializable()]
    public class LoanPreClosureResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("LoanPreClosure")]
        public LoanPreClosure[] LoanPreClosureDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Loan_PreClose")]
    public class LoanPreClosure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Loan_PreClose_Id { get; set; }

        public int Employee_Id { get; set; }
        public int Employee_Loan_Id { get; set; }
        public string Employee_Code { get; set; }
        public string Employee_Name { get; set; }
        public decimal PreClose_Amount { get; set; }
        public DateTime Loan_PreClose_Date { get; set; }
        public bool Adjustment { get; set; }
        public string AdjustmentText { get; set; }
        public string Loan_Number { get; set; }
        public decimal Loan_Amount { get; set; }
        public decimal Paid_Amount { get; set; }
        public decimal Balance_Amount { get; set; }
        public decimal EMI { get; set; }
        public int Serial_No { get; set; }
        public string Error_Message { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
    }
}