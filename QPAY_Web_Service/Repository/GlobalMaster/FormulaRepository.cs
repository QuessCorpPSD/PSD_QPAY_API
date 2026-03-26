using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.GlobalMaster
{

    public class FormulaRepository : IFormulaRepository
    {
        private readonly DbRepository _dbRepository;

        public FormulaRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataSet> Search(int? PayCode_Id)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Paycode_Id"] = PayCode_Id,
                ["@Formula_Id"] = 0,
            };
            return  _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllFormula", parameters); ;

        }

        //public async Task<DataSet> GetFormulaPayCodeList()
        //{
        //    var parameters = new Dictionary<string, object>
        //    {
        //        ["@PayCode_Code"] = "",
        //        ["@PayTypeId"] = 0,
        //        ["@IsTaxable"] = 0,
        //        ["@PayCode_Id"] = 0,
        //    };

        //    DataSet dataSet = _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllPaycode", parameters);
            
        //    return dataSet;

        //}

        public async Task<DataSet> GetPayCategory(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Companycode"] = companyId,
                ["@Band_Id"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetBandDetails", parameters); ;

        }

        public async Task<DataSet> Create(FormulasRequest items)
        {

            //var parentdata = GenericSerializer<Entity>.Serialize(items.parentDetail);
            var formulaResponse = new FormulaResponse();
            formulaResponse.FormulaDetails = new Formulas[1];
            formulaResponse.FormulaDetails[0] = items.detail;

            string formulaResponseSerialize = GenericSerializer<FormulaResponse>.Serialize(formulaResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = formulaResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateFormula", parameters);
        }

        public async Task<DataSet> GetPayrollType()
        {
            var parameters = new Dictionary<string, object>
            {
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetPayrollType", parameters); ;

        }

        public async Task<DataSet> CreateMC(MCFormulasRequest items)
        {

            //var parentdata = GenericSerializer<Entity>.Serialize(items.parentDetail);
            var formulaResponse = new MCFormulaResponse();
            formulaResponse.FormulaDetails = new MCFormulas[1];
            formulaResponse.FormulaDetails[0] = items.detail;

            string formulaResponseSerialize = GenericSerializer<MCFormulaResponse>.Serialize(formulaResponse);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = formulaResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateMCFormula", parameters);
        }


    }
}
