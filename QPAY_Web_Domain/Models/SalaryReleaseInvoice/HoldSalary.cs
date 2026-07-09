using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class HoldSalary
    {
        public string Company_Code { get; set; }
        public string PayPeriod { get; set; }
        public string Employee_Code { get; set; }
        public string InvNo { get; set; }
        public string Hold_Status { get; set; }
        public string Reason { get; set; }
        public string SalaryType { get; set; }

    }

    public class SalaryHoldCommon
    {
        public int Company_Id { get; set; }

        public int Pay_Period_Id { get; set; }

        public string QZoneUserName { get; set; } = "";
    }

    public class HoldRequestMessage
    {
        public string Validation { get; set; } = "";

    }

    public class HoldSalaryRequest
    {
        public int QZoneUserName { get; set; }
        public List<HoldSalary> requestdata { get; set; }

    }
    // //
    public class SingleHoldRequest
    {
        public int QZoneUserName { get; set; }
        public List<HoldRequestCommon> HoldListData { get; set; }

    }
    public class HoldRequestCommon
    {
        public string Company_Code { get; set; }
        public string Pay_Period { get; set; }
        public string Employee_Code { get; set; }
        public string Invoice_no { get; set; }
        public string Flag { get; set; }
        public string Hold_Status { get; set; } = "";
        public string Hold_Amount { get; set; } = "";
        public string Reason { get; set; }
        public string SalaryType { get; set; }

    }


}
