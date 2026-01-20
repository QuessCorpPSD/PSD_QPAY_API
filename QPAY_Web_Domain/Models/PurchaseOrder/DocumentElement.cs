using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI_Domain.Models.PurchaseOrder
{
    [XmlRoot("DocumentElement")]
    public class DocumentElement
    {
        [XmlElement("DOJ")]
        public List<DOJ> DOJList { get; set; } = new List<DOJ>();
    }
    public class DOJ
    {
        [XmlElement("Company_Code")]
        public string CompanyCode { get; set; } = string.Empty;

        [XmlElement("PoNumber")]
        public string PoNumber { get; set; }= string.Empty;

        [XmlElement("POValue")]
        public string POValue { get; set; }=string.Empty;

        [XmlElement("PODate")]
        public string PODate { get; set; } = string.Empty;    // keep string if format is dd/MM/yyyy

        [XmlElement("StartDate")]
        public string StartDate { get; set; } = string.Empty;

        [XmlElement("EndDate")]
        public string EndDate { get; set; } = string.Empty;

        [XmlElement("Pricing_Type")]
        public string PricingType { get; set; } = string.Empty;

        [XmlElement("CurrencyType")]
        public string CurrencyType { get; set; } = string.Empty;

        [XmlElement("POQuantuty_Type")]
        public string POQuantityType { get; set; } =string.Empty;

        [XmlElement("Billing_Type")]
        public string BillingType { get; set; } = string.Empty;

        [XmlElement("InternalExternal")]
        public string InternalExternal { get; set; } = string.Empty;

        [XmlElement("ExtentionEndDate")]
        public string ExtentionEndDate { get; set; } = string.Empty;

        [XmlElement("ExtentionPOValue")]
        public string ExtentionPOValue { get; set; } = string.Empty;

        [XmlElement("PO_Category")]
        public string POCategory { get; set; } = string.Empty;

        [XmlElement("Mode")]
        public string Mode { get; set; }= string.Empty;
    }


}
