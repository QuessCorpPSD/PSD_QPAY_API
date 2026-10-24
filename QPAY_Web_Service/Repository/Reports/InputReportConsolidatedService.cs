using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QPay.BAL.IRepository.Reports;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Reports
{
    public class InputReportConsolidatedService : IInputReportConsolidatedRepository
    {
        private readonly DbRepository _dbRepository;


        public InputReportConsolidatedService(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public DataSet GetInputReportConsolidated(int companyId, int payPeriodId, int inputId)
        {
            var parameters = new Dictionary<string, object> { { "@Company_Id", companyId }, { "@Pay_Period_Id", payPeriodId },{ "@InputType", inputId } };

            DataSet ds = this._dbRepository.ExecuteStoredProcedureToDataSetSecondaryAsync("InputAutomation_Custom_Report_Consolidated", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found");
            }
        }
    }
}
