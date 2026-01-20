using QPay.UI.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.GlobalMaster
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "QuessLegalEntityDetails")]
    [System.Serializable()]

    public class QuessLegalEntityResponse
    {
        [System.Xml.Serialization.XmlElementAttribute("QuessLegalEntity")]
        public QuessLegalEntity[] QuessLegalEntityDetails { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class QuessLegalEntity
    {
        public int Id { get; set; }

        public string EntityName { get; set; }
        public int Serial_No { get; set; }
        public string? Error_Message { get; set; }
    }


    public class QuessLegalEntityRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public QuessLegalEntity parentDetail { get; set; }

    }
}
