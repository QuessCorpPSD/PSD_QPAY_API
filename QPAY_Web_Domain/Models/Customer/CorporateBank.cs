using QPay.UI.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Customer
{
    public class CorporateBank
    {
        public Int32 Bank_Id { get; set; }
        public string Bank_Name { get; set; }
        public string Ifsc_Code { get; set; }
        public string Account_No { get; set; }
        public string Address { get; set; }
        public string Error_Message { get; set; }
        public string Serial_No { get; set; }
        public string BranchName { get; set; }
        public string BrsCode { get; set; }
        public string BankCode { get; set; }

    }

    public class CorporateBankRequest
    {
        public int createdBy { get; set; }
        public string mode { get; set; }
        public CorporateBank parentDetail { get; set; }

    }
}
