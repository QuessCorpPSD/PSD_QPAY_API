using QPay.UI.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Reimbursements
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ReimbursementData")]
    [System.Serializable()]
    public class ReimbursementResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("Reimbursement")]
        public Reimbursement[] lstReimbursement { get; set; }
    }

    public class ReimbursementDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ReimbursementDetails")]
        public ReimbursementDetail[] ReimbursementDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Reimbursement")]
    public class Reimbursement
    {
       
        public int Reimbursement_Id { get; set; }
        public int Company_Id { get; set; }
        public string Client_Code { get; set; }
        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public int Financial_Year_Id { get; set; }
        public string Financial_Year_Name { get; set; }
        public string Pay_Period { get; set; }
        public int Pay_Period_Id { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
        public DateTime Reimbursement_Date { get; set; }

        public string Error_Message { get; set; }
    }

    [Table("tbl_Reimbursement_Detail")]
    public class ReimbursementDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SNo { get; set; }

        public int Reimbursement_Detail_Id { get; set; }
        public int Reimbursement_Id { get; set; }
        public string Tax_Id { get; set; }
        public int Paycode_Id { get; set; } // adding , after UAT changes
        public string Paycode_Code { get; set; }
        public int Reimbursement_Code { get; set; }
        public string Description { get; set; }
        public int Computation_Rule_Id { get; set; }
        public decimal Claim_Amount { get; set; }
        public string Error_Message { get; set; }
    }

    public class AllReimbursement
    {
        public int SNo { get; set; }
        public int Reimbursement_Id { get; set; }
        public int Company_Id { get; set; }
        public string Client_Code { get; set; }
        public int Employee_Id { get; set; }
        public string Employee_Code { get; set; }
        public int Financial_Year_Id { get; set; }
        public string Financial_Year_Name { get; set; }
        public string Pay_Period { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
        public int Pay_Period_Id { get; set; }
        public DateTime Reimbursement_Date { get; set; }

        public int Reimbursement_Detail_Id { get; set; }

        //public int Reimbursement_Id { get; set; }
        public string Tax_Id { get; set; }

        public int Paycode_Id { get; set; } // adding , after UAT changes
        public string Paycode_Code { get; set; }
        public decimal Claim_Amount { get; set; }
        public int Reimbursement_Code { get; set; }
        public string Description { get; set; }
        public int Computation_Rule_Id { get; set; }

        public string Error_Message { get; set; }
    }

    public class REIMFileUpload
    {
        [Key]
        public string XML_File { get; set; }

        public int CreatedBy { get; set; }
        public string message { get; set; }
    }

    public class REIMMessage
    {
        public List<REIMFileUpload> reimMessage { get; set; }
    }

    public class ReimbursementRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public Reimbursement parentDetail { get; set; }
        public List<ReimbursementDetail> childDetail { get; set; }

    }

}