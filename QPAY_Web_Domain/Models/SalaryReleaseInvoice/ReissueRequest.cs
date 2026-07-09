using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class ReissueRequest
    {
        public string Bank_Invoice_Id { get; set; } = "";
        public string Invoice_No { get; set; }
        public string Employee_Code { get; set; }
        public string SalaryType { get; set; }
    }

    public class ReissueRequestData
    {
        public int QZoneUserName { get; set; }
        public List<ReissueRequest> ReissueRequestList { get; set; }

    }

    public class ReissueRequestMessage
    {
        public string Error_Message { get; set; } = "";

    }


}
