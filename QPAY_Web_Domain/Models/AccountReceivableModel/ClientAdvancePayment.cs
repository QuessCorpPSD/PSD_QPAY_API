using System;
using System.Collections.Generic;

namespace QPay.UI_Domain.Models.AccountReceivable
{
    public class ClientAdvancePayment
    {
        public class ClientAdvancePaymentResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class ClientAdvancePaymentRequest
        {
            public int Created_By { get; set; }
            public string Mode { get; set; }
            public List<ClientAdvancePaymentDetails> clientadvancepayment { get; set; }
        }

        public class ClientAdvancePaymentDetails
        {
            public int Client_Advance_Payment_Id { get; set; }
            //public string Reference_Id { get; set; } 
            public int Company_Id { get; set; }
            public string UTRChequeNumber { get; set; }
            public DateTime? Cheque_Date { get; set; }
            public DateTime? Credit_Date { get; set; }
            public DateTime? Posting_Date { get; set; }
            public int Bank_Id { get; set; }
            public decimal Amount { get; set; }
            public string Remarks { get; set; }
            public int Client_Id { get; set; }
            public int OnAccountTypeValue { get; set; }
            public int ModeOfCollectionsValue { get; set; }
            public int OnAccountNumbersValue { get; set; }
            public int Group_Detail_Id { get; set; }
        }

        public class CommonExport
        {
            public string companyId { get; set; }
            public string fromDate { get; set; }
            public string toDate { get; set; }

        }

        public class CommonExport1
        {
            public int companyId { get; set; }    
            public string fromDate { get; set; }
            public string toDate { get; set; }
            public string allEntityId { get; set; }
        }


    }

   
}