using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Tools;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Tools
{
    public class DynamicUploadRepository : IDynamicUploadRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public DynamicUploadRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetUploadType(int? Upload_Type, int? UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Upload_Type"] = Upload_Type,
                ["@UserId"] = UserId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Dynamic_Upload_Type", parameters, 1500);
        }

        public async Task<DataSet> GetAllColumns(int? Upload_Type, int? UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Upload_Type"] = Upload_Type,
                ["@UserId"] = UserId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Dynamic_Upload_Type", parameters, 1500);
        }

        public async Task<ServiceChargeResponse> FileUpload(IFormFile file, [FromForm] int UploadTypeId, [FromForm] int CreatedBy)
        {
            //string ServiceChargeMaster,string ServiceChargeType,string SlabType, string SlabInnerType, int CreatedBy
            ServiceChargeResponse poDetails = new ServiceChargeResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Dynamic_Upload");

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

                DataSet ds = new DataSet("NewDataSet");
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

                string storeProcedure = @"Dynamic_Upload";
                var parameters = new DynamicParameters();

                xmlInput = Regex.Replace(
    xmlInput,
    @"\b(\d{2})-(\d{2})-(\d{4})\b",
    "$1/$2/$3");

                parameters.Add("@xml", xmlInput);
                parameters.Add("@Upload_Type_Id", UploadTypeId);
                parameters.Add("@IsAllowDuplicate", 0);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Validation ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Rows Uploaded Successfully", StringComparison.OrdinalIgnoreCase))
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

        //public static DataSet ExcelToDataSet(string filePath)
        //{
        //    using var workbook = new XLWorkbook(filePath);
        //    var dataSet = new DataSet();

        //    foreach (var worksheet in workbook.Worksheets)
        //    {
        //        var dataTable = new DataTable(worksheet.Name);
        //        bool firstRow = true;

        //        foreach (var row in worksheet.RowsUsed())
        //        {
        //            if (firstRow)
        //            {
        //                foreach (var cell in row.Cells())
        //                {
        //                    string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
        //                    dataTable.Columns.Add(columnName);
        //                }
        //                firstRow = false;
        //            }
        //            else
        //            {
        //                var values = row.Cells(1, dataTable.Columns.Count)
        //                                .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
        //                                .ToArray();

        //                dataTable.Rows.Add(values);
        //            }
        //        }

        //        dataSet.Tables.Add(dataTable);
        //    }

        //    return dataSet;
        //}

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
                            string rawName = cell.IsEmpty()
                                ? $"Column{cell.Address.ColumnNumber}"
                                : cell.GetValue<string>();

                            string columnName = Regex.Replace(rawName, @"[^a-zA-Z0-9_]", "");

                            // Avoid duplicate column names
                            if (dataTable.Columns.Contains(columnName))
                                columnName += "_" + cell.Address.ColumnNumber;

                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                            .Select(cell =>
                            {
                                if (cell.IsEmpty())
                                    return string.Empty;

                                // ✅ Date handling
                                if (cell.DataType == XLDataType.DateTime)
                                {
                                    var date = cell.GetDateTime();
                                    return date.ToString("dd-MM-yyyy");
                                }

                                return cell.GetValue<string>();
                            })
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
