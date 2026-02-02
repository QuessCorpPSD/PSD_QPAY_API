using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace QPay.UI.Customer
{
    /// <summary>
    /// User DB Object
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "CompanyPayCodesDetails")]
    [System.Serializable()]
    public class CompanyPayCodeResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("CompanyPayCode")]
        public CompanyPayCode[] CompanyPayCodes { get; set; }
    }

    public class CompanyPayCodeDetailResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("CompanyPayCodeDetail")]
        public CompanyPayCodeDetail[] CompanyPayCodeDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Table("tbl_Company_Paycode_Mapping")]
    public class CompanyPayCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Company_Paycode_Mapping_Id { get; set; }

        public int Company_Id { get; set; }
    }

    [Table("tbl_Company_Paycode_Mapping_Detail")]
    public class CompanyPayCodeDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Company_Paycode_Mapping_Detail_Id { get; set; }

        public int Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Description { get; set; }
        public string PayType { get; set; }
        public string Taxable { get; set; }

        //public bool Is_LOP_Applicable { get; set; }
        //public bool Is_PF_Applicable { get; set; }
        //public bool Is_ESI_Applicable { get; set; }
        //public bool Is_PT_Applicable { get; set; }
        public string LOP_Applicable { get; set; }

        public string PF_Applicable { get; set; }
        public string ESI_Applicable { get; set; }
        public string PT_Applicable { get; set; }
        public string Pick_From { get; set; }
        public string EarnedPaycode_Code { get; set; }
        public int SNo { get; set; }
        public int Company_Paycode_Pick_From_Id { get; set; }
        public string Company_Paycode_Pick_From_Value { get; set; }
        public string City_Name { get; set; }
        public int City_Id { get; set; }

        public int Pay_Register_Order { get; set; } 

    }

    public class CompanyPayCodeWithDetail
    {
        public int Company_Paycode_Mapping_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Code { get; set; }
        public int Company_Paycode_Mapping_Detail_Id { get; set; }
        public int Paycode_Id { get; set; }
        public string Paycode_Code { get; set; }
        public string Description { get; set; }
        public string PayType { get; set; }
        public string Taxable { get; set; }

        //public bool Is_LOP_Applicable { get; set; }
        //public bool Is_PF_Applicable { get; set; }
        //public bool Is_ESI_Applicable { get; set; }
        //public bool Is_PT_Applicable { get; set; }
        public string LOP_Applicable { get; set; }

        public string PF_Applicable { get; set; }
        public string ESI_Applicable { get; set; }
        public string PT_Applicable { get; set; }
        public string Pick_From { get; set; }
        public string EarnedPaycode_Code { get; set; }
        public string Error_Message { get; set; }
        public int SNo { get; set; }
        public int Company_Paycode_Pick_From_Id { get; set; }
        public string Company_Paycode_Pick_From_Value { get; set; }
        public string Company_Name { get; set; }
        public string City_Name { get; set; }
        public int City_Id { get; set; }
        public int Pay_Register_Order { get; set; }

        public string? Formula { get; set; }

    }

    public class CompanyPayCodePickFrom
    {
        public int Company_Paycode_Pick_From_Id { get; set; }
        public string Company_Paycode_Pick_From_Value { get; set; }
    }

    public class PaycodeRequest
    {
        public int? Company_Paycode_Mapping_Id { get; set; }
        public int? Pay_Structure_Id { get; set; }
        public int Company_Id { get; set; }
        public int User_Id { get; set; }
        public string? Mode { get; set; }
        public List<PaycodeDetail> PaycodeDetail { get; set; }

    }
    public class PaycodeDetail
    {
        public int Paycode_Id { get; set; }
        public string EarnedPaycode_Code { get; set; }
        public int? Company_Paycode_Pick_From_Id { get; set; }
        public int? Company_Paycode_Mapping_Detail_Id { get; set; }
        public int? Pay_Structure_Detail_Id { get; set; }
        public int SNo { get; set; }
        public int Execution_Order { get; set; }
        public string? Formula { get; set; }
    }


    [XmlRoot("CompanyPayCodesDetails")]
    public class CompanyInfo
    {
        [XmlElement("CompanyPayCode")]
        public int Company_Id { get; set; }
        public int? Company_Paycode_Mapping_Id { get; set; }
        public int? Pay_Structure_Id { get; set; }
    }

    [XmlRoot("CompanyPayCodeDetailResponse")]
    public class PaycodeDetailList
    {
        [XmlElement("CompanyPayCodeDetail")]
        public List<PaycodeDetail> Items { get; set; }
    }
}