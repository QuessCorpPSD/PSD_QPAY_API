using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QPay.UI.Models.TaxAndSaving
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "CompanyProvidedBenefitData")]
    [System.Serializable()]
    public class CompanyProvidedBenefitsResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("CompanyProvidedBenefit")]
        public CompanyProvidedBenefits[] objCompanyProvidedBenefits { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Company_Provided_Benefit")]
    public class CompanyProvidedBenefits
    {
        public int Company_Provided_Benefit_Id { get; set; }
        public string Company_Provided_Benefit_Date { get; set; }
        public int Employee_Id { get; set; }
        public int Financial_Year_Id { get; set; }
        public int Perk_Code_Id { get; set; }
        public decimal Perk_Amount { get; set; }
        public string Perk_Type { get; set; }
    }

    public class CompanyProvidedBenefitsRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public CompanyProvidedBenefits parentDetail { get; set; }

    }

}