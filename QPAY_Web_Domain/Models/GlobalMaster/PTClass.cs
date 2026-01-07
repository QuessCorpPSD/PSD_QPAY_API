using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static QPay.UI.Models.GlobalMaster.ESIClass;
using static QPay.UI.Models.GlobalMaster.PTClass;

namespace QPay.UI.Models.GlobalMaster
{
    public class PTClass
    {
        public class PTResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class PTTypeUI
        {
            public string? PT_Type_Id { get; set; }
            public string? PT_Type_Name { get; set; }
            public string? PT_Type { get; set; }
        }

        public class PTCategoryUI
        {
            public string? Category_Id { get; set; }
            public string? Category_Name { get; set; }
        }

        public class PTCircleUI
        {
            public string? PTCircle_Id { get; set; }
            public string? PTCircle_Name { get; set; }
        }

        public class PTSearchRequest
        {
            public int StateID { get; set; }
            public string? EffectiveDate { get; set; }
            public int PT_Type { get; set; }
        }

        public class PTRequest
        {
            public string? mode { get; set; }
            public string? CreatedBy { get; set; }
            public PTSlab PTSlab { get; set; } = new PTSlab();
            public List<PTSlabDetail> PTSlabDetail { get; set; } = new List<PTSlabDetail>();
        }

        public class PTSlab
        {
            public int Professional_Tax_Slab_Id { get; set; }
            public string? Effective_Date { get; set; }
            public int State_Id { get; set; }
            public int PT_Type { get; set; }
            public int Category { get; set; }
            public int PTCircle_Id { get; set; }
            public int Month_Id { get; set; }
        }

        public class PTSlabDetail
        {
            public int Professional_Tax_Slab_Detail_Id { get; set; }
            public int Professional_Tax_Slab_Id { get; set; }
            public string? From_Value { get; set; }
            public string? To_Value { get; set; }
            public string? Amount { get; set; }

        }

        [XmlRoot("PTData")]
        public class PTData
        {
            [XmlElement("PTSlab")]
            public PTSlab PTSlab { get; set; } = new PTSlab();
        }

        // Wrapper for XML2
        [XmlRoot("PTSlabDetailsResponse")]
        public class PTSlabDetailsResponse
        {
            [XmlElement("PTSlabDetail")]
            public List<PTSlabDetail> PTSlabDetail { get; set; } = new List<PTSlabDetail>();
        }
    }
}
