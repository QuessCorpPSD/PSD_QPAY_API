using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Customer
{
    public class CostCenterMappingRepository: ICostCenterMappingRepository
    {
        private readonly DbRepository _dbRepository;

        public CostCenterMappingRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<CostCenterMapping>> GetAllCostCentertDetails(string? costCenter)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CostCenterMapname", costCenter);

            var res = await this._dbRepository.GetItemsAsync("sp_GetAllCostCenterMappingDetails", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CostCenterMapping>>(res) ?? new List<CostCenterMapping>();
            }

            return new List<CostCenterMapping>();
        }
        public async Task<CostCenterResponse> SaveUpdateDeleteCostCenter([FromBody] CostCenterRequest request)
        {
            CostCenterResponse CostCenterresponse = new CostCenterResponse();

            if (request == null || request.CostCentermaster == null || !request.CostCentermaster.Any())
            {
                CostCenterresponse.response = "Invalid request.";
            }

            var xmlInput = BuildCostCenterXml(request);

            string storeProcedure = "sp_CreateUpdateCostCenterMapping";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@CreatedBy", request.Created_By);
            parameters.Add("@mode", request.Mode);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            string? msg = null;

            if (!string.IsNullOrWhiteSpace(res))
            {
                var arr = JArray.Parse(res);              
                msg = arr[0]?["Error_Message"]?.ToString(); 
            }
            if (!string.IsNullOrWhiteSpace(msg))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(msg) && (msg.Contains("Cost center mapping created successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Cost center mapping Updated Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Cost center mapping Deleted Successfully", StringComparison.OrdinalIgnoreCase)))
                    {
                        CostCenterresponse.response = msg;
                    }
                    else
                    {
                        CostCenterresponse.response = "Failed to " + request.Mode + ".";
                        CostCenterresponse.errors = msg
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    CostCenterresponse.response = "Error while processing response.";
                }
            }
            else
            {
                CostCenterresponse.response = "Failed";
            }

            return CostCenterresponse;
        }
        private string BuildCostCenterXml(CostCenterRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<CostCenterMappingDetails>");

            foreach (var row in request.CostCentermaster)
            {
                sb.Append("<CostCenterMapping>");
                sb.AppendFormat("<Cost_Center_Mapping_Id>{0}</Cost_Center_Mapping_Id>", row.Cost_Center_Mapping_Id);
                sb.AppendFormat("<Map_Name>{0}</Map_Name>", row.Map_Name);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", row.Company_Id);
                sb.AppendFormat("<SPOC_Name>{0}</SPOC_Name>", row.SPOC_Name);
                sb.AppendFormat("<Cost_Center_Name>{0}</Cost_Center_Name>", row.Cost_Center_Name);
                sb.AppendFormat("<GRN_Number>{0}</GRN_Number>", row.GRN_Number);
                sb.AppendFormat("<IsActive>{0}</IsActive>", row.IsActive);
                sb.AppendFormat("<Group_Detail_Id>{0}</Group_Detail_Id>", row.Group_Detail_Id);
                sb.Append("</CostCenterMapping>");
            }
            sb.Append("</CostCenterMappingDetails>");
            return sb.ToString();
        }

        public async Task<string> PostCostCenterUpload(string xmlString, string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_CostCenterMapping_Upload", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public DataSet CostCenterExport(string? CostCenterMapName)
        {
            DataSet ds = this._dbRepository.CostCenterExport(CostCenterMapName);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given Parameters.");
            }

        }
    }
}
