using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Reports
{
    public class Payslip
    {
        public class PayslipDownloadResponse
        {
            public string response { get; set; } = string.Empty;
            public string base64string { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }
    }
}
