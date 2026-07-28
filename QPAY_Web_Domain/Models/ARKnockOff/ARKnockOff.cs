using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<ARInvoiceDetail> ARInvoiceDetails { get; set; } = new List<ARInvoiceDetail>();

    }
    public class ARInvoiceDetail
    {
        public string? Invoice_Number { get; set; } = "";
        public string? Amount { get; set; } = "";
    }
}

