using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Process;
using QPay.DAL.Repository;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.Repository.Process
{
    public class ReimbursementCalendarRepository : IReimbursementCalendarRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ReimbursementCalendarRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(SearchReimbursementRequest searchRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = searchRequest.CompanyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllReimbursement_Calender", parameters, 1500);
        }

    }
}
