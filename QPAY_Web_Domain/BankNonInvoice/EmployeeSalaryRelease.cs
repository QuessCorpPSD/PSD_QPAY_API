using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.UI.BankNonInvoice
{
    public class EmployeeSalaryRelease
    {
        public class CommonExport
        {
            public string? companyId { get; set; }
            public string? payPeriodId { get; set; }
        }
        public class CommonExports
        {
            public int Company_Id { get; set; }
            public int Pay_Period_Id { get; set; }

            public int? Employee_Id { get; set; }

        }
        public class EmployeeSalaryReleaseModel
        {
            public int Company_Id { get; set; }
            public int PayPeriod_Id { get; set; }
            public string? Company_Code { get; set; }
            public string? Group_Name { get; set; }
            public string? Vendor_Name { get; set; }
            public string? Pay_Period { get; set; }
        }

        public class EmployeeSalaryReleaseResponse
        {
            public string? response { get; set; }
            public List<string>? errors { get; set; }
        }
        public class BulkUploadErrormessage
        {
            public string? Vaildation { get; set; }

            public List<string>? errors { get; set; }

        }

        public class HoldEmpSalaryExportRequest
        {
            public int CompanyId { get; set; }

            public int PayPeriodId { get; set; }

            public string? Status { get; set; }
        }

        public class HoldEmpSalaryResponse
        {
            public string? response { get; set; }

            public List<string>? errors { get; set; }
        }
        public class ReleaseHoldSalaryResponse
        {
            public string response { get; set; } = string.Empty;
            public List<string> errors { get; set; } = new List<string>();
        }

        public class ReleaseHoldSalaryRequest
        {
            public int CreatedBy { get; set; }
            public string Action { get; set; }   // Insert / Update / Delete
            public string? User { get; set; }
            public List<ReleaseHoldSalaryDetails> Data { get; set; }
        }

        public class ReleaseHoldSalaryDetails
        {
            public string Company_Code { get; set; }
            public string Employee_Code { get; set; }
            public string Pay_Period { get; set; }
            public string PURPOSE { get; set; }
            public int BatchID { get; set; }
            public int INPUT_NO { get; set; }
        }
        public class BankCulturedata
        {
            public int Bank_Id { get; set; }
            public int Bank_Culture_id { get; set; } = 0;
            // public string Bank_Name { get; set; } = "";
        }

        public class BankCulturesave
        {
            public string Mode { get; set; }
            public int UserId { get; set; }
            public int Company_Id { get; set; }
            public List<BankCulturedata> culturedatas { get; set; }
        }
        public class BankNeftCulture
        {
            public string Bank_Id { get; set; }
            public string Bank_Name { get; set; }
            public string Bank_Culture_id { get; set; }
            public string available { get; set; }
        }

        public class BankCultureMessage
        {
            public string Error_Message { get; set; } = "";
        }

        public class Bankadvisesplitculture
        {
            public int Company_Id { get; set; }
            public int vendor_id { get; set; }
           // public string? groupdetail { get; set; }
            public int? culture_type { get; set; }
            public int? created_by { get; set; }
            public string? mode { get; set; }
            public int? Bank_Culture_id { get; set; }

            //public int? Bank_Culture_id { get; set; }
            public List<GroupDetail> groupdetail { get; set; }
           
        }

        public class GroupDetail
         {
            public int Group_Detail_Id { get; set; }
            //public string? Group_Name { get; set; }
        }
        public class searcheditdata
        {
            public int Company_Id { get; set; }
            public int vendor_id { get; set; }
            public int? bankcultureid { get; set; }
         
            public string? mode { get; set; }
        }
    }
}
