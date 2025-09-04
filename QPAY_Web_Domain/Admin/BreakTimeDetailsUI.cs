using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Admin
{
    public class BreakTimeDetailsUI
    {
        public int? BreakId { get; set; }
        public string Description { get; set; } = string.Empty;
        public TimeSpan starttime { get; set; }   // changed
        public TimeSpan endtime { get; set; }     // changed
        public string ProcessCategory { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    public class BreakTimeDetailRequest
    {
        public int? BreakId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string startTime { get; set; }

        public string EndTime { get; set; }
        public string ProcessCategory { get; set; }
         
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
       

    }
    

    public class EmployeeBreakUI
    {
        public int? BreakId { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int? TotalMinutes { get; set; }
        public int? UserBreakId { get; set; }
        public int? UserId { get; set; }
        public DateTime? BreakDate { get; set; }

        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }

        public string? Remarks { get; set; } = string.Empty;
    }
    public class BreakTimeResponse
    {
        public int? StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }

   
}
