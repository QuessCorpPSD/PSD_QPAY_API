
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.Models
{

    public class LotValidationRequest
    {
        public int? company_Id { get; set; }
        public int? payperiodId { get; set; }
        public int? lotnumber { get; set; }
        public string Payroll_Input_Type { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public int? userId { get; set; }
        public string? ActionType { get; set; }=string.Empty;
    }
    public class LotValidationResponse
    {
        public int? StatusCode { get; set; }

        public string? Messages { get; set; }
    }
        public class UserEstimateLotValidationUI
    {
        public string Name { get; set; } = string.Empty;
        public string Mail_Id { get; set; } = string.Empty;
        public string Process_Category { get; set; } = string.Empty;
        public int? TeamLead_User_Id { get; set; } 
        public string TeamLead_Email_Id { get; set; } = string.Empty;
        public int? Manager_User_Id { get; set; } 
        public string Manager_Email_Id { get; set; } = string.Empty;
        public int? Fun_Head_UserId { get; set; } 
        public string Fun_Head_EmailId { get; set; } = string.Empty;
        public int? RemainingMinutes { get; set; }

        public string ActionType { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public int? ActionTime { get; set; }
     

    }
}
