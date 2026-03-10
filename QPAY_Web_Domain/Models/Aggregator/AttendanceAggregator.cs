using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Aggregator
{
    public class AttendanceAggregator
    {
        public int LEAVE_MAPPING_DETAIL_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public int LEAVE_TYPE_ID { get; set; }
        public int LEAVE_TREAT_ID { get; set; }
        public Boolean ISACTIVE { get; set; }
        public int ATTENDANCE_TYPE { get; set; }
    }


    public class AttendanceAggregatorRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public AttendanceAggregator parentDetail { get; set; }

    }

    public class leaveTypeMaster
    {
        public int LEAVE_TYPE_ID { get; set; }
        public string LEAVE_TYPE_NAME { get; set; }
        public Boolean ISACTIVE { get; set; }
    }

    public class leaveTypeMasterRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public leaveTypeMaster parentDetail { get; set; }

    }

}
