using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "ILHPDetails")]
    [System.Serializable()]
    public class ILHPropertyResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("ILHP")]
        public IncomeLossHouseProperty[] incomeLossHouseProperty { get; set; }
    }

    //  [Table("tbl_Income_Loss_On_House_Property")]
    public class IncomeLossHouseProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Income_Loss_On_House_Property_Id { get; set; }

        public string Declaration_Date { get; set; }
        public int Employee_Id { get; set; }
        public int Financial_Year_Id { get; set; }
        public string Tax_Code { get; set; }

        public decimal Income_On_House_Property { get; set; }
        public decimal Municipal_Tax_Paid { get; set; }
        public decimal Insurance_Charge_Paid { get; set; }
        public int Number_Letout_Property { get; set; }
        public decimal Letout_Eligible_Interest { get; set; }
        public string Letout_Effective_Date { get; set; }
        public int Number_Of_SelfOccupied_Property { get; set; }
        public decimal Interest_On_Housing_Loan { get; set; }
        public string SelfOccupied_Effective_Date { get; set; }
        public decimal Eligible_Interest_On_Housing_Loan { get; set; }
        public decimal Additional_Exemption { get; set; }
        public int Declaration_Type_Id { get; set; }
        public decimal Eligible_Housing_Loan { get; set; }
        public decimal Eligible_Housing_Exemption { get; set; }
        public decimal Repair_Collection_30_Percent { get; set; }
        public decimal Net_Annual_Value { get; set; }
        public decimal Eligible_Let_Out_Exemption { get; set; }
        public decimal Net_income_on_House_property { get; set; }
    }

    public class Declaration_Type
    {
        public int Declaration_Type_Id { get; set; }
        public string Declaration_Type_Name { get; set; }
    }

    // Added for income upload by Vijay P.V
    public class IncomeLossHousePropertyUpload
    {
        [Key]
        public string XML_File { get; set; }

        public int CreatedBy { get; set; }
        public string message { get; set; }
        public string Action { get; set; }
        public List<IncomeLossHouseProperty> Message { get; set; }
    }

    public class IncomeLossHousePropertyRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public IncomeLossHouseProperty parentDetail { get; set; }

    }

}