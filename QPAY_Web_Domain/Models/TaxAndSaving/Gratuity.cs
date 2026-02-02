using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "GratuityDetails")]
    [System.Serializable()]
    public class GratuityResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("Gratuity")]
        public Gratuity[] GratuityDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Gratuity")]
    public class Gratuity

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Gratuity_Id { get; set; }
        public string Gratuity_Date { get; set; }
        public int Employee_Id { get; set; }
        public int Financial_Year_Id { get; set; }
        public int Year_Of_Service { get; set; }
        //public Boolean Tax_Exemption { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Basic { get; set; }
        public Decimal DA { get; set; }
    }

    public class GratuityRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public Gratuity parentDetail { get; set; }

    }

}