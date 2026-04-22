using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.AccountReceivableMod.TDSSlabModels;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.Repository.AccountReceivableSer
{
    public class TDSSlab : ITDSSlab
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public TDSSlab(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }
        public async Task<DataSet> GetFinancialYear(int? financialYearId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@FinancialYearId"] = financialYearId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetFinancialYear",
                parameters,
                1500
            );
        }
        public async Task<DataSet> Search(int? CompanyId, int? FinancialYearId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = CompanyId,         
                ["@Finacial_Year_id"] = FinancialYearId 
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_Bank_Invoice_GetAllClientTdsMasterDetails",
                parameters,
                1500
            );
        }
        public async Task<DataSet> ExportToExcel(CommonExport2 payload)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = payload.CompanyId,
                ["@Finacial_Year_id"] = payload.FinancialYearId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_Bank_Invoice_GetAllClientTdsMasterDetailsExportToExcel",
                parameters,
                1500
            );
        }
        public async Task<ClientAdvancePaymentResponse> UploadTDSSlab(IFormFile file, string createdBy)
        {
            ClientAdvancePaymentResponse response = new ClientAdvancePaymentResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                var dirName = Path.Combine(_configuration["ClaimDocPath"].ToString(), "TDSSlab");

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
                DataSet xmlDataSet = new DataSet("NewDataSet");
                xmlDataSet.Tables.Add(dt);

                string xmlInput = string.Empty;
                using (var sw = new StringWriter())
                {
                    xmlDataSet.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                var parameters = new Dictionary<string, object?>
                {
                    ["@xmlInput"] = xmlInput,
                    ["@mode"] = "Upload",
                    ["@CreatedBy"] = createdBy
                };

                DataSet ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                    "USP_Bank_Invoice_CreateUpdateTDSSlabMaster",
                    parameters,
                    1500
                );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var errors = ds.Tables[0].AsEnumerable()
                        .Select(x => x["Error_Message"]?.ToString())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();

                    if (errors.Any())
                    {
                        response.response = "Upload completed with errors.";
                        response.errors = errors;
                    }
                    else
                    {
                        response.response = "Upload completed successfully.";
                    }
                }
                else
                {
                    response.response = "Upload completed successfully.";
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
                        DataRow dr = dt.NewRow();
                        int i = 0;

                        foreach (var cell in row.Cells(1, dt.Columns.Count))
                        {
                            dr[i] = cell.Value.ToString().Trim();
                            i++;
                        }

                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }
        private string BuildTdsSlabXml(TdsSlabSaveRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<TdsDetails>");

            foreach (var item in request.TdsDetails)
            {
                sb.Append("<ClientTdsSlabMaster>");
                sb.AppendFormat("<TdsSlabMaster_Id>{0}</TdsSlabMaster_Id>", item.TdsSlabMaster_Id);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", item.Company_Id);
                sb.AppendFormat("<Company_Name>{0}</Company_Name>", item.Company_Name ?? "");
                sb.AppendFormat("<Company_Code>{0}</Company_Code>", item.Company_Code ?? "");
                sb.AppendFormat("<TypeOfSelection>{0}</TypeOfSelection>", item.TypeOfSelection);
                sb.AppendFormat("<Value>{0}</Value>", item.Value);
                sb.AppendFormat("<Percentage>{0}</Percentage>", item.Percentage);
                sb.AppendFormat("<FromDate>{0}</FromDate>", item.FromDate ?? "");
                sb.AppendFormat("<ToDate>{0}</ToDate>", item.ToDate ?? "");
                sb.AppendFormat("<NatureOfBusiness>{0}</NatureOfBusiness>", item.NatureOfBusiness ?? "");
                sb.AppendFormat("<TAN>{0}</TAN>", item.TAN ?? "");
                sb.AppendFormat("<PAN>{0}</PAN>", item.PAN ?? "");
                sb.AppendFormat("<Client_Id>{0}</Client_Id>", item.Client_Id);
                sb.AppendFormat("<Serial_No>{0}</Serial_No>", item.Serial_No);
                sb.AppendFormat("<Financial_Year_Id>{0}</Financial_Year_Id>", item.Financial_Year_Id);
                sb.AppendFormat("<Financial_Year_Name>{0}</Financial_Year_Name>", item.Financial_Year_Name ?? "");
                sb.Append("</ClientTdsSlabMaster>");
            }

            sb.Append("</TdsDetails>");
            return sb.ToString();
        }
        public async Task<UploadResponse> UploadLTDSSlab(IFormFile file, int userId)
        {
            UploadResponse response = new UploadResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response = "File not found";
                    return response;
                }

                var dirPath = Path.Combine(_configuration["ClaimDocPath"].ToString(), "LTDSSlab");

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

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

                DataSet xmlDataSet = new DataSet("NewDataSet");
                xmlDataSet.Tables.Add(dt);

                string xmlInput = string.Empty;

                using (var sw = new StringWriter())
                {
                    xmlDataSet.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                var parameters = new Dictionary<string, object?>
                {
                    ["@xmlInput"] = xmlInput,
                    ["@mode"] = "UploadLTDS",
                    ["@CreatedBy"] = userId
                };

                DataSet ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                    "USP_Bank_Invoice_CreateUpdateTDSSlabMaster",
                    parameters,
                    1500
                );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string message = Convert.ToString(row["Error_Message"]);

                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            if (message.ToLower().Contains("success"))
                            {
                                response.response = message;
                            }
                            else
                            {
                                response.errors.Add(message);
                            }
                        }
                    }

                    if (response.errors.Count > 0)
                    {
                        response.response = "Upload completed with errors.";
                    }
                    else if (string.IsNullOrWhiteSpace(response.response))
                    {
                        response.response = "Upload completed successfully.";
                    }
                }
                else
                {
                    response.response = "Upload completed successfully.";
                }
            }
            catch (Exception ex)
            {
                response.response = ex.Message;
            }

            return response;
        }
        public async Task<TdsSlabSaveResponse> TdsSlabCreate(TdsSlabSaveRequest request)
        {
            TdsSlabSaveResponse response = new TdsSlabSaveResponse();

            try
            {
                if (request == null || request.TdsDetails == null || !request.TdsDetails.Any())
                {
                    response.response = "Invalid request.";
                    return response;
                }

                string xmlInput = BuildTdsSlabXml(request);

                var parameters = new Dictionary<string, object?>
                {
                    ["@xmlInput"] = xmlInput,
                    ["@mode"] = request.action,
                    ["@CreatedBy"] = request.userId
                };

                DataSet ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                    "USP_Bank_Invoice_CreateUpdateTDSSlabMaster",
                    parameters,
                    1500
                );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var errors = ds.Tables[0].AsEnumerable()
                        .Select(x => x["Error_Message"]?.ToString())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();

                    if (errors.Any())
                    {
                        response.response = "Completed with errors.";
                        response.errors = errors;
                    }
                    else
                    {
                        response.response = "Completed successfully.";
                    }
                }
                else
                {
                    response.response = "Completed successfully.";
                }
            }
            catch (Exception ex)
            {
                response.response = ex.Message;
            }

            return response;
        }
        public async Task<List<CompanyNameByCodeResult>> GetCompanyNameByCode(string companyCode)
        {
            List<CompanyNameByCodeResult> result = new List<CompanyNameByCodeResult>();

            try
            {
                var parameters = new Dictionary<string, object?>
                {
                    ["@CompanyCode"] = companyCode
                };

                DataSet ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                    "sp_GetClientNamesByClientCode",
                    parameters,
                    1500
                );

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        result.Add(new CompanyNameByCodeResult
                        {
                            Client_Id = row["Client_Id"] != DBNull.Value ? Convert.ToInt32(row["Client_Id"]) : 0,
                            Client_Code = row["Client_Code"]?.ToString(),
                            Company_Name = row["Company_Name"]?.ToString()
                        });
                    }
                }
            }
            catch
            {
                return new List<CompanyNameByCodeResult>();
            }

            return result;
        }
    }
}
