using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QPay.UI.Models.Invoice
{
    public class DraftNew
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
            public string? Map_Name_Id { get; set; }
            public string? Invoice_Category_Id { get; set; }
        }

        public class PushData
        {
            public int? CompanyId { get; set; }
            public int? PayPeriodId { get; set; }
            public string? LotNumbers { get; set; }
            public string? Input_No { get; set; }
            public string? Employee_Head_Count { get; set; }
            public int? Map_Name_Id { get; set; }
            public string? Map_Name { get; set; }
            public decimal? NetPay { get; set; }
            public int? Invoice_Category_Id { get; set; }
            public string? Invoice_Category { get; set; }
            public int? InvoiceType_Id { get; set; }
            public string? Service_Charge_Master { get; set; }

            public string? InvoiceCulture_id { get; set; } = "";
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
        public class InvoiceResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }



        public class LotMergeRequest
        {
            public List<MergeRequest> mergeRequests { get; set; }
            public string CreatedBy { get; set; } = string.Empty;
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
            public string ActionType { get; set; } = string.Empty;
            public string InvoiceDateType { get; set; } = string.Empty;

            public string RemarksText { get; set; } = string.Empty;

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
            public int? InvoiceType_Id { get; set; }
            [XmlIgnore]
            public string CreatedBy { get; set; } = string.Empty;
            public string Service_Charge_Master { get; set; } = string.Empty;
            [XmlIgnore]
            public string Section_billing { get; set; } = string.Empty;
            [XmlIgnore]
            public Int16? DraftType { get; set; }

        }

        public class DraftInvoice
        {
            public int DraftType { get; set; }
            public List<InvoiceInitiateRequest> InvoiceInitiateRequests { get; set; }
        }

        public class InvoiceInitiateAgainstProfomaRequest
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public List<InvoiceInitiateAgainstProfoma> invoiceInitiateAgainstProfomas { get; set; }

        }

        public class InvoiceInitiateAgainstProfomaInitiation
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public string CreatedBy { get; set; } = string.Empty;
            public List<PerformaToActualUI> invoiceInitiateAgainstProfomas { get; set; }

        }



        public class InvoiceInitiateAgainstProfoma
        {
            public string CompanyId { get; set; } = string.Empty;
            public string PayPeriodId { get; set; } = string.Empty;
            public int LotNumber { get; set; }

            public string StateId { get; set; } = string.Empty;
            public string GroupDetailId { get; set; } = string.Empty;
            public string InputNumber { get; set; } = string.Empty;
            public string EmployeeCount { get; set; } = string.Empty;
            public string NetPay { get; set; } = string.Empty;
            public string CTC { get; set; } = string.Empty;
            public string CostCenterMappingId { get; set; } = string.Empty;
            public string ServiceChargeAmount { get; set; } = string.Empty;
        }

        public class PerformaToActualUI
        {
            public int RowNumber { get; set; }
            public int Company_Id { get; set; }
            public int Pay_Period_Id { get; set; }
            public int Map_Name_Id { get; set; }
            public int Invoice_Map_Name_Id { get; set; }
            public int PT_State_Id { get; set; }
            public int Other_Income_Id { get; set; }
            public int LotNo { get; set; }
            public int InputNumber { get; set; }
            public string MapName { get; set; } = string.Empty;
            public string Company_Code { get; set; } = string.Empty;

            public string PayPeriod { get; set; } = string.Empty;
            public int EmployeeCount { get; set; }
            public string Service_Charge_Master { get; set; } = string.Empty;
            public string Service_Charge_Type { get; set; } = string.Empty;

            public int InvoiceType_Id { get; set; }
            public int? Invoice_Id { get; set; }
            public int? GroupDetailId { get; set; }
            public string Group_Name { get; set; } = string.Empty;
            public decimal Net_CTC { get; set; }

            public decimal NetPay { get; set; }

            public int InvoiceCulture_id { get; set; }
            public string InvoiceCul_Ref_No { get; set; } = string.Empty;
            public int Invoice_Category_Id { get; set; }
            public decimal ServiceChargeAmount { get; set; }
            public string Invoice_Category { get; set; } = string.Empty;
            public string StateName { get; set; } = string.Empty;
            public bool IsInitiation { get; set; }
            public bool IsActive { get; set; }
            public string InvoiceNumber { get; set; } = string.Empty;
            public string Data_from { get; set; } = string.Empty;
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
    }
}

