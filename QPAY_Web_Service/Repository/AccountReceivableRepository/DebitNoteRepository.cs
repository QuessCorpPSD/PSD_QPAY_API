using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.DebitNote;
using QPay.DAL.Repository;
using QPay.UI.DebitNote;
using System.Data;

namespace QPay.BAL.Repository.DebitNote
{
    public class DebitNoteRepository : IDebitNoteRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public DebitNoteRepository(
            DbRepository dbRepository,
            IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        public async Task<DataSet> Search(
    string ClientName,
    string EmpCode,
    string FromDate,
    string ToDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@ClientName"] = ClientName,
                ["@EmpCode"] = EmpCode,
                ["@FromDate"] = FromDate,
                ["@ToDate"] = ToDate
            };

            return _dbRepository
                .ExecuteStoredProcedureToDataSetAsync(
                    "Sp_DebitNoteSearch_QPay",
                    parameters,
                    1500
                );
        }

        public async Task<DataSet> DebitNoteExportToExcel(
            DebitNoteExport payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] =
                    payload.companyId,

                ["@FromDate"] =
                    payload.fromDate,

                ["@ToDate"] =
                    payload.toDate
            };

            return  _dbRepository
                .ExecuteStoredProcedureToDataSetAsync(
                    "Sp_DebitNote_Export_QPay",
                    parameters,
                    1500
                );
        }
    }
}