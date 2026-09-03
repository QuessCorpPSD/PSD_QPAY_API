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

        public class BillingDashboard
        {
            public string? Status { get; set; } = "";
            public int? Req_No { get; set; }
            public DateTime? RequestDatetime { get; set; }

            public string? Company_Code { get; set; }

            public string? Company_Name { get; set; }

            public string? LotNo { get; set; }
            public int? HC { get; set; }

            public string? ReqUserName {get;set;}
            public string? AssignedTo { get;set;}
            public string? InvoiceType { get; set; }

            public bool isedit { get; set; }

            public DateTime? AllocationDatetime {  get; set; }  
            public DateTime? Invoice_Created_Date {  get; set; }

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
            public string InvoiceCulture_Id { get; set; } = string.Empty;
            public string Po_Number { get; set; } = string.Empty;

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
    public class CancelRequest
    {
        public string Pay_Period { get; set; } = string.Empty;
        public int Company_Id { get; set; }
        public int PayPeriod_Id { get; set; }
    }
    public class InvoiceCancelApprovalRequest
    {
        public List<int> invoice_Id { get; set; }
        public int? CompanyId { get; set; }
        public int? PayPeriodId { get; set; }
        public string? userId { get; set; }
        public string? remarks { get; set; }
    }

    public class AttributeUI
    {
        public int? id { get; set; }
        public string? AttributeName { get; set; } = string.Empty;
        public string? ActionType { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }

        public string? createdOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }

        public string? Messages { get; set; }

    }

    public class SelectedItems
    {
        public string value { get; set; } = "";
        public string text { get; set; } = "";
    }

    public class InvoiceResponse
    {
        public string response { get; set; } = string.Empty;
        public List<string> errors { get; set; } = new List<string>();
    }
}
