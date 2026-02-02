using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using static QPay.UI.Models.Reports.Payslip;



namespace QPay.BAL.Repository.Reports
{
    public class PayslipReportRepository : IPayslipReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public PayslipReportRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetEmployee(int CompanyId, int PayperiodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = CompanyId,
                ["@Pay_Period_Id"] = PayperiodId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_Getemployee_details_Payslip", parameters, 1500);
        }

        public async Task<DataSet> DownloadPayslip(int EmployeeId, string payperiod)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EmployeeId"] = EmployeeId,
                ["@PayPeriodMonth"] = payperiod
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Create_PaySlip", parameters, 1500);
        }
    }
}
