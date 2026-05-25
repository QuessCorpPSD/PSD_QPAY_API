using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.IBankNonInvoice;
using QPay.DAL.Repository;
using QPay.UI.Models.BankNonInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.BankNonInvoice
{
    public class BankAdviceSplitCultureRepository : IBankAdviceSplitCultureRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public BankAdviceSplitCultureRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }

        public async Task<DataSet> GetVendorname(string filter, int Company_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Fileter"] = filter,
                ["@Company_id"] = Company_id
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Get_IsNoninvoice_Vendor_Name_Bank", parameters, 1500);
        }

        public async Task<DataSet> GetSearchEditdata(int Company_id, int Vendor_id, int Bank_Culture_Id, string Mode)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = Company_id,
                ["@Vendor_Id"] = Vendor_id,
                ["@Bank_Culture_Id"] = Bank_Culture_Id,
                ["@Mode"] = Mode
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Search_EditBankculture_Data", parameters, 1500);
        }

        public async Task<BankAdviceSplitCultureUploadResponse>
    BankSplitCultureupload(
        IFormFile file,
        int CreatedBy)
        {
            BankAdviceSplitCultureUploadResponse response =
                new BankAdviceSplitCultureUploadResponse();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.response =
                        "File not found";

                    return response;
                }

                var dirName = Path.Combine(
                    _configuration["ClaimDocPath"]
                        .ToString(),
                    "BankSplitCultureUpload");

                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(file.FileName);

                string filePath =
                    Path.Combine(dirName, fileName);

                using (var stream =
                    new FileStream(
                        filePath,
                        FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // READ EXCEL
                DataTable dt =
                    ReadExcelToDataTable(filePath);

                if (dt.Rows.Count == 0)
                {
                    response.response =
                        "Excel sheet is empty.";

                    return response;
                }

                // XML CONVERSION
                dt.TableName = "Table";

                DataSet ds =
                    new DataSet("NewDataSet");

                ds.Tables.Add(dt);

                string xmlInput = "";

                using (var sw = new StringWriter())
                {
                    ds.WriteXml(sw);

                    xmlInput = sw.ToString();
                }

                var parameters =
                    new DynamicParameters();

                parameters.Add("@xml", xmlInput);

                parameters.Add(
                    "@CreatedBy",
                    CreatedBy);

                var res =
                    await _dbRepository.GetItemsAsync(
                        "SP_Bank_Advice_Split_Culture_Upload",
                        parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (res.ToLower().Contains("success"))
                    {
                        response.response = res;
                    }
                    else
                    {
                        response.response =
                            "Failed to import.";

                        response.errors =
                            res?.Split(
                                new[] { '\r', '\n' },
                                StringSplitOptions.RemoveEmptyEntries)
                            .ToList()

                            ?? new List<string>
                            {
                        "Unknown error."
                            };
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

        private string BuildBankCultureXml(
     CreateBankCultureRequest request)
        {
            var sb = new StringBuilder();

            sb.Append("<BankCultureDetailsResponse>");

            foreach (var item in request.Data)
            {
                sb.Append("<Bank_Advice_Split_Culture>");

                sb.AppendFormat(
                    "<Bank_Culture_Detail_id>{0}</Bank_Culture_Detail_id>",
                    item.Bank_Culture_Detail_id);

                sb.AppendFormat(
                    "<Bank_Culture_id>{0}</Bank_Culture_id>",
                    item.Bank_Culture_id);

                sb.AppendFormat(
                    "<Company_Id>{0}</Company_Id>",
                    item.Company_Id);

                sb.AppendFormat(
                    "<Vendor_Id>{0}</Vendor_Id>",
                    item.Vendor_Id);

                sb.AppendFormat(
                    "<Group_Detail_Id>{0}</Group_Detail_Id>",
                    item.Group_Detail_Id);

                sb.AppendFormat(
                    "<available>{0}</available>",
                    item.available);

                sb.AppendFormat(
                    "<Culture_Type>{0}</Culture_Type>",
                    item.Culture_Type);

                sb.Append("</Bank_Advice_Split_Culture>");
            }

            sb.Append("</BankCultureDetailsResponse>");

            return sb.ToString();
        }
        public async Task<BankCultureResponse>
    CreateBankCulture(
        CreateBankCultureRequest request)
        {
            BankCultureResponse response =
                new BankCultureResponse();

            try
            {
                if (request == null
                    || request.Data == null
                    || !request.Data.Any())
                {
                    response.Error_Message =
                        "Invalid request.";

                    response.Status = 0;

                    return response;
                }

                string xmlInput = "";

                // DELETE MODE
                if (request.Mode == "Delete")
                {
                    var firstItem =
                        request.Data.First();

                    xmlInput =
                        $"<Bank_Culture_Detail_id>{firstItem.Bank_Culture_Detail_id}</Bank_Culture_Detail_id>";
                }
                else
                {
                    xmlInput =
                        BuildBankCultureXml(request);
                }

                var firstData =
                    request.Data.First();

                string storeProcedure =
                    "CreateBankCulture";

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@Company_id",
                    firstData.Company_Id);

                parameters.Add(
                    "@Vendor_id",
                    firstData.Vendor_Id);

                parameters.Add(
                    "@GroupDetail",
                    xmlInput);

                parameters.Add(
                    "@Culture_Type",
                    firstData.Culture_Type);

                parameters.Add(
                    "@CreatedBy",
                    request.CreatedBy);

                parameters.Add(
                    "@Mode",
                    request.Mode);

                var res =
                    await _dbRepository.GetItemsAsync(
                        storeProcedure,
                        parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        // SUCCESS
                        if (
                            res.Contains("Successfully")
                            || res.Contains("successfully")
                            || res.Contains("Success")
                        )
                        {
                            response.Status = 1;

                            response.Error_Message = res;
                        }
                        else
                        {
                            response.Status = 0;

                            response.Error_Message = res;
                        }
                    }
                    catch
                    {
                        response.Status = 0;

                        response.Error_Message =
                            "Error while processing response.";
                    }
                }
                else
                {
                    response.Status = 0;

                    response.Error_Message =
                        "Failed";
                }
            }
            catch (Exception ex)
            {
                response.Status = 0;

                response.Error_Message =
                    ex.Message;
            }

            return response;
        }
        public async Task<DataSet> Getgroupname(int Company_id, int Client_id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = Company_id,
                ["@Client_id"] = Client_id
            };

            return _dbRepository
                .ExecuteStoredProcedureToDataSetAsync(
                    "GetBank_GroupnameBasedonVendor",
                    parameters,
                    1500);
        }
    }
}
