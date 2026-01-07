using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Common
{
    public  class PayperiodDD
    {
        public int Payfrequencyid { get; set; }
        public string PaySequenceNo { get; set; } = string.Empty;
        public string PayPeriod { get; set; } = string.Empty;

        public DateTime? Start_At { get; set; }

        public DateTime? End_At { get; set; }
        public string displayName => string.Format("{0} - {1}", Payfrequencyid, PayPeriod);
    }
    public class CompanyPicker
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string DisplayName => string.Format("{0} ({1})", CompanyCode, CompanyName);
    }
}
