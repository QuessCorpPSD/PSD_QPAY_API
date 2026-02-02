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
using static QPay.BAL.Repository.Process.ArrearAttendanceProcessRepository;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.Repository.Process
{
    public class AttendanceBatchIdUpdateRepository : IAttendanceBatchIdUpdateRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public AttendanceBatchIdUpdateRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<ProcessResponse> ImportAttendanceBatchIdUpdate(IFormFile file, [FromForm] string User)
        {
            ProcessResponse processDetails = new ProcessResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "AttendanceBatchIdUpdate_Process");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"AttendanceBatchIdUpdate_{datePrefix}{extension}";

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

                string storeProcedure = "Proc_Upload_AttendanceBatchIdUpdate";
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

                        if (!string.IsNullOrWhiteSpace(message) && message.Contains("Row(s) Uploaded Successfully."))
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
    }
}

