using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Process;
using QPay.DAL.Repository;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;
using static QPay.BAL.Repository.Process.ArrearAttendanceProcessRepository;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;

namespace QPay.BAL.Repository.Process
{
    public class LOPAdjustmentProcessRepository : ILOPAdjustmentProcessRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public LOPAdjustmentProcessRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(SearchLOPRequest searchRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = searchRequest.Company_id,
                ["@Pay_Period_Id"] = searchRequest.Pay_Frequency_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("spSearchLOPAdjustments", parameters, 1500);
        }

        public async Task<DataSet> ExporttoExcel(ExporttoExcelxml exporttoExcelRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = exporttoExcelRequest.Company_id,
                ["@Pay_Period_Id"] = exporttoExcelRequest.Pay_Frequency_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_SearchLOPAdjustmentsExportToExcel", parameters, 1500);
        }

        public async Task<DataSet> DeleteLOPAdjustment(string LOP_Adjustment_Id, string CreatedBy)
        {
            string xmlInput = "<LOPAdjustmentData><LOPAdjustment><LOP_Adjustment_Id>" + LOP_Adjustment_Id + "</LOP_Adjustment_Id></LOPAdjustment></LOPAdjustmentData>";
            var parameters = new Dictionary<string, object?>
            {
                ["@xmlInput"] = xmlInput,
                ["@xmlInputDetail"] = "''",
                ["@mode"] = "Delete",
                ["@CreatedBy"] = CreatedBy
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateLOPAdjustment", parameters, 1500);
        }

        public async Task<ProcessResponse> ImportLOPAdjustment(IFormFile file, [FromForm] string User)
        {
            ProcessResponse processDetails = new ProcessResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "LOPAdjustment_Process");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"LOPAdjustment_Process_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    processDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return processDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                using var xmlWriter2 = new StringWriter();

                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                dscolumns.WriteXml(xmlWriter2, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string xmlInput2 = xmlWriter2.ToString();

                string storeProcedure = "Proc_Upload_LOP_Adjustment_NewUI";
                var parameters = new DynamicParameters();

                parameters.Add("@CreatedBy", User);
                parameters.Add("@XML_File", xmlInput.Replace("Sheet1", "Table"));

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("Rows Import Successfully"))
                        {
                            processDetails.response = message;
                        }
                        else if (!string.IsNullOrWhiteSpace(message) && message.Contains("No rows to Upload"))
                        {
                            processDetails.response = message;
                        }
                        else if (!string.IsNullOrWhiteSpace(message) && message.Contains("Uploaded faild due to"))
                        {
                            processDetails.response = message;
                        }
                        else
                        {
                            processDetails.response = "Failed to import.";
                            processDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        processDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    processDetails.response = "Failed";
                }

            }
            else
            {
                processDetails.response = "File not found";
            }
            return processDetails;
        }

        public async Task<PoResponse> BulkPOCreate(IFormFile file, [FromForm] string flag,
          [FromForm] string CreatedBy)
        {
            PoResponse poDetails = new PoResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "PO", "MainPO", "BulkPO");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"BulkPO_{CreatedBy}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }

                DataSet dscolumns = new DataSet();
                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        // Replace PODate, StartDate, EndDate values with formatted string
                        row["PODate"] = DateTime.Parse(row["PODate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        row["StartDate"] = DateTime.Parse(row["StartDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                        row["EndDate"] = DateTime.Parse(row["EndDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                    }
                }

                using var xmlWriter = new StringWriter();
                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();

                string storeProcedure = "Proc_Main_PO_Upload";
                var parameters = new DynamicParameters();

                parameters.Add("@Action", flag);
                parameters.Add("@XmlData", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);


                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                WriteLog(res.ToString());
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {

                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;



                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully."))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch (Exception ex)
                    {
                        poDetails.response = ex.Message;
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }

            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
        }
        public void WriteLog(string message)
        {
            string filePath = _configuration["LogPath"].ToString() + "api-log.txt";// @"F:\Backup API\Logs\api-log.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // ensure folder exists
            File.AppendAllText(filePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }

    }
}
