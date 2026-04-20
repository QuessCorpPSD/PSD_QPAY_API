using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.BAL.Models;
using QPay.UI.Models.AccountReceivableMod;


namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface IClientledger
    {
        Task<DataSet> GetFinancialYear(int? financialYearId);
        Task<DataSet> ExportClientLedger(ClientLedgerExportRequest request);
    }
}
