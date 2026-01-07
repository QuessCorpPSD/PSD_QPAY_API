using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.GlobalMaster
{
    public class PFClass
    {
        public class PFResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }
        public class PFPayCodesUI
        {
            public string? Paycode_Id { get; set; }
            public string? Paycode_Code { get; set; }
            public string? Description { get; set; }
            public string? Print_As { get; set; }
            public string? Page_Type_Value { get; set; }
        }


        public class PFCapTypeUI
        {
            public string? Cap_Type_Id { get; set; }
            public string? Cap_Type_Name { get; set; }
        }

        public class PFRequest
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public PF PF { get; set; } = new PF();
            public List<PFDetail> PFDetail { get; set; } = new List<PFDetail>();
        }

        public class PFDeleteRequest
        {
            public int ProvidentFundId { get; set; }
            
        }

        public class PF
        {
            public int Provident_Fund_Id { get; set; }
            public string? Effective_Date { get; set; }
            public int PayCode_Id { get; set; }
            public int IsCapType { get; set; }
            public string? Criteria { get; set; }
        }

        public class PFDetail
        {
            public int Provident_Fund_Detail_Id { get; set; }
            public int Provident_Fund_Id { get; set; }
            public string? From_Value { get; set; }
            public string? To_Value { get; set; }
            public string? Criteria { get; set; }
            public int Criteria_Type_Id { get; set; }
            public string? Formula { get; set; }

        }

        [XmlRoot("PFData")]
        public class PFData
        {
            [XmlElement("PF")]
            public PF PF { get; set; } = new PF();
        }

        // Wrapper for XML2
        [XmlRoot("PFDetailsResponse")]
        public class PFDetailsResponse
        {
            [XmlElement("PFDetail")]
            public List<PFDetail> PFDetail { get; set; } = new List<PFDetail>();
        }
    }
}
