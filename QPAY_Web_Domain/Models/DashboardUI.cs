using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class DashboardUI
    {
        public double? total_minutes        { get; set; } = default(double);

        public double? total_hours { get; set; } = default(double);

        public double? days { get; set; } = default(double);

        public double? Extratime { get; set; } = default(double);
        public double? InComplate_Assignment { get; set; } = default(double);

        public double? OnTime_Assignment { get; set; } = default(double);

        public double? OverDue_Assignment { get; set; } = default(double);

        public int? Total_Assignment { get; set; } = default(int);
        public int? PendingAssignment { get; set; } = default(int);
        public int? completedAssignment { get; set; } = default(int);
        public int? OverDueAssignment { get; set; } = default(int);
        public int? OverDueMinutes { get; set; } = default(int);
        public bool CheckInStatus { get; set; }
        public DateTime? CheckInDateTime { get; set; }
     


    }
}
