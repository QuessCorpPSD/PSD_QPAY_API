using QPay.UI.Common;
using QPay.UI.Models.GlobalMaster;
using QPay.UI.Models.TaxAndSaving;
using QPay.UI.Reimbursements;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Common.StandingDataEnum;

namespace QPay.DTo.Models.Masters
{
    public class InvoiceRule
    {
        public string invoicingRulesID { get; set; } = string.Empty;
        public string companyId { get; set; } = string.Empty;
        public string companyCode { get; set; } = string.Empty;
        public string siteName { get; set; } = string.Empty;
        public string siteId { get; set; } = string.Empty;
        public string billingType { get; set; } = string.Empty;
        public string asPerTimesheet { get; set; } = string.Empty;
        public string asPerTimesheetText { get; set; } = string.Empty;
        public string daysPerMonth { get; set; } = string.Empty;
        public string weekends { get; set; } = string.Empty;
        public string holidays { get; set; } = string.Empty;
        public string compOff { get; set; } = string.Empty;
        public string maternity { get; set; } = string.Empty;
        public string leaveAddition { get; set; } = string.Empty;
        public string discounts { get; set; } = string.Empty;
        public string rebates { get; set; } = string.Empty;
        public string serviceFee { get; set; } = string.Empty;
        public string reimbursement { get; set; } = string.Empty;
        public string gratuity { get; set; } = string.Empty;
        public string ot { get; set; } = string.Empty;
        public string leaveRule { get; set; } = string.Empty;
        public string billableDaysFormula { get; set; } = string.Empty;
        public string leavetypes { get; set; } = string.Empty;
        public string leavetypesText { get; set; } = string.Empty;
        public string leavePeriod { get; set; } = string.Empty;
        public string carryforward { get; set; } = string.Empty;
        public string noofCarryfarward { get; set; } = string.Empty;
        public string typE_OF_BILLING_ID { get; set; } = string.Empty;
        public string typE_OF_BILLING_NAME { get; set; } = string.Empty;
        public string payroll_weekends_billable { get; set; } = string.Empty;

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
        public string type_of_billing { get; set; } = "";
        public string type_of_billing_name { get; set; } = "";
    }

    public class InvoiceRuleUpdate
    {
        public int? invoicingRulesID { get; set; }
        public int? companyId { get; set; }
        public string companyCode { get; set; } = "";
        public int? siteId { get; set; }
        public string siteName { get; set; } = "";
        public string billingtype { get; set; } = "";
        public bool daysAsPerTimesheet { get; set; }
        public int? dayspermonth { get; set; }
        public string weekendsrule { get; set; } = "";
        public string holidaysrule { get; set; } = "";
        public string comppoffrule { get; set; } = "";
        public string maternityleave { get; set; } = "";
        public int? leavetypes { get; set; }
        public decimal? leavecredit { get; set; }
        public string leaverule { get; set; } = "";
        public string payperiodfrom { get; set; } = "";
        public string payperiodto { get; set; } = "";
        public string carryforward { get; set; } = "";
        public int? noofcarryforwards { get; set; }
        public string otrule { get; set; } = "";
        public string gratuity { get; set; } = "";
        public string reimbursement { get; set; } = "";
        public string servicefeeonexpenses { get; set; } = "";
        public string rebates { get; set; } = "";
        public string discounts { get; set; } = "";
        public string billabledaysformula { get; set; } = "";
        public string userId { get; set; } = "";
        public string payroll_weekends_billable { get; set; } = "";
        public string type_of_billing { get; set; } = "";
        public string type_of_billing_name { get; set; } = "";
    }

    public class InvoiceRuleTemplate
    {
        public string invoicingRulesID { get; set; } = string.Empty;
        public string companyId { get; set; } = string.Empty;
        public string companyCode { get; set; } = string.Empty;
        public string siteName { get; set; } = string.Empty;
        public string siteId { get; set; } = string.Empty;
        public string billingType { get; set; } = string.Empty;
        public string asPerTimesheet { get; set; } = string.Empty;
        public string asPerTimesheetText { get; set; } = string.Empty;
        public string daysPerMonth { get; set; } = string.Empty;
        public string weekends { get; set; } = string.Empty;
        public string holidays { get; set; } = string.Empty;
        public string compOff { get; set; } = string.Empty;
        public string maternity { get; set; } = string.Empty;
        public string leaveAddition { get; set; } = string.Empty;
        public string discounts { get; set; } = string.Empty;
        public string rebates { get; set; } = string.Empty;
        public string serviceFee { get; set; } = string.Empty;
        public string reimbursement { get; set; } = string.Empty;
        public string gratuity { get; set; } = string.Empty;
        public string ot { get; set; } = string.Empty;
        public string leaveRule { get; set; } = string.Empty;
        public string billableDaysFormula { get; set; } = string.Empty;
        public string leavetypes { get; set; } = string.Empty;
        public string leavetypesText { get; set; } = string.Empty;
        public string leavePeriod { get; set; } = string.Empty;
        public string carryforward { get; set; } = string.Empty;
        public string noofCarryfarward { get; set; } = string.Empty;
        public string typE_OF_BILLING_ID { get; set; } = string.Empty;
        public string typE_OF_BILLING_NAME { get; set; } = string.Empty;
        public string payroll_weekends_billable { get; set; } = string.Empty;

    }
}
