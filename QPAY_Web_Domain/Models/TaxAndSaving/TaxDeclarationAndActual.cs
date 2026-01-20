using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving

{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "TaxDeclarationAndActualDetails")]
    [System.Serializable()]
    public class TaxDeclarationAndActualResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("TaxDeclarationActual")]
        public TaxDeclarationAndActual[] TaxDeclarationAndActualDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Tax_Declaration_Actual")]
    public class TaxDeclarationAndActual
    {
        //public string Message { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Tax_Declaration_Actual_Id { get; set; }

        public int SNo { get; set; }

        public string Company_Code { get; set; }
        public int Company_Id { get; set; }
        public int Employee_Id { get; set; } //tbl_employee
        public string Employee_Code { get; set; }//tbl_employee
        public string Employee_Name { get; set; } //tbl_employee
        public int Financial_Year_Id { get; set; }
        public string Financial_Year { get; set; }
        public DateTime Tax_Declaration_Actual_Date { get; set; }
        public int Computation_Rule_Id { get; set; }
        public string Computation_Rule { get; set; }
        public string Computation_Rule_Category_Name { get; set; }//for Section
        public string Category { get; set; }//for Section
        public string Description { get; set; }
        public Decimal Eligible_Amount { get; set; }
        public int Declaration_Type_Id { get; set; }
        public string Citizen_Category { get; set; }
        public string Tax_Code { get; set; }
        public int No_Of_Children { get; set; }
        public string Type { get; set; }
        public Decimal Amount { get; set; }
        public string Error_Message { get; set; }
    }

    public class TaxCodeDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Computation_Rule_Id { get; set; }

        public int Computation_Rule_Category_Id { get; set; }
        public string Computation_Rule_Category_Name { get; set; }//for Section
        public string Category { get; set; }//for Section
        public string Tax_Id { get; set; }
        public string Computation_Rule { get; set; }//for Formula
        public string Description { get; set; }
        public Decimal Amount { get; set; }
    }

    //public class Message
    //{
    //    public string Error_Message { get; set; }
    //}

    //public class TaxDeclarationAndActualUpload
    //{
    //    [Key]
    //    public string XML_File { get; set; }
    //    public int CreatedBy { get; set; }
    //    public string message { get; set; }
    //    public List<TaxDeclarationAndActual> _TDAMessage { get; set; }
    //}

    public class TaxDecAndActImport
    {
        public string Error_Message { get; set; }
    }

    public class TaxDeclarationAndActualUpload
    {
        [Key]
        public string XML_File { get; set; }

        public int CreatedBy { get; set; }
        public string message { get; set; }
        public List<TaxDecAndActImport> _TaxDeclarationActualMessage { get; set; }
    }

    public class TaxDeclarationAndActualRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public TaxDeclarationAndActual parentDetail { get; set; }

    }


}