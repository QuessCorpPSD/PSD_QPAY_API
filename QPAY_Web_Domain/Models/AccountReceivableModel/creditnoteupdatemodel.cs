using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.AccountReceivableMod
{
    public class creditnoteupdatemodel
    {
        public class CreditNoteExport
        {
            public string companyId { get; set; }
            public string fromDate { get; set; }
            public string toDate { get; set; }
        }

        public class CreditNoteUploadResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class BulkDownloadRequest
        {
            public string PdfType { get; set; }
            public int UserId { get; set; }
            public List<BulkItem> Items { get; set; }
        }

        public class BulkItem
        {
            public int CreditNoteId { get; set; }
            public int CompanyId { get; set; }
            public string InvoiceNumber { get; set; }
            public int InvoiceId { get; set; }
        }

        public class CreditNoteEditRequest
        {
            public int Created_By { get; set; }
            public string Mode { get; set; }

            public CreditNoteHeader CreditNote { get; set; }
            public List<CreditNoteEmployee> CreditNoteDetails { get; set; }
        }

        public class CreditNoteHeader
        {
            public string CreditNote_No { get; set; }
            public string Credit_Note_Type_Text { get; set; }
            public string Invoice_Number { get; set; }
            public string Sap_Reference_Number { get; set; }
            public string Credit_Note_Status { get; set; }
        }

        public class CreditNoteEmployee
        {
            public int CreditNote_Id { get; set; }
            public string Employee_Code { get; set; }
            public string Ref_Id { get; set; }
            public decimal Credit_Note_Amount { get; set; }
            public DateTime? Credit_Note_Dates { get; set; }
        }
    }
}
