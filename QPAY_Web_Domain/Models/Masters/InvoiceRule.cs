using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.DTo.Models.Masters
{
    public class InvoiceRule
    {
        public int? invoicingRulesID { get; set; }
        public int? companyId { get; set; }
        public string companyCode { get; set; } = "";
        public string siteName { get; set; } = "";
        public int? siteId { get; set; }
        public string daysPerMonth { get; set; } = "";
        public string weekends { get; set; } = "";
        public string holidays { get; set; } = "";
        public string compOff { get; set; } = "";
        public string maternity { get; set; } = "";
        public string leaveAddition { get; set; } = "";
        public string leaveRule { get; set; } = "";
        public string billableDaysFormula { get; set; } = "";
    }

    public class InvoiceRuleAdd
    {
        public int? CompanyId { get; set; }
        public string CompanyCode { get; set; } = "";
        public int? SiteId { get; set; }
        public string SiteName { get; set; } = "";
        public string BillingType { get; set; } = "";
        public bool DaysAsPerTimesheet { get; set; }
        public int? DaysPerMonth { get; set; }
        public string WeekendsRule { get; set; } = "";
        public string HolidaysRule { get; set; } = "";
        public string ComppoffRule { get; set; } = "";
        public string MaternityLeave { get; set; } = "";
        public int? LeaveTypes { get; set; }
        public decimal? LeaveCredit { get; set; }
        public string LeaveRule { get; set; } = "";
        public string PayPeriodFrom { get; set; } = "";
        public string PayPeriodTo { get; set; } = "";
        public string CarryForward { get; set; } = "";
        public int? NoOfCarryForwards { get; set; }
        public string OtRule { get; set; } = "";
        public string Gratuity { get; set; } = "";
        public string Reimbursement { get; set; } = "";
        public string ServiceFeeOnExpenses { get; set; } = "";
        public string Rebates { get; set; } = "";
        public string Discounts { get; set; } = "";
        public string BillableDaysFormula { get; set; } = "";
        public string UserId { get; set; } = "";
        public string payroll_weekends_billable { get; set; } = "";
    }
}
