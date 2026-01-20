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
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.Repository.Process
{
    public class PayRegisterUploadRepository : IPayRegisterUploadRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public PayRegisterUploadRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> DownloadTemplate(SearchPayRegisterRequest searchRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = searchRequest.Company_id,
                ["@Pay_Period_Id"] = searchRequest.Pay_Frequency_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_PayregisterUploadFormat", parameters, 1500);
        }

        public async Task<DataSet> ExporttoExcel(SearchPayRegisterRequest exporttoExcelRequest)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = exporttoExcelRequest.Company_id,
                ["@Pay_Period_Id"] = exporttoExcelRequest.Pay_Frequency_Id
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_PayregisteruploadexporttoExcel", parameters, 1500);
        }

        public async Task<ProcessResponse> ImportPayRegister(IFormFile file, [FromForm] string User)
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

                DataSet ds0 = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                DataSet ds4 = new DataSet();
                DataSet ds5 = new DataSet();

                string XmlPayregister = string.Empty;
                string XmlInvestment = string.Empty;
                string XmlHra = string.Empty;
                string XmlLta = string.Empty;
                string XmlIncome = string.Empty;
                string Xmlprevious = string.Empty;


                System.Data.DataTable dtToSerilize = new System.Data.DataTable();
                dtToSerilize = ds.Tables[0];

                System.Data.DataTable dtToSerilize1 = new System.Data.DataTable();
                dtToSerilize1 = ds.Tables[1];

                System.Data.DataTable dtToSerilize2 = new System.Data.DataTable();
                dtToSerilize2 = ds.Tables[2];

                System.Data.DataTable dtToSerilize3 = new System.Data.DataTable();
                dtToSerilize3 = ds.Tables[3];

                System.Data.DataTable dtToSerilize4 = new System.Data.DataTable();
                dtToSerilize4 = ds.Tables[4];

                System.Data.DataTable dtToSerilize5 = new System.Data.DataTable();
                dtToSerilize5 = ds.Tables[5];


                string[] hra = (from DataColumn x in ds.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                string[] income = (from DataColumn x in ds1.Tables[0].Columns select x.ColumnName.Trim()).ToArray();

                string[] invest = (from DataColumn x in ds2.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                string[] lta = (from DataColumn x in ds3.Tables[0].Columns select x.ColumnName.Trim()).ToArray();


                string[] payregister = (from DataColumn x in ds4.Tables[0].Columns select x.ColumnName.Trim()).ToArray();
                string[] previous = (from DataColumn x in ds5.Tables[0].Columns select x.ColumnName.Trim()).ToArray();

                using (StringWriter ms = new StringWriter())
                {
                    dtToSerilize.WriteXml(ms);
                    XmlHra = ms.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "").Replace("_x0029_", "").Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                }
                using (StringWriter ms1 = new StringWriter())
                {
                    dtToSerilize1.WriteXml(ms1);
                    XmlIncome = ms1.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "").Replace("_x0029_", "").Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                }

                using (StringWriter ms2 = new StringWriter())
                {
                    dtToSerilize2.WriteXml(ms2);
                    XmlInvestment = ms2.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "").Replace("_x0029_", "").Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                }
                using (StringWriter ms3 = new StringWriter())
                {
                    dtToSerilize3.WriteXml(ms3);
                    XmlLta = ms3.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "").Replace("_x0029_", "").Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                }

                using (StringWriter ms4 = new StringWriter())
                {
                    dtToSerilize4.WriteXml(ms4);
                    XmlPayregister = ms4.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "").Replace("_x0029_", "").Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                }
                using (StringWriter ms5 = new StringWriter())
                {
                    dtToSerilize5.WriteXml(ms5);
                    Xmlprevious = ms5.ToString().Replace("_x0020_", "").Replace("_x0028_", "").Replace("_x002F_", "").Replace("_x0029_", "").Replace("_x0027_", "").Replace("_x003A_", "").Replace("_x0023_", "");
                }

                string storeProcedure = "Sp_UploadPayregister";
                var parameters = new DynamicParameters();

                parameters.Add("@CreatedBy", User);
                parameters.Add("@XMLPayRegister", XmlPayregister);
                parameters.Add("@XMLInvestment", XmlInvestment);
                parameters.Add("@XMLHRA", XmlHra);
                parameters.Add("@XMLLTA", XmlLta);
                parameters.Add("@XMLILOHP", XmlIncome);
                parameters.Add("@XMLPE", Xmlprevious);
                

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
    }
}
