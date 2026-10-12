using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.ARKnockOff
{
    public class ARKnockOffclass
    {
        public string? Batch_Id { get; set; } = "";
        public List<ARPaymentDetail> ARPaymentDetails { get; set; } = new List<ARPaymentDetail>();

    }

    public class ARPaymentDetail
    {
        public string? UTR_No { get; set; } = "";
        public string? Remittance_Date { get; set; } = "";
        public string? Remitter { get; set; } = "";
        public string? Benificiary { get; set; } = "";
        public decimal Total_Remitted_Amount { get; set; }
        public string? Sender_Mail { get; set; } = "";
        public string? Mail_Date { get; set; } = "";
        public string? Mail_Subject { get; set; } = "";
        public string? Error_Message { get; set; } = "";
        public string? FileName { get; set; } = "";
        public List<ARInvoiceDetail> ARInvoiceDetails { get; set; } = new List<ARInvoiceDetail>();
    }
    public class ARInvoiceDetail
    {
        public string? Invoice_Number { get; set; } = "";
        public string? Amount { get; set; } = "";
        public string? TDS_Percent { get; set; } = "";
        public string? TDS_Amount { get; set; } = "";
    }
    public class IgnoreTextUI
    {
        public string? UTR_No_keywords { get; set; } = "";
        public string? Benificiary_keywords { get; set; } = "";
        public string? Total_Remitted_Amount_keywords { get; set; } = "";
        public string? Invoice_Number_keywords { get; set; } = "";
        public string? Amount_keywords { get; set; } = "";
        public string? TDS_Amount_keywords { get; set; } = "";
        public string? TDS_Percent_keywords { get; set; } = "";
        public int? id { get; set; }
        [XmlIgnore]
        public string? mode { get; set; }
        [XmlIgnore]
        public int userId { get; set; }
    }

}

