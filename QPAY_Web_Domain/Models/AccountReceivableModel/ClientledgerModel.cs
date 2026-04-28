using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.AccountReceivableMod
{
    public class ClientLedgerExportRequest
    {
        public int CompanyId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
