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
}
