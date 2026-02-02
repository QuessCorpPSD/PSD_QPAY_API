using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Report
{
    public class EntityReport
    {
        public class EntityPayregisterFilter
        {
            public int? EntityId { get; set; }
            public string? PayPeriod { get; set; }
        }

        public class EntityPayregister_Filter
        {
            public int? EntityId { get; set; }
            public string? FromDate { get; set; }

            public string? ToDate { get; set; }
            public int? ReportTypeId { get; set; }
            public int? UserId { get; set; }

        }

        public class PayperiodFilter
        {
            public string? PayPeriod { get; set; }
        }

        public class companywithdateFilter
        {
            public int? CompanyId { get; set; }
            public string? FromDate { get; set; }
            public string? ToDate { get; set; }

        }

    }
}
