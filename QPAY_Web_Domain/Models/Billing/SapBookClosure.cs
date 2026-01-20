using QPay.UI.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Billing
{
    public class SapBookClosure
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public string ClosureDate { get; set; }
        public string Error_Message { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
    }

    public class SapBookClosureRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public SapBookClosure parentDetail { get; set; }

    }

}
