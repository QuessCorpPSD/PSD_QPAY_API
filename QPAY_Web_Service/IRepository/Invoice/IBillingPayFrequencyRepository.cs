using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository
{
    public interface IBillingPayFrequencyRepository
    {       
        Task<DataSet> Search(int? companyId);
        Task<DataSet> ExportToExcel(int? companyId);
        Task<DataSet> GetGroupName(int? companyId);
        Task<DataSet> GetData( string startDate, string endDate);
        Task<DataSet> CheckPayFrequencyExists(int companyId, string startDate, string endDate, string payPeriod);

        Task<DataSet> Create(BillingPayFrequencyRequest request);
    }
}
