using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class PartialHoldRelease
    {
        public string InvoiceNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string PartialReleaseAmount { get; set; }
        public string SalaryType { get; set; }

    }
    public class PartialHold
    {
        public string InvoiceNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string HoldAmount { get; set; }
        public string SalaryType { get; set; }
        public string HoldReason { get; set; }

    }

    public class PartialHoldMessage
    {
        public string Error_Message { get; set; } = "";

    }

    public class PartilHoldRequest
    {
        public int QZoneUserName { get; set; }
        public List<PartialHold> PartialHoldList { get; set; }

    }

    public class PartialRelease
    {
        public int QZoneUserName { get; set; }
        public List<PartialHoldRelease> PartialReleaseList { get; set; }

    }


}
