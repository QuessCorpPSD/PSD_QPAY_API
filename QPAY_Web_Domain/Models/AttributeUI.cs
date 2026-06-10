using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models
{
    public class AttributeUI
    {
        public int? id { get; set; }
        public string? AttributeName { get; set; } = string.Empty;
        public string? ActionType { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }

        public string? createdOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }

        public string? Messages { get; set; }

    }

    public class AttributeParams
    {
        public string CompanyId {get; set;} = "";
        public string PayPeriodId {get; set;} = "";
        public string LotNo {get; set;} = "";
        public string Input_No {get; set;} = "";
        public string Map_Name_Id {get; set;} = "";
        public string Invoice_Category_Id { get; set; } = "";
        public string CreatedBy { get; set; } = "";
    }
    [XmlRoot("NewDataSet")]
    public class AttributeValues
    {
        [XmlElement("Table")]
        public List<AttributeParams> details { get; set; } = new List<AttributeParams>();
        public string cols { get; set; } = "";
        public string flag { get; set; } = "";
    }
}
