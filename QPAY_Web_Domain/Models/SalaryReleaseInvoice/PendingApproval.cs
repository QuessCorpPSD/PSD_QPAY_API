using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.SalaryReleaseInvoice
{
    public class PendingApproval
    {
        public string InvoiceNumber { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
    public class ApproveBankAdvice
    {
        public string BatchType { get; set; }
        public string CollectionStatus { get; set; }
        public int UserId { get; set; }      
      
        public List<PendingApproval> approvedata { get; set; }

    }

    public class BankadviceApprovalMessage
    {
        public string Validation { get; set; } = "";

    }
}
