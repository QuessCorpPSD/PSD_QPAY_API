using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "HRA_Details")]
    [System.Serializable()]
    public class HRAResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("HRA")]
        public HRA[] hra { get; set; }
    }

    public class HRACalculationDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("HRA_Calculation_Detail")]
        public HRA_Calculation_Detail[] hraCalculationDetail { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_HRA_Calculation")]
    public class HRA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HRA_Calculation_Id { get; set; }
        public string HRA_Calculation_Date { get; set; }
        public int Employee_Id { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public decimal Monthly_Rent_Paid { get; set; }
        public int Declaration_Type_Id { get; set; }
        public decimal Eligible_Basic { get; set; }
        public decimal Eligible_HRA { get; set; }
        public string Residing_Location { get; set; }
        public int Financial_Year_Id { get; set; }
        public bool IsMetroSelected { get; set; }
        public decimal Total_Exemption { get; set; }
    }

    [Table("tbl_HRA_Calculation_Detail")]
    public class HRA_Calculation_Detail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HRA_Calculation_Detail_Id { get; set; }
        public int HRA_Calculation_Id { get; set; }
        public int Pay_Frequency_Detail_Id { get; set; }
        public decimal Fixed_Basic { get; set; }
        public decimal Earned_Basic { get; set; }
        public decimal Fixed_HRA { get; set; }
        public decimal Earned_HRA { get; set; }
        public decimal Monthly_Rent_Paid { get; set; }
        public decimal HRA_Received { get; set; }
        public decimal Rent_Paid_Minus_Basic { get; set; }
        public decimal Percentage_Of_Basic { get; set; }
        public decimal HRA_Exemption { get; set; }
    }

   

    public class HRARequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public HRA parentDetail { get; set; }
        public List<HRA_Calculation_Detail> childDetail { get; set; }

    }
}