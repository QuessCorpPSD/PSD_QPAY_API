using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class DashboardUI
    {
        public int TotalMinutes { get; set; }

        public int TotalHours { get; set; }

        public int TotalDays { get; set; }

        public int TotalAssignment { get; set; }
        public int PendingAssignment { get; set; }

        public int ComplateAssignment { get; set; }

        public int OverDueAssignment { get; set; }

        public int OverDueMinutes { get; set; }

    }
}
