using ClosedXML.Excel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.TaxAndSaving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository
{
    public class GratuityRepository : IGratuityRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public GratuityRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetEmployeeCodeForGratuity(int? companyId, int? FinancialYrId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@FinancialYrId"] = FinancialYrId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetResignedEmployeeDetails", parameters, 1500);
        }

        public async Task<DataSet> GetGratuityEmployeeByEmpId(int? employeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EmployeeId"] = employeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetGratuityEmployeeByEmpId", parameters, 1500);
        }

        public async Task<DataSet> GetBasicAmountByEmployeeId(int? employeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EmployeeId"] = employeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetBasicAmountByEmployeeId", parameters, 1500);
        }
        public async Task<DataSet> GetDAAmountByEmployeeId(int? employeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@EmployeeId"] = employeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetDAAmountByEmployeeId", parameters, 1500);
        }

        public async Task<DataSet> Search(int? companyId,int? EmployeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@EmployeeId"] = EmployeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetGratuityDetails", parameters, 1500);
        }

        public async Task<DataSet> Create(GratuityRequest items)
        {
            var objGratuityResponse = new GratuityResponse();
            objGratuityResponse.GratuityDetails = new Gratuity[1];
            objGratuityResponse.GratuityDetails[0] = items.parentDetail;

            string GratuityResponseSerialize = GenericSerializer<GratuityResponse>.Serialize(objGratuityResponse);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInputDetail"] = GratuityResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateGratuity", parameters);
        }

    }
}
