using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
   public class CompanyData
    {
        public int AssignmentNumber { get; set; }
        public string CompanyCode { get; set; }

        public string Category { get; set; }
        public string HeadCount { get; set; }
        public string Revised { get; set; }

        public string EstimateTime { get; set; }

        public NewJoinee NewJoinee { get; set; }
        public Attendance Attendance { get; set; }
        public Adhoc Adhoc { get; set; }
        public Increment Increment { get; set; }
        public OtherInput OtherInput { get; set; }
    }
    public class OtherInput
    {
        public int Input { get; set; }
        public int Output { get; set; }
        public bool Ismatching { get; set; }
        public string Remarks { get; set; }
    }
    public class Increment
    {
        public int Input { get; set; }
        public int Output { get; set; }
        public bool Ismatching { get; set; }
        public string Remarks { get; set; }
    }
    public class Adhoc
    {
        public int Input { get; set; }
        public int Output { get; set; }
        public bool Ismatching { get; set; }
        public string Remarks { get; set; }
    }
    public class NewJoinee
    {
        public int Input { get; set; }
        public int Output { get; set; }
        public bool Ismatching { get; set; }
        public string Remarks { get; set; }
    }
    public class Attendance
    {
        public int Input { get; set; }
        public int Output { get; set; }
        public bool Ismatching { get; set; }
        public string Remarks { get; set; }
    }

    public class Payload
    {
        public List<CompanyData> Yesterday_Lot { get; set; }

        public List<CompanyData> TodayDay_Lot { get; set; }
    }
}
