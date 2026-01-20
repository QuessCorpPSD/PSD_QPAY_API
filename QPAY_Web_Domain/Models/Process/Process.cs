using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.Process
{
    public class Process
    {
        public class PayProcessRequest
        {
            public int company_Id { get; set; }
            public string End_At { get; set; } = "";
        }

        public class PayProcessPayperiodRequest
        {
            public int company_Id { get; set; }
            public string payperiodId { get; set; } = "";
        }
        public class Message
        {
            public string Error_Message { get; set; }
        }
        public class ProcessUI
        {
            public string Is_Processed { get; set; } = "";
            public string Actual_declared { get; set; } = "";
        }

        public class ProcessUIDate
        {
            public string Date { get; set; } = "";
        }

        public class PayFrequency
        {
            public int? Pay_Frequency_Id { get; set; }
            public int? Pay_Frequency_Detail_Id { get; set; }
            public string Pay_Period { get; set; }
            public string Pay_Sequence_Number { get; set; }
            public int? Pay_Period_Days { get; set; }
            public int? Working_Days { get; set; }
            public string Start_At { get; set; }
            public string Actual_declared { get; set; }
            public int? Is_Processed { get; set; }
            public int Is_Reprocessed { get; set; }
            public int Is_FProcessed { get; set; }
            public int Is_FReprocessed { get; set; }
            public string End_At { get; set; }
            public decimal? Month_Days { get; set; }
        }

        public class ReprocessRequest
        {
            public string Company_Id { get; set; } = "";
            public string Pay_Period_Id { get; set; } = "";
            public string Declaration_type { get; set; } = "";
            public string CreatedBy { get; set; } = "";
        }

        public class PayProcessResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class ProcessResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class SearchLOPRequest
        {
            public string Company_id { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
        }

        public class SearchPayRegisterRequest
        {
            public string Company_id { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
        }

        public class SearchItRequest
        {
            public string Company_id { get; set; } = "";
            public string Employee_id { get; set; } = "";
        }

        public class SearchAllowReprocessRequest
        {
            public string Company_id { get; set; } = "";
        }

        public class AllowReprocessCreateRequest
        {
            public string Mode { get; set; } = "";
            public string CreatedBy { get; set; } = "";
            public List<AllowReprocess> allowReprocesses { get; set; } = new List<AllowReprocess>();
        }

        public class AllowReprocess
        {
            public string Allow_Reprocess_Id { get; set; } = "";
            public string Pay_Frequency_Detail_Id { get; set; } = "";
            public string Serial_No { get; set; } = "";
            public string Company_Code { get; set; } = "";
            public string Client_Name { get; set; } = "";
            public string Pay_Sequence_Number { get; set; } = "";
            public string Pay_Period { get; set; } = "";
            public string Reprocess_Flag { get; set; } = "";
            public string Company_Id { get; set; } = "";
            public string Error_Message { get; set; } = "";
        }

        public class SearchLockPayperiodRequest
        {
            public string PayPeriod { get; set; } = "";
        }

        public class LockPayperiodRequest
        {
            public string Company_Id { get; set; } = "";
            public string Pay_Frequency_Detail_Id { get; set; } = "";
            public string CreatedBy { get; set; } = "";
        }

        public class SearchOnetimeReplacementRequest
        {
            public string Company_id { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
            public string Employee_Code { get; set; } = "";
        }

        public class SearchOIRequest
        {
            public string Company_id { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
        }

        public class SearchEmployeeRequest
        {
            public string CompanyId { get; set; } = "";
            public string EmployeeId { get; set; } = "";
        }

        public class SearchPayTransactionRequest
        {
            public string CompanyId { get; set; } = "";
            public string EmployeeId { get; set; } = "";
            public string Pay_Frequency_Id { get; set; } = "";
            public string Paycode_Id { get; set; } = "";
        }

        public class SearchReimbursementRequest
        {
            public string CompanyId { get; set; } = "";
        }
    }
}
