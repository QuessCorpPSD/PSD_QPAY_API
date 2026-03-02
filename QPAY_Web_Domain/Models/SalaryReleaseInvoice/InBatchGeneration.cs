using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class InBatchGeneration
    {
        public long BankAdviceApprovalsId { get; set; }
        public string Invoice_No { get; set; }
        public string CompanyCode { get; set; }
        public string PayPeriod { get; set; }
        public string MapName { get; set; } = string.Empty;
        public string NetAmount { get; set; }
        public int NoOfEmployees { get; set; } = 0;
        public int BatchCreationTypeId { get; set; }
    }

    public class BatchCreate
    {
        public int Entity_id  { get; set; }
        public string BatchType { get; set; }
        public int UserId { get; set; }
        public List<InBatchGeneration> BatchList { get; set; }
    }

    public class RejectBankAdviceRequest
    {
        
        public long Bank_Advice_Approvals_Id { get; set; }
        public string Invoice_No { get; set; }
        public string Remarks { get; set; }
       
    }

    public class RejectBankAdvice
    {
        public string BatchType { get; set; }       
        public int UserId { get; set; }
        public List<RejectBankAdviceRequest> RejectList { get; set; }
    }

    public class BatchList
    {
        public string BatchId { get; set; }
       
    }

    public class IntitiateBatch
    {
        public string BatchType { get; set; }
        public string BatchId { get; set; }
        public int UserId { get; set; }

    }


    public class  EntityMaster
    {
        public int Entity_Id { get; set; }
        public string Entity_Name { get; set; }
        
    }

    public class CommonGenModel
    {
        public int GEN_iID { get; set; }
        public string GEN_vDescription { get; set; }
    }

    public class BulkUploadErrorMessage
    {
       // public int Status { get; set; }
        public string Validation { get; set; }
    }

    public class SatausErrorMessage
    {
        
        public string Error_Message { get; set; }
    }

    public class CommonDropDown
    {
        public string value { get; set; }
        public string name { get; set; }
    }
}
