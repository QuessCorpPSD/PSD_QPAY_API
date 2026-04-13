using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using QPay.UI.Models;
using QPay.UI.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Reports
{
    public class PoReportRepository : IPoReportRepository
    {
        private readonly DbRepository _dbRepository;

        public PoReportRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<string> GetAllPOEmployeeReportNew(string employeeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EMP_ID", employeeId);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_EMPLOYEEWISE_REPORT_NEW", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        //public async Task<>
        public async Task<string> GetAllPOEmployeeReportOld(string employeeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EMP_ID", employeeId);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_EMPLOYEEWISE_REPORT_NEW", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> GetPOYears()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("USP_POACTIVE_GETYEARS", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<DataSet> GetGrossMarginReport(string pay_Period,int submit)
        {
            var parameters = new Dictionary<string, object?>
            {
                
                ["@Pay_Period"] = pay_Period,
                ["@Submit"] = submit
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GrossMarginReport", parameters, 1500);
        }
        //public async Task<FileResponse> AccuralFileFormat()
        //{
        //    var parameter = new DynamicParameters();
        //    var res = await _dbRepository.GetItemsAsync("SP_Accrual_Format_download", parameter);
        //    if(res.Any())
        //    {
        //        DataTable dataTable= JsonConvert.DeserializeObject<DataTable>>(res).ToList();
        //    }
        //}

        public async Task<DataSet> GetUnProcessedGrossMarginReport(string pay_Period)
        {
            var parameters = new Dictionary<string, object?>
            {

                ["@Pay_Period"] = pay_Period
               
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("UnProcessed_GrossMargin_Payregister", parameters, 1500);
        }

        public async Task<string> GetVerticals(string userId, string poType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LOGGEDIN_USER", userId);
            parameters.Add("@PO_TYPE", poType);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_GET_VERTICALS", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> POActiveReportGrid(POActiveInactive pOActiveInactive)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CLIENT_ID", pOActiveInactive.CompanyId);
            parameters.Add("@SITE_ID", pOActiveInactive.SiteId);
            parameters.Add("@ISACTIVE", pOActiveInactive.Isactive);
            parameters.Add("@Access_Company_Code", pOActiveInactive.CompanyCode);
            parameters.Add("@PO_TYPE", pOActiveInactive.PoType);
            parameters.Add("@YEAR", pOActiveInactive.PoYear);
            parameters.Add("@VERTICAL", pOActiveInactive.Vertical);
            parameters.Add("@LOGGEDIN_USER", pOActiveInactive.UserId);

            var res = await this._dbRepository.GetItemsAsync("USP_PO_GET_NEW_EMPACTIVE_REPORTDEATILS_EXPORT", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }


        public DataSet GetAllMonthWisePOReport(string txtFromDate, string txtToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@STARTDT"] = txtFromDate,
                ["@ENDDT"] = txtToDate
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_PO_EmployeeMonth_Report_1", parameters);
        }
    }
}
