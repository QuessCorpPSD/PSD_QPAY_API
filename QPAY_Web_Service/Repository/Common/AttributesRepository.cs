using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Common;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.AttributesClass;

namespace QPay.IRepository.Repository.Common
{
    public class AttributesRepository : IAttributesRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly ICommonRepository _icommon;
        private readonly IConfiguration _configuration;

        public AttributesRepository(DbRepository dbRepository, ICommonRepository icommon, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._icommon = icommon;
            _configuration = configuration;
        }

        public async Task<DataSet> GetAttributes()
        {
            var parameters = new Dictionary<string, object?>
            {
               
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_OMSHEADERS", parameters, 1500);
        }


        public async Task<List<AttributeUI>> GetAllAttribute(AttributeUI attributeUI)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", attributeUI.id);
            parameter.Add("@AttributeName", attributeUI.AttributeName);
            parameter.Add("@Isactive", attributeUI.IsActive);
            parameter.Add("@CreatedBy", attributeUI.CreatedBy);
            parameter.Add("@ActionType", attributeUI.ActionType);
            parameter.Add("@CompanyId", attributeUI.CompanyId);

            var res = await _dbRepository.GetItemsAsync("SP_tbl_Attributes_AddUpdate", parameter);

            if (res != null)
            {
                var attribute = JsonConvert.DeserializeObject<List<AttributeUI>>(res);
                return attribute;
            }
            else
            {
                return new List<AttributeUI>();
            }
        }

        public async Task<AttributesResponse> UploadAttributesData(IFormFile file, [FromForm] string User,
          [FromForm] string companyCode, [FromForm] string OfferId)
        {
            AttributesResponse AttributesDetails = new AttributesResponse();

            if (file != null && file.Length != 0)
            {
               

              

                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Attributes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Attributes_{companyCode}_{datePrefix}{extension}";

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
                    AttributesDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return AttributesDetails;
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

                string storeProcedure = "USP_Harbour_To_OMS";
                var parameters = new DynamicParameters();

              
                parameters.Add("@User", User);
                parameters.Add("@xml", xmlInput);
                parameters.Add("@OfferId", OfferId);

                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(res) && res.Contains("Rows Inserted Successfully", StringComparison.OrdinalIgnoreCase))
                        {
                            AttributesDetails.response = res;
                        }
                        else
                        {
                            AttributesDetails.response = "Failed to import.";
                            AttributesDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        AttributesDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    AttributesDetails.response = "Failed";
                }

            }
            else
            {
                AttributesDetails.response = "File not found";
            }
            return AttributesDetails;
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

        public async Task<DataSet> GetAttributeTemplate(string xml)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Xml"] = xml

            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetProvisionalAttributeTemplate", parameters, 1500);
        }
    }
      
}
