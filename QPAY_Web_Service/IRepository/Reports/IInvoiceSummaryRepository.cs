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
    public interface IInvoiceSummaryRepository
    {
        Task<DataSet> GetTaxTypes();
        Task<DataSet> ExportToExcel(int? CompanyId, string FromDate, string ToDate, int? ReportTypeId, int? UserId);

        Task<DataSet> ExportToExcel_Entity(EntityPayregister_Filter items);

    }
}
