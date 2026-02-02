using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "PreviousEmploymentTaxDetails")]
    [System.Serializable()]
    public class PreviousEmploymentTaxDetailsResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("PreviousEmployement")]
        public PreviousEmploymentTaxDetails[] PreviousEmploymentTaxDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Previous_Employment_Tax_Details")]
    public class PreviousEmploymentTaxDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Previous_Employment_Id { get; set; }
        public string Date { get; set; }
        public int Employee_Id { get; set; }
        public int Financial_Year_Id { get; set; }
        public decimal Income_After_Exemption_10 { get; set; }
        public decimal Tax_Paid { get; set; }

        public decimal Surcharge { get; set; }
        public decimal Education_Cess { get; set; }
        public decimal Total_Tax_Paid { get; set; }
        public decimal PF_Paid { get; set; }
        public decimal PT_Paid { get; set; }
        public int Declaration_Type_Id { get; set; }
    }

    public class PrevEmploymentRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public PreviousEmploymentTaxDetails parentDetail { get; set; }

    }

}