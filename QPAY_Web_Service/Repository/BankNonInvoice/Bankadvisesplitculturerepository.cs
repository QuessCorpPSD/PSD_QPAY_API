using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.BankNonInvoice;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class Bankadvisesplitculturerepository : Ibankadvisesplitculturerepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public Bankadvisesplitculturerepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            _configuration = configuration;
                }
        public async Task<DataSet> getvendor(string? filter, int Company_id)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Fileter"]=filter,
                ["@Company_id"] = Company_id,
               
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("get_Isnoninvoice_Vendor_Name_Bank", parameters); ;

        }

        public async Task<DataSet> getgroupname(int? Company_id, int client_id)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_id"] = Company_id,
                ["@Client_id"] = client_id,

            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("getbank_groupnamebasedonvendor", parameters); ;

        }

        public async Task<DataSet> createbankadvisesplitculture(Bankadvisesplitculture payload)
        {
            
           
            string xml = ConvertWithDynamicRoot(payload.groupdetail, "BankCultureDetailsResponse", "BankCulture");

            var parameters = new Dictionary<string, object>
            {
                ["@Mode"] = payload.mode,
                ["@CreatedBy"] = payload.created_by,
                ["@Company_id"] = payload.Company_Id,
                ["@Vendor_id"] = payload.vendor_id,
                ["@GroupDetail"] = xml,
                ["@Culture_Type"] = payload.culture_type,
                ["@BankCulture_id"] = payload.Bank_Culture_id,

            };


            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("CreateBankCulture_New", parameters);
            
                        
        }

        public async Task<DataSet> getsearcheditdata(searcheditdata payload)
        {

            var parameters = new Dictionary<string, object>
            {
                ["@Mode"] = payload.mode,
               
                ["@Company_Id"] = payload.Company_Id,
                ["@Vendor_Id"] = payload.vendor_id,
                ["@Bank_Culture_Id"] = payload.bankcultureid,


            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("search_editbankculture_data", parameters);
        }

        public async Task<DataSet> getsearcheditdataExport(searcheditdata payload)
        {

            var parameters = new Dictionary<string, object>
            {
               // ["@Mode"] = payload.mode,

                ["@Company_Id"] = payload.Company_Id,
                ["@Vendor_Id"] = payload.vendor_id,
                ["@Bank_Culture_Id"] = payload.bankcultureid,


            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Batchgeneration", parameters);
        }
        public async Task<BulkUploadErrormessage> uploadbankadvisesplitculture(IFormFile file, int created_by)
        {
            BulkUploadErrormessage response = new BulkUploadErrormessage();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.Vaildation = "File not found";
                    return response;
                }

                var dirPath = Path.Combine(_configuration["ClaimDocPath"], "BankNonInvoice", "BanksplitCulture");

                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(dirPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ✅ SAME AS CLIENT ADVANCE → DataTable
                DataTable dt = ReadExcelToDataTable(filePath);

                if (dt.Rows.Count == 0)
                {
                    response.Vaildation = "Excel sheet is empty.";
                    return response;
                }

                // ✅ IMPORTANT: MATCH SP XML FORMAT
                dt.TableName = "Table";
                DataSet ds = new DataSet("NewDataSet");
                ds.Tables.Add(dt);

                string xmlInput = "";
                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);
                    xmlInput = sw.ToString();
                }

                // ✅ CALL SP (no hardcode changes — same as your current SP)
                var parameters = new DynamicParameters();
                parameters.Add("@xml", xmlInput);
                parameters.Add("@Createdby", created_by);

                var result = await _dbRepository.GetItemsAsync(
                    "SP_Bank_Advice_Split_Culture_Upload",
                    parameters
                );

                // ✅ SAME RESPONSE HANDLING AS CLIENT ADVANCE
                if (!string.IsNullOrWhiteSpace(result))
                {
                    if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
                    {
                        response.Vaildation = result;
                    }
                    else
                    {
                        response.Vaildation = "Failed to import.";
                        response.errors = result
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    response.Vaildation = "No response from server.";
                }
            }
            catch (Exception ex)
            {
                response.Vaildation = "Error occurred.";
                response.errors = new List<string> { ex.Message };
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
                            dt.Columns.Add(cell.Value.ToString().Trim());

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

        public static string ConvertWithDynamicRoot<T>(IEnumerable<T> list, string rootName, string tableName)
        {
            var root = new XElement(rootName);

            foreach (var item in list)
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StringWriter();
                serializer.Serialize(writer, item);

                var doc = XDocument.Parse(writer.ToString());
                root.Add(new XElement(tableName, doc.Root.Elements()));
            }

            return root.ToString();
        }
    }
}
