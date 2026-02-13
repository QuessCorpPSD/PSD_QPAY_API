using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.UI.Models.Invoice;

namespace QPay.UI.Invoice
{
    public class InvoiceCancel
    {
        public string client_id { get; set; }
        public string client_hash { get; set; }
        public string gstin { get; set; }
        public string sup_gstin { get; set; }
        public string ip_addr { get; set; }
        public string environment { get; set; }
        public string single_encryption { get; set; }
        //public EInvoiceCancellationPayload()
        //{
        //    docs = new List<DocsCancellation>();
        //}
        //public List<DocsCancellation> docs { get; set; }
        public DocsCancellation docs { get; set; }

    }

    public class InvoiceCancelResult
    {
        public int Invoice_Id { get; set; }
        public string Invoice_No { get; set; }
        public string Irn { get; set; }
        public string Status { get; set; }         // SUCCESS / FAILED
        public string Error_Message { get; set; }
    }
    public class DocsCancellation
    {

        public string Irn { get; set; }

        public string Reason_Code { get; set; }
    }

    public class CreditnoteInvoiceList
    {
        public List<int> InvoiceIds { get; set; } = new();
    }
    public class InvoiceCancelResponse
    {
        public string Status { get; set; }  // SUCCESS / FAILED
        public string Message { get; set; }
        public string InvoiceId { get; set; }
        public CreditnoteInvoiceList CreditnoteInvoices { get; set; } = new();
        public List<InvoiceCancel> ApiPayloads { get; set; } = new();
        public List<InvoiceCancelResult> InvoiceResults { get; set; } = new();
        public List<(string InvoiceId, int PayloadIndex)> InvoiceMap { get; set; } = new();
        public List<DocsCancellation> CancellationDocs { get; set; } = new(); // ✅ IRNs list
    }

    public class InvoiceCancelGrid
    {
        public int? Invoice_Id { get; set; }
        public string Invoice_Number { get; set; } = "";
        public string Status { get; set; } = "";
        public int? Company_Id { get; set; }
        public string Company_Code { get; set; } = "";
        public int? Cost_Center_Mapping_Id { get; set; }
        public string Map_Name { get; set; } = "";
        public int? City_Id { get; set; }
        public string City_Name { get; set; } = "";
        public int? Financial_Year_Id { get; set; }
        public int? Pay_Period_Id { get; set; }
        public string Pay_Period { get; set; } = "";
        public int? Invoice_Type_Id { get; set; }
        public string InvoiceType { get; set; } = "";
        public string? Invoice_Date { get; set; }
        public int? StateId { get; set; }
        public string? State_Name { get; set; }
        public decimal? IGST_Percentage { get; set; }
        public decimal? IGST_Amount { get; set; }
        public decimal? CGST_Percentage { get; set; }
        public decimal? CGST_Amount { get; set; }
        public decimal? SGST_Percentage { get; set; }
        public decimal? SGST_Amount { get; set; }
        public decimal? UTGST_Percentage { get; set; }
        public decimal? UTGST_Amount { get; set; }
        public decimal? Net_Amount { get; set; }

        public string IRN_Status { get; set; } = "";
        public string? Irn_Number { get; set; }
        public string CreditNote_Status { get; set; } = "";
        public string? Crn_Number { get; set; }
        public string Crn_IRN_Status { get; set; } = "";
        public string Crn_IRN_Number { get; set; } = "";
        public string? CreditNoteNumber { get; set; }
        public string? CancelledOn { get; set; }
        public string? FilePath { get; set; }

    }

}

