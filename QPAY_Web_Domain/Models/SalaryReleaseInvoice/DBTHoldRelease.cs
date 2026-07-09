using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class DBTHoldRelease
    {
        public string InvoiceNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string DBTReleaseAmount { get; set; }
        public string SalaryType { get; set; }

    }

    public class DBTHold
    {
        public string InvoiceNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string HoldAmount { get; set; }
        public string SalaryType { get; set; }
        public string HoldReason { get; set; }

    }

    public class DBTHoldMessage
    {
        public string Error_Message { get; set; } = "";

    }

    public class DBTHoldRequest
    {
        public int QZoneUserName { get; set; }
        public List<DBTHold> DBTHoldList { get; set; }

    }

    public class DBTRelease
    {
        public int QZoneUserName { get; set; }
        public List<DBTHoldRelease> DBTReleaseList { get; set; }

    }


}
