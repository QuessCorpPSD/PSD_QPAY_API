using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.AccountReceivableMod
{
    public class ReIssueApproveUploadResponse
    {
        public string response { get; set; }
        public List<string> errors { get; set; } = new List<string>();
    }

    public class ReIssueApproveExportRequest
    {
        public int? CompanyId { get; set; }
        public int? PayPeriodId { get; set; }
        public string? vPayPeriods { get; set; }
        public int? ReissueTypeId { get; set; }
        public string? Status { get; set; }
    }

    public class ReIssueApproveRejectDetail
    {
        public int SNo { get; set; }
        public int Bank_Invoice_Id { get; set; }
        public int Company_Id { get; set; }
        public string? Company_Code { get; set; }
        public string? Company_Name { get; set; }
        public string? BatchId { get; set; }
        public int Pay_Period_Id { get; set; }
        public string? Pay_Period { get; set; }
        public int Employee_Id { get; set; }
        public string? Correct_Employee_Name { get; set; }
        public string? Employee_Code { get; set; }
        public string? Update_Bank_Name { get; set; }
        public string? Update_Bank_Acctno { get; set; }
        public string? Update_IFSC_Code { get; set; }
        public string? Cheque_Number { get; set; }
        public decimal Cheque_Amount { get; set; }
        public int PayMode_Id { get; set; }
        public string? Pay_Mode { get; set; }
        public int ReIssueType_Id { get; set; }
        public string? ReIssueType { get; set; }
        public string? Remarks { get; set; }
    }

    public class ReIssueApproveRejectRequest
    {
        public List<ReIssueApproveRejectDetail> Groupdetail { get; set; } = new List<ReIssueApproveRejectDetail>();
        public string Cheque_Status { get; set; } = string.Empty;
        public string Cancellation_Charges { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string mode { get; set; } = string.Empty;
        public int userId { get; set; }
    }

    public class ReIssueApproveRejectResponse
    {
        public string response { get; set; } = string.Empty;
        public int Bank_Invoice_Id { get; set; }
        public List<string> errors { get; set; } = new List<string>();
    }
}