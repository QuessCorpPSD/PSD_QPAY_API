using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.GlobalMaster
{
    public class GstRequest
    {
        public string Action { get; set; } = "";
        public string UserId { get; set; } = "";
        public int GstMasterId { get; set; }
        public string? EffectiveDate { get; set; }
        public string GstNumber { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string CompanyAddress { get; set; } = "";
        public int CreatedBy { get; set; }
        public decimal Gst_Percentage { get; set; }
        public int EntityId { get; set; }
        public int Pincode { get; set; }


    }
}
