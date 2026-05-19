using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using QPay.UI.Models.AccountReceivableMod;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class APARAdjustmentReposioty : IAPARAdjustmentRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
    
     public APARAdjustmentReposioty(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchAPARAdjustmentUpdate(int CompanyId, string fromdate, string todate)
        {
            DateTime from;
            DateTime to;

          
            if (!DateTime.TryParse(fromdate, out from))
                throw new Exception("Invalid From Date");

            if (!DateTime.TryParse(todate, out to))
                throw new Exception("Invalid To Date");

            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = CompanyId,
                ["@fromdate"] = from,
                ["@todate"] = to
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_SearchAPARAdjustmentUpdate",
                parameters,
                1500
            );
        }
        public async Task<DataSet> APARAdjustmentEmployeeSearch(string APARAdjustmentNo)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@APARAdjustmentNo"] = APARAdjustmentNo
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_SearchEmployeeAPARAdjustmentUpdate", 
                parameters,
                1500
            );
        }

        public async Task<DataSet> APARAdjustmentExportToExcel(APARAdjustmentExport payload)
        {
            DateTime from = DateTime.Parse(payload.fromDate);
            DateTime to = DateTime.Parse(payload.toDate);

            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = payload.companyId,
                ["@fromdate"] = from,
                ["@todate"] = to
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_SearchAPARAdjustmentUpdateDetail_ExportToExcel",
                parameters,
                1500
            );
        }

        public async Task<APARAdjustmentUploadResponse> UploadAPARAdjustmentCancel(IFormFile file, string User)
        {
            APARAdjustmentUploadResponse response = new APARAdjustmentUploadResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

               
                var dirPath = Path.Combine(_configuration["ClaimDocPath"], "APARAdjustmentCancel");

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(dirPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

              
                DataTable dt = ReadExcelToDataTable(filePath);

                if (dt.Rows.Count == 0)
                {
                    response.response = "Excel sheet is empty.";
                    return response;
                }

                
                dt.TableName = "Table";
                DataSet ds = new DataSet("NewDataSet");
                ds.Tables.Add(dt);

                string xmlInput;
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

              
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", User);

                string spName = "Proc_Upload_BulkAPARAdjustmentCancel"; // ✅ from MVC

                var res = await _dbRepository.GetItemsAsync(spName, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success"))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Upload completed with errors";
                        response.errors = res
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    response.response = "Failed";
                }
            }
            catch (Exception ex)
            {
                response.response = ex.Message;
            }

            return response;
        }

        private DataTable ReadExcelToDataTable(string filePath)
        {
            DataTable dt = new DataTable();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1); // first sheet
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                   
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString().Trim());
                        }
                        firstRow = false;
                    }
                    else
                    {
                     
                        dt.Rows.Add();
                        int i = 0;

                        foreach (var cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString().Trim();
                            i++;
                        }
                    }
                }
            }

            return dt;
        }


        public async Task<string> EditAPARAdjustment(APARAdjustmentEditRequest request)
        {
            if (request == null || request.APARAdjustment == null)
                return "Invalid request";

           
            var headerXml = BuildAPARHeaderXml(request);

         
            var employeeList = JsonConvert.DeserializeObject<List<APARAdjustmentEmployee>>(request.APARAdjustmentdetail);
            var employeeXml = BuildAPAREmployeeXml(employeeList);

            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", headerXml);
            parameters.Add("@xmlEmployeeInput", employeeXml);
            parameters.Add("@Createdby", request.Created_By);
            parameters.Add("@mode", request.Mode);

      
            var result = await _dbRepository.GetItemsAsync(
                "dbo.sp_APARAdjustmentUpdateSaveAndCancel",
                parameters
            );

            return result;
        }
        private string BuildAPARHeaderXml(APARAdjustmentEditRequest request)
        {
            var sb = new StringBuilder();

            sb.Append("<APARAdjustmentDetails>");
            sb.Append("<APARAdjustment>");

            sb.AppendFormat("<APARAdjustment_No>{0}</APARAdjustment_No>", request.APARAdjustment.APARAdjustment_No ?? "");
            sb.AppendFormat("<APAR_Adjustment_Type_Text>{0}</APAR_Adjustment_Type_Text>", request.APARAdjustment.APAR_Adjustment_Type_Text ?? "");
            sb.AppendFormat("<Invoice_Number>{0}</Invoice_Number>", request.APARAdjustment.Invoice_Number ?? "");
            sb.AppendFormat("<Sap_Reference_Number>{0}</Sap_Reference_Number>", request.APARAdjustment.Sap_Reference_Number ?? "");
            sb.AppendFormat("<APAR_Adjustment_Status>{0}</APAR_Adjustment_Status>", request.APARAdjustment.APAR_Adjustment_Status ?? "");

            sb.Append("</APARAdjustment>");
            sb.Append("</APARAdjustmentDetails>");

            return sb.ToString();
        }

        private string BuildAPAREmployeeXml(List<APARAdjustmentEmployee> employees)
        {
            var sb = new StringBuilder();

            sb.Append("<APARAdjustmentDetails>");

            foreach (var item in employees)
            {
                sb.Append("<APARAdjustment>");

                sb.AppendFormat("<APARAdjustment_Id>{0}</APARAdjustment_Id>", item.APARAdjustment_Id);
                sb.AppendFormat("<Employee_Code>{0}</Employee_Code>", item.Employee_Code ?? "");
                sb.AppendFormat("<Ref_Id>{0}</Ref_Id>", item.Ref_Id ?? "");
                sb.AppendFormat("<APAR_Adjustment_Amount>{0}</APAR_Adjustment_Amount>", item.APAR_Adjustment_Amount);
                sb.AppendFormat("<APAR_Adjustment_Dates>{0}</APAR_Adjustment_Dates>",
                    item.APAR_Adjustment_Dates?.ToString("yyyy-MM-dd") ?? "");

                sb.Append("</APARAdjustment>");
            }

            sb.Append("</APARAdjustmentDetails>");

            return sb.ToString();
        }

        public async Task<APARAdjustmentUploadResponse> UploadAPARAdjustment(IFormFile file, string user)
        {
            APARAdjustmentUploadResponse response = new APARAdjustmentUploadResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                var dirPath = Path.Combine(_configuration["ClaimDocPath"], "APARAdjustmentUpload");

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(dirPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Read Excel
                DataTable dt = ReadExcelToDataTable(filePath);

                if (dt.Rows.Count == 0)
                {
                    response.response = "Excel sheet is empty.";
                    return response;
                }

                // Convert to XML
                dt.TableName = "Table";
                DataSet ds = new DataSet("NewDataSet");
                ds.Tables.Add(dt);

                string xmlInput;
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", user);

                string spName = "Upload_APARAdjustmentbulkApproval"; // ✅ MVC SP

                var res = await _dbRepository.GetItemsAsync(spName, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success"))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Upload completed with errors";
                        response.errors = res
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    response.response = "Failed";
                }
            }
            catch (Exception ex)
            {
                response.response = ex.Message;
            }

            return response;
        }

    }



}