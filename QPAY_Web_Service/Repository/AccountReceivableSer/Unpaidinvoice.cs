using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class Unpaidinvoice : IUnpaidinvoice
    {
        private readonly DbRepository _dbRepository;

        public Unpaidinvoice(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public async Task<DataSet> GetEntity(string flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = flag
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "Proc_ManageLegalEntityMapping",
            parameters,
            1500
            );
        }
        public async Task<DataSet> ExportToExcel(CommonExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@AllEntityId"] = 0
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Proc_UnPaidInvoiceReport",
                parameters,
                1500
            );
        }
    }
}
