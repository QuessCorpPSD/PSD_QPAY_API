using QPay.UI.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Reports
{
    public interface IPoReportRepository
    {
        Task<string> GetAllPOEmployeeReportNew(string employeeId);
        Task<string> GetAllPOEmployeeReportOld(string employeeId);
        Task<string> GetPOYears();
        Task<string> GetVerticals(string userId, string poType);
        Task<string> POActiveReportGrid(POActiveInactive pOActiveInactive);
        DataSet GetAllMonthWisePOReport(string txtFromDate, string txtToDate);
        Task<DataSet> GetGrossMarginReport(string pay_Period, int submit);
    }
}
