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
using System.Xml;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class ReIssueApprove : IReIssueApprove
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public ReIssueApprove(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        public async Task<DataSet> SearchReIssueApprove(int CompanyId, int PayPeriodId, string ReIssueTypes, int? PaytypeId, string vPayperiods, string Status)
        {
            try
            {
                string xmlPayPeriods = string.Empty;

                if (!string.IsNullOrEmpty(vPayperiods))
                {
                    XmlDocument objXml = JsonConvert.DeserializeXmlNode(
                        "{\"vPayPeriod\":" + vPayperiods + "}",
                        "Bankinvoice");

                    xmlPayPeriods = objXml.InnerXml;
                }

                var parameters = new Dictionary<string, object?>
                {
                    ["@CompanyID"] = CompanyId,
                    ["@PayPeriodID"] = PayPeriodId,
                    ["@ReIssueTypes"] = ReIssueTypes,
                    ["@Blank"] = PaytypeId,
                    ["@XML_File"] = xmlPayPeriods,
                    ["@Status"] = Status
                };

                return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                    "sp_BankInvoiceSearchReissueApprove",
                    parameters,
                    1500);
            }
            catch (Exception ex)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("Table");
                dt.Columns.Add("ErrorMessage");
                dt.Rows.Add(ex.Message);
                ds.Tables.Add(dt);
                return ds;
            }
        }

        public async Task<DataSet> GetDropdown(string flag)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = flag
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "SP_BankInvoiceDropDownsBind",
                parameters,
                1500);
        }

        public async Task<ReIssueApproveUploadResponse> ReissueProcessApproveBulkUpload(IFormFile file, string User)
        {
            ReIssueApproveUploadResponse response = new ReIssueApproveUploadResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                var dirName = Path.Combine(_configuration["ClaimDocPath"].ToString(), "ReissueProcessApprove");

                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(dirName, fileName);

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

                string xmlInput = string.Empty;
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", Convert.ToInt32(User));

                var res = await _dbRepository.GetItemsAsync(
                    "Proc_Upload_BankInvoiceReissueProcessApproveReject",
                    parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success"))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Failed to import.";
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
                var worksheet = workbook.Worksheet(1);
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
                            if (i < dt.Columns.Count)
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString().Trim();
                            }
                            i++;
                        }
                    }
                }
            }

            return dt;
        }

        public async Task<DataSet> ExportToExcel(ReIssueApproveExportRequest payload)
        {
            string xmlPayPeriods = string.Empty;

            if (!string.IsNullOrEmpty(payload.vPayPeriods))
            {
                XmlDocument objXml = JsonConvert.DeserializeXmlNode(
                    "{\"vPayPeriod\":" + payload.vPayPeriods + "}",
                    "Bankinvoice");

                xmlPayPeriods = objXml.InnerXml;
            }

            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyID"] = payload.CompanyId,
                ["@PayPeriodID"] = payload.PayPeriodId,
                ["@XML_File"] = xmlPayPeriods,
                ["@ReIssueTypes"] = payload.ReissueTypeId,
                ["@Status"] = payload.Status
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_BankInvoiceSearchReissueApprove_ExportToExcel",
                parameters,
                1500
            );
        }

        private string BuildReIssueApproveRejectXml(ReIssueApproveRejectRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<ReIssueProcessResponse>");

            foreach (var item in request.Groupdetail)
            {
                sb.Append("<ReIssueProcessDetail>");
                sb.AppendFormat("<SNo>{0}</SNo>", item.SNo);
                sb.AppendFormat("<Bank_Invoice_Id>{0}</Bank_Invoice_Id>", item.Bank_Invoice_Id);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", item.Company_Id);
                sb.AppendFormat("<Company_Code>{0}</Company_Code>", item.Company_Code ?? "");
                sb.AppendFormat("<Company_Name>{0}</Company_Name>", item.Company_Name ?? "");
                sb.AppendFormat("<BatchId>{0}</BatchId>", item.BatchId ?? "");
                sb.AppendFormat("<Pay_Period_Id>{0}</Pay_Period_Id>", item.Pay_Period_Id);
                sb.AppendFormat("<Pay_Period>{0}</Pay_Period>", item.Pay_Period ?? "");
                sb.AppendFormat("<Employee_Id>{0}</Employee_Id>", item.Employee_Id);
                sb.AppendFormat("<Correct_Employee_Name>{0}</Correct_Employee_Name>", item.Correct_Employee_Name ?? "");
                sb.AppendFormat("<Employee_Code>{0}</Employee_Code>", item.Employee_Code ?? "");
                sb.AppendFormat("<Update_Bank_Name>{0}</Update_Bank_Name>", item.Update_Bank_Name ?? "");
                sb.AppendFormat("<Update_Bank_Acctno>{0}</Update_Bank_Acctno>", item.Update_Bank_Acctno ?? "");
                sb.AppendFormat("<Update_IFSC_Code>{0}</Update_IFSC_Code>", item.Update_IFSC_Code ?? "");
                sb.AppendFormat("<Cheque_Number>{0}</Cheque_Number>", item.Cheque_Number ?? "");
                sb.AppendFormat("<Cheque_Amount>{0}</Cheque_Amount>", item.Cheque_Amount);
                sb.AppendFormat("<PayMode_Id>{0}</PayMode_Id>", item.PayMode_Id);
                sb.AppendFormat("<Pay_Mode>{0}</Pay_Mode>", item.Pay_Mode ?? "");
                sb.AppendFormat("<ReIssueType_Id>{0}</ReIssueType_Id>", item.ReIssueType_Id);
                sb.AppendFormat("<ReIssueType>{0}</ReIssueType>", item.ReIssueType ?? "");
                sb.AppendFormat("<Remarks>{0}</Remarks>", item.Remarks ?? "");
                sb.Append("</ReIssueProcessDetail>");
            }

            sb.Append("</ReIssueProcessResponse>");
            return sb.ToString();
        }

        public async Task<ReIssueApproveRejectResponse> CreateReIssueApproveReject(ReIssueApproveRejectRequest request)
        {
            ReIssueApproveRejectResponse response = new ReIssueApproveRejectResponse();

            if (request == null || request.Groupdetail == null || !request.Groupdetail.Any())
            {
                response.response = "Invalid request.";
                return response;
            }

            try
            {
                var xmlInput = BuildReIssueApproveRejectXml(request);

                string storeProcedure = "sp_BankInvoiceCreateUpdateReissueRequest";

                var parameters = new DynamicParameters();
                parameters.Add("@xmlInput", xmlInput);
                parameters.Add("@Createdby", request.userId);
                parameters.Add("@mode", request.mode);

                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.Contains("Successfully") || res.Contains("successfully") || res.Contains("Success") || res.Contains("success"))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response = "Failed to " + request.mode;
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