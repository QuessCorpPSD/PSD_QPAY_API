using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models.AccountReceivable
{
    public class CollectionPendingExport
    {
        public int CompanyId { get; set; }
        public int FinancialId { get; set; }
        public string AsOnDate { get; set; }
        public string AllEntityId { get; set; }
    }
}
