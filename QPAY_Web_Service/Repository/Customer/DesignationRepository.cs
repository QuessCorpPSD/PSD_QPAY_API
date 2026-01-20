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
    public class DesignationRepository: IDesignationRepository
    {

        private readonly DbRepository _dbRepository;

        public DesignationRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<Designation>> GetAllDesignationDetails(string companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Companycode", companyId);

            var res = await this._dbRepository.GetItemsAsync("sp_GetDesignationDetails", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Designation>>(res) ?? new List<Designation>();
            }

            return new List<Designation>();
        }
        public async Task<DesignationResponse> SaveUpdateDeleteDesignation([FromBody] DesignationRequest request)
        {
            DesignationResponse deptresponse = new DesignationResponse();

            if (request == null || request.Designationmaster == null || !request.Designationmaster.Any())
            {
                deptresponse.response = "Invalid request.";
            }

            var xmlInput = BuildDesignationXml(request);

            string storeProcedure = "sp_CreateUpdateDesignation";
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
                    if (!string.IsNullOrWhiteSpace(msg) && (msg.Contains("Designation Created Sucessfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Designation Updated Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Designation Deleted Successfully", StringComparison.OrdinalIgnoreCase)))
                    {
                        deptresponse.response = msg;
                    }
                    else
                    {
                        deptresponse.response = "Failed to " + request.Mode + ".";
                        deptresponse.errors = msg
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    deptresponse.response = "Error while processing response.";
                }
            }
            else
            {
                deptresponse.response = "Failed";
            }

            return deptresponse;
        }
        private string BuildDesignationXml(DesignationRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<DesignationDetails>");

            foreach (var row in request.Designationmaster)
            {
                sb.Append("<Designation>");
                sb.AppendFormat("<Designation_Id>{0}</Designation_Id>", row.Designation_Id);
                sb.AppendFormat("<Designation_Name>{0}</Designation_Name>", row.Designation_Name);
                sb.AppendFormat("<Standard_Designation>{0}</Standard_Designation>", row.Standard_Designation);
                sb.AppendFormat("<Amount>{0}</Amount>", row.Amount);
                sb.AppendFormat("<Skill_Category>{0}</Skill_Category>", row.Skill_Category);
                sb.AppendFormat("<NpDays>{0}</NpDays>", row.NpDays);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", row.Company_Id);
                sb.AppendFormat("<Serial_No>{0}</Serial_No>", row.Serial_No);
                sb.Append("</Designation>");
            }

            sb.Append("</DesignationDetails>");
            return sb.ToString();
        }

        public async Task<string> PostDesignationUpload(string xmlString, string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_Designation", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public DataSet DesignationExport(int companyId)
        {
            DataSet ds = this._dbRepository.DesignationExport(companyId);
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
