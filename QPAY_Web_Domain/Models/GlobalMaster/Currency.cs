using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.GlobalMaster
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "currency")]
    [System.Serializable()]
    public class Currency
    {
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyName { get; set; }

    }

    public class GetCurrencyRequest
    {
        public string? xmldata { get; set; }
        public string? mode { get; set; }
        public string? UserId { get; set; }
    }

    public class CurrencyConversionRequest
    {
        [System.Xml.Serialization.XmlElementAttribute("CurrencyConversion")]
        public CurrencyConversion currency { get; set; } = new CurrencyConversion();
        public string? mode { get; set; }
        public int UserId { get; set; }

    }

    [Table("tbl_CurrencyConversion")]
    public class CurrencyConversion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrencyConversionId { get; set; }
        public int CurrencyId { get; set; }
        public int Company_Id { get; set; }
        public string? ExchangeRate { get; set; }
        public string? ExchangeDate { get; set; }
        public int InvoiceCurrencyId { get; set; }
    }
}
