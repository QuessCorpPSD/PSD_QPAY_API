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
    }
}
