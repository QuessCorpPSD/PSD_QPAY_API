using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class UanRelease
    {
        public string Entity_Name { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string InvoiceNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PayPeriod { get; set; }
        public string NetPay { get; set; }
        public string HoldStatus { get; set; }
        public string UANRemarks { get; set; }


    }

    public class UanReleaseRequest
    {
        public string QZoneUserName { get; set; } = "";
        public List<UanRelease> UanReleaselist { get; set; }

    }
    public class UanReleaseCommon
    {
        public int Entity_Id { get; set; }

        public int Pay_Period_Id { get; set; } = 0;

        public string Employee_Id { get; set; } = "";

        public string QZoneUserName { get; set; } = "";
    }

    public class UanErrorMessage
    {
        public string Error_Message { get; set; } = "";

    }
    public class VanDetailsView
    {
        public string QZoneUserName { get; set; }
        public string Pay_Period { get; set; }
        public List<CompanycodeList> CompanyCodelist { get; set; }

    }

    public class CompanycodeList
    {
        public string CompanyCode { get; set; }

    }

    public class VanCompanyCode
    {

        public string Company_Id { get; set; } = "";

        public string Company_Code { get; set; } = "";
    }

    public class VanPayPeriod
    {

        public List<VanCompanyCode> requestdata { get; set; }
    }

    public class VanRequest
    {
        public string QZoneUserName { get; set; } = "";

        public List<VanPaymentRelese> VanRequestList { get; set; }
    }

    public class VanPaymentRelese
    {

        public string Company_Code { get; set; }

        public string Harbour_id { get; set; }

        public string Employee_Code { get; set; }

        public string CN_Code { get; set; }

        public string PayPeriod { get; set; }

        public string Status { get; set; }
    }


}
