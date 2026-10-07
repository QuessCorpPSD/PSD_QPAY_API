using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models.MailApprovalProcess
{
    public class ClientBlock
    {
        public string Id { get; set; }
        public string Company_code { get; set; }
        public string Remarks { get; set; }
    }
   

    public class ClientApprove
    {
        public string UserId { get; set; }
        public int IsApproved { get; set; }
        public List<ClientBlock> ApproveList { get; set; }
    }
    public class ErrorMessage
    {
        public string Message { get; set; }
    }
    public class SourceBlock
    {
        public string EMPLOYEE_CODE { get; set; }
        public string Remarks { get; set; }
    }
    public class SourcingApprove
    {
        public int ACTION { get; set; }
        public int IsApproved { get; set; }
        public string UserId { get; set; }
        public int PAY_PERIOD_ID { get; set; }
        public int LOT_NUMBER { get; set; }
        public List<SourceBlock> ApproveList { get; set; }
    }
}
