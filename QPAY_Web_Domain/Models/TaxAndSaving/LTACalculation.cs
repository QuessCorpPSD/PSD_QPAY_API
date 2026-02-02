using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "LTACalculationDetail")]
    [System.Serializable()]
    public class LTACalculationResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("LTACalculation")]
        public LTACalculation[] LTACalculations { get; set; }
    }

    public class LTACalculationDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("LTACalculationDetail")]
        public LTACalculationDetail[] LTACalculationDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_LTA_Declaration")]
    public class LTACalculation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LTA_Declaration_Id { get; set; }
        public int Employee_Id { get; set; }
        public DateTime Claim_Date { get; set; }
        public decimal Claim_Amount { get; set; }
        public int Financial_Year_Id { get; set; }
        public int LTA_Block_Period_Id { get; set; }
        public int Declaration_Type_Id { get; set; }
        public DateTime Travel_From_Date { get; set; }
        public DateTime Travel_To_Date { get; set; }
        public string Travel_Location { get; set; }
    }

    [Table("tbl_LTA_Declaration_Detail")]
    public class LTACalculationDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LTA_Declaration_Detail_Id { get; set; }
        public int LTA_Declaration_Id { get; set; }
        public decimal Actual_Amount { get; set; }
        public decimal Eligible_Amount { get; set; }
        public decimal Exemption_Amount { get; set; }
        public string Remarks { get; set; }
        public bool Carry_Forward { get; set; }
    }


    public class LTACalculationRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public LTACalculation parentDetail { get; set; }
        public List<LTACalculationDetail> childDetail { get; set; }

    }
}