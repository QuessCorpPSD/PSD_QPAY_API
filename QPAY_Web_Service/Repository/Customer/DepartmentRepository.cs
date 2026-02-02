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
    public class DepartmentRepository: IDepartmentRepository
    {
        private readonly DbRepository _dbRepository;

        public DepartmentRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<Department>> GetAllDepartmentDetails(string companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Companycode", companyId);

            var res = await this._dbRepository.GetItemsAsync("sp_GetDepartmentDetails", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Department>>(res) ?? new List<Department>();
            }

            return new List<Department>();
        }
        public async Task<DepartmentResponse> SaveUpdateDeleteDepartment([FromBody] DepartmentRequest request)
        {
            DepartmentResponse deptresponse = new DepartmentResponse();

            if (request == null || request.Departmentmaster == null || !request.Departmentmaster.Any())
            {
                deptresponse.response = "Invalid request.";
            }

            var xmlInput = BuildDepartmentXml(request);

            string storeProcedure = "sp_CreateUpdateDepartment";
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
                    if (!string.IsNullOrWhiteSpace(msg) && (msg.Contains("Department Created Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Department Updated Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Department Deleted Successfully", StringComparison.OrdinalIgnoreCase)))
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
        private string BuildDepartmentXml(DepartmentRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<DepartmentDetails>");

            foreach (var row in request.Departmentmaster)
            {
                sb.Append("<Department>");
                sb.AppendFormat("<Department_Id>{0}</Department_Id>", row.Department_Id);
                sb.AppendFormat("<Department_Name>{0}</Department_Name>", row.Department_Name);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", row.Company_Id);
                sb.AppendFormat("<Serial_No>{0}</Serial_No>", row.Serial_No);
                sb.Append("</Department>");
            }

            sb.Append("</DepartmentDetails>");
            return sb.ToString();
        }

        public async Task<string> PostDepartmentUpload(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Upload_Department", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public DataSet DepartmentExport(int companyId)
        {
            DataSet ds = this._dbRepository.DepartmentExport(companyId);
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
