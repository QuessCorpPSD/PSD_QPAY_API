using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{
    public class UserLotValidationUI
    {
        public string Name { get; set; } = string.Empty;
        public string Mail_Id { get; set; } = string.Empty;
        public string Process_Category { get; set; } = string.Empty;
        public int? TeamLead_User_Id { get; set; }
        public int? Manager_User_Id { get; set; }
        public int? Fun_Head_UserId { get; set; }        
        public string TeamLead_Email_Id { get; set; } = string.Empty;
        public string Manager_Email_Id { get; set; } = string.Empty;
        public string Fun_Head_EmailId { get; set; } = string.Empty;
        public string MailType { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
        public string subjects { get; set; } = string.Empty;

        public float? Score { get; set; } 


    }
}
