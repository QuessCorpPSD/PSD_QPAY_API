using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class AdminDashboardUI
    {
        public int TotalLot {  get; set; }
        public int CompletedLot { get; set; }
        public int PendingLot { get; set; }
        public int overdue_lot { get; set; }
        public int Ontimer_lot { get; set; }
        public int NotAllotted { get; set; }
        public int yettocome { get; set; }
        public int Processing {  get; set; }

        public int? InvoiceTotal { get; set; }
        public int? InvoiceInprogress { get; set; }
        public int? InvoiceCompleted { get; set; }
        public int? InvoicePending { get; set; }
        public int? InvoiceYetToCome { get; set; }

        public int? TotalInvoiceAmount { get; set; }
        public int? CompletedInvoiceAmount { get; set; }
        public int? PendingInvoiceAmount { get; set; }
        public int? yettoComeInvoiceAmount { get; set; }

    }
}
