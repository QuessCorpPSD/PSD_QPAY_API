using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Invoice
{
    public class ClientBillableReportRepository : IClientBillableReportRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _config;

        public ClientBillableReportRepository(DbRepository dbRepository, IConfiguration config)
        {
            this._dbRepository = dbRepository;
            this._config = config;
        }

        public async Task<DataSet> Search(int? entityId, string? startDate, string? endDate)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EntityId"] = entityId,
                ["@FromDate"] = startDate,
                ["@ToDate"] = endDate,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllBillableReport_ExportToExcel_datewise_New", parameters, 1500);
        }
    }
}
