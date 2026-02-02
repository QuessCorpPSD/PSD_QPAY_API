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
    public class ChildrenEducationAllowanceRepository : IChildrenEducationAllowanceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ChildrenEducationAllowanceRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> sp_GetFinancialYear()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@FinancialYearId"] = "0",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetFinancialYear", parameters, 1500);
        }

        public async Task<DataSet> GetAllType()
        {
            var parameters = new Dictionary<string, object?>
            {
               
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllDeclarationType", parameters, 1500);
        }

        public async Task<DataSet> GetEmployeesList(int? companyId, int? financialYearId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Financial_Year_Id"] = financialYearId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEmployeesByCompanyIdandFinYear", parameters, 1500);
        }

        public async Task<DataSet> GetEligibleEmployee(int? financialYearId, int? EmployeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Financial_Year_Id"] = financialYearId,
                ["@Employee_Id"] = EmployeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEligibleAmountbyEmployeeID", parameters, 1500);
        }

        public async Task<DataSet> GetEligibleChildren(string Effective_Date, int Number_Of_Children)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Effective_Date"] = Effective_Date,
                ["@Number_Of_Children"] = Number_Of_Children,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetEligibleAmountbyFinancialYearNoOfChildren", parameters, 1500);
        }

        public async Task<DataSet> Search(int? companyId, int? financialYearId, int? EmployeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@FinancialYearId"] = financialYearId,
                ["@EmployeeId"] = EmployeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spGetAllChildrenEducationAllowance", parameters, 1500);
        }

        public async Task<DataSet> Create(ChildrenEducationAllowanceRequest items)
        {
            string parentdata = JsonConvert.SerializeObject(items.parentDetail);
            string childdata = JsonConvert.SerializeObject(items.childDetail);

            ChildrenEducationAllowance objChildrenEducationAllowance = JsonConvert.DeserializeObject<ChildrenEducationAllowance>(parentdata);
            ChildrenEducationAllowanceDetail[] objChildrenEducationAllowanceDetail = JsonConvert.DeserializeObject<ChildrenEducationAllowanceDetail[]>(childdata);
            var ChildrenEducationAllowanceDetailResponse = new ChildrenEducationAllowanceResponse();
            ChildrenEducationAllowanceDetailResponse.ChildrenEducationAllowance = new ChildrenEducationAllowance[1];
            ChildrenEducationAllowanceDetailResponse.ChildrenEducationAllowance[0] = objChildrenEducationAllowance;
            string resultMessage = string.Empty;
            var objChildrenEducationAllowanceResponse2 = new ChildrenEducationAllowanceDetailResponse();
            objChildrenEducationAllowanceResponse2.ChildrenEducationAllownceDetails = objChildrenEducationAllowanceDetail;
            string ChildrenEducationAllowanceResponseSerialize = GenericSerializer<ChildrenEducationAllowanceResponse>.Serialize(ChildrenEducationAllowanceDetailResponse);
            string ChildrenEducationAllowanceResponseDetailSerialize = GenericSerializer<ChildrenEducationAllowanceDetailResponse>.Serialize(objChildrenEducationAllowanceResponse2);
            ChildrenEducationAllowanceResponseSerialize = ChildrenEducationAllowanceResponseSerialize == "<ChildrenEducationAllowanceDetailResponse/>" ? "<ChildrenEducationAllowanceDetailResponse></ChildrenEducationAllowanceDetailResponse>" : ChildrenEducationAllowanceResponseSerialize;


            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = ChildrenEducationAllowanceResponseSerialize,
                ["@xmlInputDetail"] = ChildrenEducationAllowanceResponseDetailSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateChildrenEducationAllowance", parameters);
        }

    }
}
