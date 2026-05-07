using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Invoice
{
    public class InvoiceBackDatedUI
    {
        public int StatusCode { get; set; }
        public string BackDated { get; set; } = "";
        public string MonthDate { get; set; } = "";
    }

    public class SplitParams
    {
        public int? company_id { get; set; }
        public int? Pay_Period_Id { get; set; }
        public string? LotNo { get; set; }
        public string? InputNo { get; set; }
        public string? Map_Name_Id { get; set; }
        public string? Invoice_Category_Id { get; set; }
    }

    public class PushData
    {
        public int? CompanyId {get; set;}
        public int? PayPeriodId {get; set;}
        public string? LotNumbers {get; set;}
        public string? Input_No {get; set;}
        public string? Employee_Head_Count {get; set;}
        public int? Map_Name_Id {get; set;}
        public string? Map_Name {get; set;}
        public decimal? NetPay {get; set;}
        public int? Invoice_Category_Id {get; set;}
        public string? Invoice_Category {get; set;}
        public int? InvoiceType_Id {get; set;}
        public string? Service_Charge_Master { get; set; }
        public int? InvoiceCulture_id { get; set; }

    }


    [XmlRoot("Main")]
    public class PushModel
    {
        [XmlElement("InvoiceInitiateRequest")]
        public List<PushData> details { get; set; }
        [XmlIgnore]
        public int company_id { get; set; }
        [XmlIgnore]
        public int Pay_Period_Id { get; set; }
        [XmlIgnore]
        public string Action { get; set; }
        [XmlIgnore]
        public string? CreatedBy { get; set; }
        [XmlIgnore]
        public int DraftTypeId { get; set; }

    }

    public class EmpExport
    {
        public int? Company_Id { get; set; }
        public int? pay_period_id { get; set; }
        public string? LotNo { get; set; }
        public int? Map_Name_Id { get; set; }
        public Int64? Input_No { get; set; }
        public string? Invoice_Type { get; set; } = "";
    }

    public class InvoiceCountRequest
    {
        public int? CompanyId { get; set; }
        public int? PayPeriodId { get; set; }
        public int? MapNameId { get; set; }
        public string? Input_No { get; set; }
        public string? LotNo { get; set; }
        public string Invoice_Category { get; set; }
    }

}
