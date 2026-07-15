using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QPay.DTo.Models.PayrollInput
{
    public class Onboarding
    {
        public string? offerId { get; set; }
        public string? employeeName { get; set; }
        public string? fatherName { get; set; }
        public string? gender { get; set; }
        public string? doj { get; set; }
        public string? dob { get; set; }
        public string? designation { get; set; }
        public string? jobState { get; set; }
        public string? jobLocation { get; set; }
    }
    public class FileResponse
    {
        public string FileName { get; set; }
        public string File { get; set; }

    }
    public class NewJoineeModel
    {
        public int companyId { get; set; }
        public int payPeriodId { get; set; }
    }
    public class FinalSubmission
    {
        public string? InputName { get; set; }
        public string? HeadCount { get; set; }
        public string? CurrentStatus { get; set; }
        public string? InputLotNumber { get; set; }
        public string? Merged_Lot { get; set; }
        public string? FirstName { get; set; }
        public bool? IsProcessed { get; set; }
        public int? Revised { get; set; }
        public bool? IsSubmitted { get; set; }
        public bool? IsEmployeeID { get; set; }
        public string? Inputype { get; set; }
        public bool? Customer_Confirmation_Status { get; set; }

    }

    public class FileJson {
        public string? FileName { get; set; }
    }

    public class SEZJson
    {
        public string? FilePath { get; set; }
    }

    public class OfferIds 
    {
        public string[]? offerIds { get; set; }
    }

    public class RollbackofferIds
    {
        public string[]? offerIds { get; set; }
        public string? userId { get; set; }
    }

    public class MoveOffer
    {
        public string[]? offerIds { get; set; } 
        public int companyId { get; set; }
        public string payPeriod { get; set; } = "";
        public int payPeriodId { get; set; }
        public string userId { get; set; } = "";
    }

    public class FinalSubmitMerge
    {
        public int CompanyId { get; set; }
        public int PayPeriodId { get; set; }
        public string? MergedLots { get; set; } = "";
        public string? CreatedBy { get; set; } = "";
        public string? Remarks { get; set; } = "";
        public string? InputType { get; set; } = "";
    }
}

