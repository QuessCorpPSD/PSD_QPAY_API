using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Common
{
    public  class PayperiodDD
    {
        public int Payfrequencyid { get; set; }
        public string PaySequenceNo { get; set; } = string.Empty;
        public string PayPeriod { get; set; } = string.Empty;
    }
    public class CompanyPicker
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string DisplayName => string.Format("{0} ({1})", CompanyCode, CompanyName);
    }
    public class MapnameDD
    {
        public int mapNameId { get; set; }
        public string mapName { get; set; } = string.Empty;
    }
    public class InputTypeDD
    {
        public int inputId { get; set; }
        public string inputType { get; set; } = string.Empty;
    }

    public class PSDStatus
    {
        public bool QC_Verified_Status { get; set; }
        public bool Report_Status { get; set; }
        public bool Customer_Confirmation_Status { get; set; }
        public bool Invoice_Status { get; set; }

    }

    public class Site
    {
        public string SiteCode { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string STATUS { get; set; } = string.Empty;
    }

    public class City
    {
        public string City_Name { get; set; } = string.Empty;
        public string City_Id { get; set; } = string.Empty;

    }

    public class AllPayperiod
    {
        public string Pay_Frequency_Detail_Id { get; set; } = "";
        public string Pay_Period { get; set; } = "";

    }

    public class Paycodes
    {
        public int Paycode_Id { get; set; }
        public string Paycode_Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
