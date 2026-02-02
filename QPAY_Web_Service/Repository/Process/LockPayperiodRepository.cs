using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dapper;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Process;
using QPay.DAL.Repository;
using static QPay.BAL.Repository.Process.ArrearAttendanceProcessRepository;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.Repository.Process
{
    public class LockPayperiodRepository : ILockPayperiodRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public LockPayperiodRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> SearchDetails(SearchLockPayperiodRequest searchRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@payperiod"] = searchRequest.PayPeriod
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetLockPayPeriodNew", parameters, 1500);
        }

        public async Task<DataSet> ExporttoExcel(SearchLockPayperiodRequest exporttoExcelRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@PayPeriodId"] = exporttoExcelRequest.PayPeriod
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetLockPayPeriodNewExporttoexcel", parameters, 1500);
        }

        public async Task<ProcessResponse> ImportLockpayperiod(IFormFile file, [FromForm] string User)
        {
            ProcessResponse processDetails = new ProcessResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = System.IO.Path.Combine(_configuration["ClaimDocPath"].ToString(), "LockPayPeriod_Process");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = System.IO.Path.GetFileName(file.FileName);
                var extension = System.IO.Path.GetExtension(originalFileName);
                var newFileName = $"LockPayPeriod_Process_{datePrefix}{extension}";

                var filePath = System.IO.Path.Combine(uploadsFolder, newFileName);

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

                string storeProcedure = "Proc_Upload_LockPayPeriod";
                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput.Replace("Sheet1", "Table"));
                parameters.Add("@CreatedBy", User);


                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("Rows Uploaded Successfully."))
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

        public async Task<ProcessResponse> Lock(string xml, string User)
        {
            ProcessResponse processDetails = new ProcessResponse();

            string storeProcedure = "sp_CreateUpdateLockPayperiodinPayfrequency";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xml);
            parameters.Add("@mode", "Add");
            parameters.Add("@CreatedBy", User);


            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (message != "")
                    {
                        processDetails.response = message;
                    }
                    else
                    {
                        processDetails.response = "Failed.";
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
            return processDetails;
        }

    }
}
