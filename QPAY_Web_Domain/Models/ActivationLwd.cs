using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI_Domain.Models
{
    public class ActivationLwd
    {
        public class Activation
        {
            public string COMPANY_CODE { get; set; } = string.Empty;
            public string EMPLOYEE_CODE { get; set; } = string.Empty;
            public string EMPLOYEE_NAME { get; set; } = string.Empty;
            public string DOJ { get; set; } = string.Empty;
            public string LAST_WORKING_DAY { get; set; } = string.Empty;
            public string REASON_OF_LEAVING { get; set; } = string.Empty;
            public string REMARKS { get; set; } = string.Empty;
            public string RELIEVING_LETTER { get; set; } = string.Empty;
        }

        public class Lwd
        {
            public string COMPANY_CODE { get; set; } = string.Empty;
            public string EMPLOYEE_CODE { get; set; } = string.Empty;
            public string EMPLOYEE_NAME { get; set; } = string.Empty;
            public string DOJ { get; set; } = string.Empty;
            public string LAST_WORKING_DAY { get; set; } = string.Empty;
            public string REASON_OF_LEAVING { get; set; } = string.Empty;
            public string RELIEVING_LETTER_YES_NO { get; set; } = string.Empty;
        }

        public class ActivationUpload
        {
            public string User { get; set; } = string.Empty;
            public string COMPANY_CODE { get; set; } = string.Empty;
            public string FLAG { get; set; } = string.Empty;
            public List<ActivationEmployeelist> activationemployeelist { get; set; }

        }

        public class ActivationEmployeelist
        {
            public string EMPLOYEE_CODE { get; set; } = string.Empty;
            public string REMARKS { get; set; } = string.Empty;
        }

        public class LWDEmployeelist
        {
            public string Employee_Code { get; set; } = string.Empty;
            public string First_Name { get; set; } = string.Empty;
            public string DoJ { get; set; } = string.Empty;
            public string Last_Working_Day { get; set; } = string.Empty;
            public string Reason_Of_Leaving { get; set; } = string.Empty;
            public string RELIEVING_LETTER_YES_NO { get; set; } = string.Empty;
        }

        public class ActivationResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }
    }
}
