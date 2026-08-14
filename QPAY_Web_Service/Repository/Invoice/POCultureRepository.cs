using Dapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.InvoiceCulture;
using static QPay.UI.Models.Invoice.POCulture;

namespace QPay.BAL.Repository.Invoice
{
    public class POCultureRepository: IPOCultureRepository

    {
        private readonly DbRepository _dbRepository;

        public POCultureRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
    
        public async Task<List<PoCulture>> GetAllPOCulture(int companyId, int UserId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "Get",
                ["@UserId"] = UserId,
                ["@CompanyId"] = companyId
            };

            var res = await _dbRepository.GetItemsAsync("Proc_ManagePOInvoiceCulture_New", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PoCulture>>(res)
                       ?? new List<PoCulture>();
            }

            return new List<PoCulture>();
        }

     
        public async Task<DataSet> Create(POCultureRequest model, int createdBy)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = model.mode,
                ["@UserId"] = createdBy,
                ["@Id"] = model.parentDetail.POCulture_id,
                ["@CompanyId"] = model.parentDetail.Company_Id,
                ["@CostCenterMappingId"] = model.parentDetail.Cost_Center_Mapping_Id,
                ["@IsMapnameWiseInvoice"] = model.parentDetail.IsMapnameWiseInvoice,
                ["@IsActive"] = true,
                ["@CreatedBy"] = createdBy,
                ["@CreatedOn"] = DateTime.Now,
                ["@ModifiedBy"] = DBNull.Value,
                ["@ModifiedOn"] = DBNull.Value
            };

            return this._dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManagePOInvoiceCulture_New", parameters);

        }
        public async Task<string> PostPOCulture(string xmlString, string userId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Action", "Import");
            parameters.Add("@UserId", Convert.ToInt32(userId));
            parameters.Add("@XmlData", xmlString);

            var res = await _dbRepository.GetItemsAsync(
                "Proc_ManagePOInvoiceCulture_New",
                parameters);

            return !string.IsNullOrWhiteSpace(res)
                ? res
                : "No data found";
        }
        public DataSet POCultureExport(int companyId,int userId)
        {
            DataSet ds = this._dbRepository.POCultureExport(companyId,userId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given Parameters.");
            }

        }

        public async Task<List<PurchaseOrder>> GetAllPONumbers(int companyId, int UserId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "GetPONumbers",
                ["@UserId"] = UserId,
                ["@CompanyId"] = companyId
            };

            var res = await _dbRepository.GetItemsAsync("Proc_ManagePOInvoiceCulture_New", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PurchaseOrder>>(res)
                       ?? new List<PurchaseOrder>();
            }

            return new List<PurchaseOrder>();
        }
    }
}
