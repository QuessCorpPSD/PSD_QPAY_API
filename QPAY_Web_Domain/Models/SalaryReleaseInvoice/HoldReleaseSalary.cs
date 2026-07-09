using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class HoldReleaseSalary
    {
        public string Serial_No { get; set; }
        public string Company_Code { get; set; }
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        public string Invoice_No { get; set; }
        public string Employee_Code { get; set; }
        public string Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public string Pay_Period { get; set; }
        public int Pay_Period_Id { get; set; }
        public decimal Net_Pay { get; set; }
        public string Salary_Hold_Type { get; set; }
        public string SalaryType { get; set; }

    }

    public class HoldReleaseRequest
    {
        public List<HoldReleaseColums> HoldReleaseList { get; set; }
        public string QZoneUserName { get; set; }

    }
    public class HoldReleaseColums
    {
        public string Company_Code { get; set; }
        public string Employee_Code { get; set; }
        public string PayPeriod { get; set; }
        public string InvNo { get; set; }
        public string SalaryType { get; set; } = "";
        public string ProvisionalInvoiceNumber { get; set; } = "";


    }
    public class SalaryHoldReleaseCommon
    {
        public int Company_Id { get; set; }

        public int Pay_Period_Id { get; set; }

        public string Employee_Id { get; set; } = "";

        public string QZoneUserName { get; set; } = "";
    }

    public class HoldReleaseMessage
    {
        public string Error_Message { get; set; } = "";

    }

}
