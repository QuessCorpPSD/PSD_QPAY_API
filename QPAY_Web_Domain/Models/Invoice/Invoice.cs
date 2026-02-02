using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Invoice
{
    public class Invoice
    {
        public class InvoiceResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }



        public class LotMergeRequest
        {
            public  List<MergeRequest> mergeRequests { get; set; }
            public string CreatedBy {  get; set; }= string.Empty;
            public string ActionType { get; set; } = "";
        }

        public class MergeNewRequest
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string MAP_NAME_ID { get; set; } = string.Empty;
            public string MergeLot { get; set; } = string.Empty;
            public string Merged_Input_No { get; set; } = string.Empty;
            public string Invoice_Category_Id { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public string Remarks { get; set; } = string.Empty;
            public string Data_From { get; set; } = string.Empty;

        }
        public class MergeRequest
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string MAP_NAME_ID { get; set; } = string.Empty;
            public string MergeLot { get; set; } = string.Empty;
            public string Merged_Input_No { get; set; } = string.Empty;
            public string InvoiceCategory { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public string Remarks { get; set; } = string.Empty;
            public string Data_From { get; set; } = string.Empty;

        }

        public class DraftInvoiceInitiate
        {
            public List<InvoiceInitiateRequest> DraftInvoiceInitiateRequest { get; set; } = new List<InvoiceInitiateRequest>();
            public string CreatedBy { get; set; } = string.Empty;
            public string ActionType { get; set; }= string.Empty;
        }

        public class InvoiceInitiateRequest
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string LotNumbers { get; set; } = string.Empty;
            public string Input_No { get; set; } = string.Empty;
            public string Employee_Head_Count { get; set; } = string.Empty;
            public string Map_Name_Id { get; set; } = string.Empty;
            public string Map_Name { get; set; } = string.Empty;
            public string NetPay { get; set; } = string.Empty;
            public string Invoice_Category_Id { get; set; } = string.Empty;
            public string Invoice_Category { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
        }

        public class InvoiceInitiateAgainstProfomaRequest
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public List<InvoiceInitiateAgainstProfoma> invoiceInitiateAgainstProfomas { get; set; }

        }

        public class InvoiceInitiateAgainstProfoma
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string CostCenterMappingId { get; set; } = string.Empty;
            public string StateId { get; set; } = string.Empty;
            public string GroupDetailId { get; set; } = string.Empty;
            public string InputNumber { get; set; } = string.Empty;
            public string EmployeeCount { get; set; } = string.Empty;
            public string NetPay { get; set; } = string.Empty;
            public string CTC { get; set; } = string.Empty;
            public string ServiceChargeAmount { get; set; } = string.Empty;
        }

        public class ProvisionalInvoiceInitiateRequest
        {
            public string CompanyId { get; set; } = string.Empty;
            public string CompanyCode { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string PayPeriod { get; set; } = string.Empty;
            public string LotNo { get; set; } = string.Empty;
            public string Input_No { get; set; } = string.Empty;
            public string Amount { get; set; } = string.Empty;
            public string ServiceCharge { get; set; } = string.Empty;
            public string State_Name { get; set; } = string.Empty;
            public string State_Id { get; set; } = string.Empty;
            public string Map_Name_Id { get; set; } = string.Empty;
            public string Map_Name { get; set; } = string.Empty;
            public string Invoice_Category_Id { get; set; } = string.Empty;
            public string Invoice_Category { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public string Isactive { get; set; } = string.Empty;
            public string Employee_code { get; set; } = string.Empty;

        }

        public class InvoiceResult
        {
            public string InvoiceId { get; set; }
            public List<object> Data { get; set; }
        }

        public class GstInvoiceCreateResponse
        {
            public string response { get; set; }
            public string? InvoiceId { get; set; }
            public List<object> Data { get; set; }
            public string? errors { get; set; }
        }

    }
}
