using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.Billing;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Billing;
using QPay.UI.Common;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.Customer;
using QPay.UI.Utilities;
using QPay.UI_Domain.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Billing.GenericUpload;
using static QPay.UI.Customer.Company;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;

namespace QPay.BAL.Repository.Billing
{
    public class SapBookClosureRepository : ISapBookClosureRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public SapBookClosureRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetMonths()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "GetMonths",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_CommonDropDowns", parameters, 1500);
        }

        public async Task<DataSet> GetBusinessUnitNames()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "GetEntity",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_SAP_BookClosure", parameters, 1500);
        }

        public async Task<DataSet> Search()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Search",
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_SAP_BookClosure", parameters, 1500);
        }

        public async Task<DataSet> Create(SapBookClosureRequest items)
        {
            var parentdata = GenericSerializer<SapBookClosure>.Serialize(items.parentDetail);
            var parameters = new Dictionary<string, object>
            {
                ["@xmlData"] = parentdata,
                ["@Action"] = items.mode,
                ["@Created_By"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Manage_SAP_BookClosure", parameters);
        }

    }
}
