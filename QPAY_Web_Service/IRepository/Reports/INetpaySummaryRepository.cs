using QPay.UI.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Report.EntityReport;

namespace QPay.BAL.IRepository.Reports
{
    public interface INetpaySummaryRepository
    {
        Task<DataSet> ExportToExcel(int? CompanyId, int? PayPeriodId);
        Task<DataSet> ExportToExcel_Entity(EntityPayregisterFilter items);
    }
}
