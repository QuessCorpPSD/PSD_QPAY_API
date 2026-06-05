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
            parameters.Add("@WEEKENDS", invoiceRuleAdd.WeekendsRule.ToUpper());
            parameters.Add("@HOLIDAYS", invoiceRuleAdd.HolidaysRule.ToUpper());
            parameters.Add("@COMP_OFF", invoiceRuleAdd.ComppoffRule.ToUpper());
            parameters.Add("@MATERNITY", invoiceRuleAdd.MaternityLeave.ToUpper());
            parameters.Add("@LEAVE_ADDITION", invoiceRuleAdd.LeaveCredit);
            parameters.Add("@LEAVE_RULE", invoiceRuleAdd.LeaveRule.ToUpper());
            parameters.Add("@OT", invoiceRuleAdd.OtRule.ToUpper());
            parameters.Add("@Gratuity", invoiceRuleAdd.Gratuity.ToUpper());
            parameters.Add("@Rebates", invoiceRuleAdd.Rebates.ToUpper());
            parameters.Add("@ServiceFee", invoiceRuleAdd.ServiceFeeOnExpenses.ToUpper());
            parameters.Add("@Reimbursement", invoiceRuleAdd.Reimbursement.ToUpper());
            parameters.Add("@Discounts", invoiceRuleAdd.Discounts.ToUpper());
            parameters.Add("@BILLABLE_DAYS_Formula", invoiceRuleAdd.BillableDaysFormula);
            parameters.Add("@AsPerTimesheet", invoiceRuleAdd.DaysAsPerTimesheet);
            parameters.Add("@Created_by", invoiceRuleAdd.UserId);
            parameters.Add("@Leavetypes", invoiceRuleAdd.LeaveTypes);
            parameters.Add("@LeavePeriod", invoiceRuleAdd.PayPeriodFrom + "-" + invoiceRuleAdd.PayPeriodTo);
            parameters.Add("@Carryfarward", invoiceRuleAdd.CarryForward);
            parameters.Add("@NoofCarryfarward", invoiceRuleAdd.NoOfCarryForwards);
            parameters.Add("@payroll_weekends_billable", invoiceRuleAdd.payroll_weekends_billable);
            parameters.Add("@type_of_billing", invoiceRuleAdd.type_of_billing);
            parameters.Add("@type_of_billing_name", invoiceRuleAdd.type_of_billing_name);

            var res = await this._dbRepository.GetItemsAsync("Get_tb_ClientWiseInvoicingRules_Insert_new", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> PostUpdateInvoiceRule(InvoiceRuleUpdate invoiceruleUpdate)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@invoicingRulesID", invoiceruleUpdate.invoicingRulesID);
            parameters.Add("@DAYS_PER_MONTH", invoiceruleUpdate.dayspermonth);
            parameters.Add("@BillingType", invoiceruleUpdate.billingtype);
            parameters.Add("@WEEKENDS", invoiceruleUpdate.weekendsrule.ToUpper());
            parameters.Add("@HOLIDAYS", invoiceruleUpdate.holidaysrule.ToUpper());
            parameters.Add("@COMP_OFF", invoiceruleUpdate.comppoffrule.ToUpper());
            parameters.Add("@MATERNITY", invoiceruleUpdate.maternityleave.ToUpper());
            parameters.Add("@LEAVE_ADDITION", invoiceruleUpdate.leavecredit);
            parameters.Add("@LEAVE_RULE", invoiceruleUpdate.leaverule.ToUpper());
            parameters.Add("@OT", invoiceruleUpdate.otrule.ToUpper());
            parameters.Add("@Gratuity", invoiceruleUpdate.gratuity.ToUpper());
            parameters.Add("@Rebates", invoiceruleUpdate.rebates.ToUpper());
            parameters.Add("@ServiceFee", invoiceruleUpdate.servicefeeonexpenses.ToUpper());
            parameters.Add("@Reimbursement", invoiceruleUpdate.reimbursement.ToUpper());
            parameters.Add("@Discounts", invoiceruleUpdate.discounts.ToUpper());
            parameters.Add("@BILLABLE_DAYS_Formula", invoiceruleUpdate.billabledaysformula);
            parameters.Add("@AsPerTimesheet", invoiceruleUpdate.daysAsPerTimesheet);
            parameters.Add("@Leavetypes", invoiceruleUpdate.leavetypes);
            parameters.Add("@LeavePeriod", invoiceruleUpdate.payperiodfrom + "-" + invoiceruleUpdate.payperiodto);
            parameters.Add("@Carryfarward", invoiceruleUpdate.carryforward);
            parameters.Add("@NoofCarryfarward", invoiceruleUpdate.noofcarryforwards);
            parameters.Add("@payroll_weekends_billable", invoiceruleUpdate.payroll_weekends_billable);
            parameters.Add("@type_of_billing", invoiceruleUpdate.type_of_billing);
            parameters.Add("@type_of_billing_name", invoiceruleUpdate.type_of_billing_name);
            parameters.Add("@Created_by", invoiceruleUpdate.userId);


            var res = await this._dbRepository.GetItemsAsync("Get_tb_ClientWiseInvoicingRules_Update_new", parameters);

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
        //public DataSet GetInvoiceRuleTemplate(int companyId, string siteName)
        //{
        //    DataSet ds = this._dbRepository.GetInvoiceRuleTemplate(companyId, siteName);
        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        return ds;
        //    }
        //    else
        //    {
        //        throw new Exception("No data found for the given company.");
        //    }

        //}
        public async Task<string> PostInvoiceRuleUpload(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Xmldoc", xmlString);
            parameters.Add("@Created_By", userId);

            var res = await this._dbRepository.GetItemsAsync("proc_bulk_InvoiceRule_New", parameters);

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

        public DataSet GetInvoiceruleTemplate(int? companyId, string? siteName)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@SiteName"] = siteName
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Template_InvoiceRule_New", parameters, 1500);
        }
    }
}
