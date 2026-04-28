using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.UI_Domain.Models.AccountReceivable;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface ICollectionPendingReportRepository
    {
        Task<DataSet> GetFinancialYear(int? financialYearId);

        Task<DataSet> GetEntity(string flag);

         Task<DataSet> CollectionPendingExportToExcel(CollectionPendingExport payload);

    }
}
