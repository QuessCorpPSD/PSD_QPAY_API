using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Customer.Company;

namespace QPay.BAL.Repository.Customer
{
    public class CompanyPaycodeMappingRepository : ICompanyPaycodeMappingRepository
    {
        private readonly DbRepository _dbRepository;

        public CompanyPaycodeMappingRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataTable> Search(int? companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataTableAsync("sp_GetAllCompanyPayCodePayStructure", parameters);

        }
        public async Task<DataSet> ExportToExcel(int? companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllCompanyPayCodePayStructureExportToExcel", parameters);
        }

        public async Task<DataSet> GetAllCompanyPayCodePickFrom()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@companyPaycodePickFromId"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllCompanyPaycodePickFrom", parameters);
        }

        public async Task<DataSet> GetAllPaycodeCompanyPacode(string? PayCode, int? PayTypeId, int? IsTaxable, int? PayId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@PayCode_Code"] = PayCode,
                ["@PayTypeId"] = PayTypeId,
                ["@IsTaxable"] = IsTaxable,
                ["@PayCode_Id"] = PayId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaycode_CompanyPacode", parameters);
        }

        public async Task<DataSet> Create(string companyXml, string paycodeXml, string mode, int? User_Id)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = companyXml,
                ["@xmlInputDetail"]= paycodeXml,
                ["@mode"] = mode,
                ["@CreatedBy"] = User_Id
            };
            return this._dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateCompanyPayCodePayStructure", parameters);

            //if (!string.IsNullOrEmpty(res))
            //{
            //    return res;
            //}

            //return "No data found";
        }

    }
}
