using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class BonusFlashOut
    {
        public string InvoiceNumber { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class BonusReleaseRequest
    {
        public int QZoneUserName { get; set; }
        public List<BonusFlashOut> BonusReleaseList { get; set; }

    }

    public class BonusErrorMessage
    {
        public string Error_Message { get; set; } = "";

    }

    public class DeductionFlashOut
    {
        public string CompanyCode { get; set; }
        public string Employeecode { get; set; }
        public string PayPeriod { get; set; }
        public string PayCode { get; set; }
        public string InvoiceNumber { get; set; }
        public string Amount { get; set; }
        public IFormFile Attachment { get; set; }
    }

    public class DeductionReleaseRequest
    {
        public int QZoneUserName { get; set; }
        public List<DeductionFlashOut> DeductionReleaseList { get; set; }

    }

    public class DeductionErrorMessage
    {
        public string Error_Message { get; set; } = "";

    }



}
