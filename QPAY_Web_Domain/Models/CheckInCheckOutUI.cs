using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
  public  class CheckInCheckOutUI
    {
        public int User_Timesheet_Id { get; set; }
        public int User_Id { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
