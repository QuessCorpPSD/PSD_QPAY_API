using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace QPay.UI.Models.GlobalMaster
{
    public class LWFClass
    {
        public class LWFResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class LWFSearchRequest
        {
            public string? StateID { get; set; }
            public string? EffectiveDate { get; set; }
        }

        public class LWFSlabRequest
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public LWFSlab LWFSlab { get; set; } = new LWFSlab();
            public List<LWFSlabDetails> LWFSlabDetails { get; set; } = new List<LWFSlabDetails>();
        }

        public class LWFSlab
        {
            public int LWF_Slab_Id { get; set; }
            public int Financial_Year_Id { get; set; }
            public int State_Id { get; set; }
            public string? Effective_Date { get; set; }
        }

        public class LWFSlabDetails
        {
            public int LWF_Slab_Detail_Id { get; set; }
            public string? From_Value { get; set; }
            public string? To_Value { get; set; }
            public int Frequency_Id { get; set; }
            public int Month_Id { get; set; }
            public string? EmployerContribution { get; set; }
            public string? EmployeeContribution { get; set; }
        }

        [XmlRoot("LWFDetails")]
        public class LWFDetails
        {
            [XmlElement("LWF")]
            public LWFSlab LWFSlab { get; set; } = new LWFSlab();
        }

        // Wrapper for XML2
        [XmlRoot("LabourWelfareFareFundDetailsResponse")]
        public class LabourWelfareFareFundDetailsResponse
        {
            [XmlElement("LWFSLABDETAILS")]
            public List<LWFSlabDetails> LWFSlabDetails { get; set; } = new List<LWFSlabDetails>();
        }

    }
}
