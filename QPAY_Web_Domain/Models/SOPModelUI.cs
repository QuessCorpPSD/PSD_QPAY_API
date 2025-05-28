using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class SOPModelUI
    {
        public int UniqueId { get; set; }
        public int CategoryId { get; set; }
        public int QuestionId { get; set; }
        public int SubId { get; set; }
        public string QuestionName { get; set; } = "";
        public string Attribute { get; set; } = "";
        public string IsMulti { get; set; } = "";
        public int IsMandatory { get; set; }

        
    }
}
