using Dapper;
using Newtonsoft.Json;
using QPay.DAL.Repository;
using QPay.DTo.Models.Masters;
using QPay.IRepository.iRepository.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.IRepository.Repository.Masters
{
    public class InvoiceRuleRepository : IInvoiceRuleRepository
    {
        private readonly DbRepository _dbRepository;

        public InvoiceRuleRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<InvoiceRule>> GetAllInvoiceRule(int? companyId, string? siteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            parameters.Add("@Site_id", siteId);

            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllInvoicingRule", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InvoiceRule>>(res) ?? new List<InvoiceRule>();
            }

            return new List<InvoiceRule>();
        }
        public async Task<string> PostAddInvoiceRule(InvoiceRuleAdd invoiceRuleAdd)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", invoiceRuleAdd.CompanyId);
            parameters.Add("@SITE_Id", invoiceRuleAdd.SiteId);
            parameters.Add("@CompanyCode", invoiceRuleAdd.CompanyCode);
            parameters.Add("@SITE_NAME", invoiceRuleAdd.SiteName);
            parameters.Add("@DAYS_PER_MONTH", invoiceRuleAdd.DaysPerMonth);
            parameters.Add("@BillingType", invoiceRuleAdd.BillingType);
            parameters.Add("@WEEKENDS", invoiceRuleAdd.WeekendsRule);
            parameters.Add("@HOLIDAYS", invoiceRuleAdd.HolidaysRule);
            parameters.Add("@COMP_OFF", invoiceRuleAdd.ComppoffRule);
            parameters.Add("@MATERNITY", invoiceRuleAdd.MaternityLeave);
            parameters.Add("@LEAVE_ADDITION", invoiceRuleAdd.LeaveCredit);
            parameters.Add("@LEAVE_RULE", invoiceRuleAdd.LeaveRule);
            parameters.Add("@OT", invoiceRuleAdd.OtRule);
            parameters.Add("@Gratuity", invoiceRuleAdd.Gratuity);
            parameters.Add("@Rebates", invoiceRuleAdd.Rebates);
            parameters.Add("@ServiceFee", invoiceRuleAdd.ServiceFeeOnExpenses);
            parameters.Add("@Reimbursement", invoiceRuleAdd.Reimbursement);
            parameters.Add("@Discounts", invoiceRuleAdd.Discounts);
            parameters.Add("@BILLABLE_DAYS_Formula", invoiceRuleAdd.BillableDaysFormula);
            parameters.Add("@AsPerTimesheet", invoiceRuleAdd.DaysAsPerTimesheet);
            parameters.Add("@Created_by", invoiceRuleAdd.UserId);
            parameters.Add("@Leavetypes", invoiceRuleAdd.LeaveTypes);
            parameters.Add("@LeavePeriod", invoiceRuleAdd.PayPeriodFrom + "-" + invoiceRuleAdd.PayPeriodTo);
            parameters.Add("@Carryfarward", invoiceRuleAdd.CarryForward);
            parameters.Add("@NoofCarryfarward", invoiceRuleAdd.NoOfCarryForwards);
            parameters.Add("@payroll_weekends_billable", invoiceRuleAdd.payroll_weekends_billable);

            var res = await this._dbRepository.GetItemsAsync("Get_tb_ClientWiseInvoicingRules_Insert_new", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> PostDeleteInvoiceRule(int invoicingRulesID)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@invoicingRulesID", invoicingRulesID);
            var res = await this._dbRepository.GetItemsAsync("Proc_Delete_InvoiceRule", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public DataSet GetInvoiceRuleTemplate(int companyId, string siteName)
        {
            DataSet ds = this._dbRepository.GetInvoiceRuleTemplate(companyId, siteName);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company.");
            }

        }
        public async Task<string> PostInvoiceRuleUpload(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Xmldoc", xmlString);
            parameters.Add("@Created_By", userId);

            var res = await this._dbRepository.GetItemsAsync("proc_bulk_InvoiceRule", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public DataSet InvoiceRuleExport(int companyId, int siteCode)
        {
            DataSet ds = this._dbRepository.InvoiceRuleExport(companyId, siteCode);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given Parameters.");
            }

        }
    }
}
