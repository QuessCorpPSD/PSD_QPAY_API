using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.TaxAndSaving;
using QPay.UI.Common;
using QPay.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Billing.GenericUpload;

namespace QPay.BAL.Repository
{
    public class LTACalculationRepository : ILTACalculationRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public LTACalculationRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> Search(int? companyId, int? EmployeeId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Employee_Id"] = EmployeeId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllLTACalculation", parameters, 1500);
        }

        public async Task<DataSet> GetLTABlockPayPeriod()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@LTABlockID"] = 0,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllLTABlocks", parameters, 1500);
        }

        
        public async Task<DataSet> Create(LTACalculationRequest items)
        {
            string parentdata = JsonConvert.SerializeObject(items.parentDetail);
            string childdata = JsonConvert.SerializeObject(items.childDetail);

            LTACalculation lTACalculation = JsonConvert.DeserializeObject<LTACalculation>(parentdata);
            LTACalculationDetail[] lTACalculationDetail = JsonConvert.DeserializeObject<LTACalculationDetail[]>(childdata);
            var lTACalculationResponse = new LTACalculationResponse();
            lTACalculationResponse.LTACalculations = new LTACalculation[1];
            lTACalculationResponse.LTACalculations[0] = lTACalculation;

            var lTACalculationDetailResponse = new LTACalculationDetailResponse();
            lTACalculationDetailResponse.LTACalculationDetails = lTACalculationDetail;
            string lTACalculationResponseSerialize = GenericSerializer<LTACalculationResponse>.Serialize(lTACalculationResponse);
            string lTACalculationDetailResponseSerialize = GenericSerializer<LTACalculationDetailResponse>.Serialize(lTACalculationDetailResponse);
            lTACalculationResponseSerialize = lTACalculationResponseSerialize == "<LTACalculationDetailResponse />" ? "<LTACalculationDetailResponse></LTACalculationDetailResponse>" : lTACalculationResponseSerialize;


            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = lTACalculationResponseSerialize,
                ["@xmlInputDetail"] = lTACalculationDetailResponseSerialize,
                ["@mode"] = items.mode,
                ["@CreatedBy"] = items.createdBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdateLTACalculation", parameters);
        }

        public async Task<RequestResponse> Upload(IFormFile file, [FromForm] int createdBy)
        {
            RequestResponse poDetails = new RequestResponse();
            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "LTACalculation");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);

                var filePath = Path.Combine(uploadsFolder, originalFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string strXml = WriteXmlFromExcel.ReadFileFromExcel(filePath, "DynamicUpload");
                if (strXml.Contains("Columns Name are not matching") == true)
                {
                    poDetails.response = "Columns Name are not matching please upload valid template";
                    return poDetails;
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
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();

                string storeProcedure = @"Upload_LTACalculation";
                var parameters = new DynamicParameters();
                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", createdBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<GenericUploadResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Record(s) Inserted Successfully!", StringComparison.OrdinalIgnoreCase))
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
                    catch
                    {
                        poDetails.response = "Error while processing response.";
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

        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }


    }
}
