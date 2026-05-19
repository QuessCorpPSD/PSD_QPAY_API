using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using Qzone.IRepository.SplitCulture;
using QZone.DTo.SplitCulture;
using System.Data;
using System.Text;
namespace Qzone.IRepository.SplitCulture
{
    public class SplitCultureRepository : ISplitCultureRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public SplitCultureRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }
        public Task<DataSet> SearchBankAdviceSplitCulture(SplitCultureSearchDto request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = request.Company_Id,
                ["@Bank_Culture_Id"] = (object?)request.Bank_Culture_Id ?? DBNull.Value,
                ["@Mode"] = request.Mode
            };

            var result = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "Search_Invoice_EditBankculture_Data",
                parameters,
                1500
            );

            if (result == null || result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                result = new DataSet();

            return Task.FromResult(result);
        }
        public Task<DataSet> GetInvoiceBankCompanywiseMapname(int companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId
            };

            var ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "GetInvoiceBank_CompanywiseMapname",
                parameters,
                1500
            );

            return Task.FromResult(ds ?? new DataSet());
        }
        public async Task<SplitCultureResponse> CreateInvoiceBankCulture(BankCultureRequestDto request)
        {

            SplitCultureResponse responseDetails = new SplitCultureResponse();
            var xml = ConvertToXml(request);           
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = request.Company_id,
                ["@Vendor_id"] = request.Vendor_id,
                ["@GroupDetail"] = xml,
                ["@Culture_Type"] = request.Culture_Type,
                ["@CreatedBy"] = request.CreatedBy,
                ["@Mode"] = request.Mode
            };

            var res = await this._dbRepository.GetItemsAsync("CreateInvoiceBankCulture", parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                    var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(message) && (message.Contains("successfully") ||
                        message.Contains("Successfully")))
                    {
                        responseDetails.response = message;
                    }
                    else
                    {
                        responseDetails.response = "Failed to import.";
                        responseDetails.errors = res
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    responseDetails.response = "Error while processing response.";
                }
            }
            else
            {
                responseDetails.response = "Failed";
            }

            return responseDetails;            

        }
        private string ConvertToXml(BankCultureRequestDto request)
        {
            var list = request?.BankCultureDetailsResponse?.BankCulture;

            if (request.Mode.Equals("Delete", StringComparison.OrdinalIgnoreCase))
            {
                return $"<Bank_Culture_Detail_id>{list?.FirstOrDefault()?.Bank_Culture_Detail_id}</Bank_Culture_Detail_id>";
            }

            var sb = new StringBuilder();

            
            sb.AppendFormat("<Bank_Culture_id>{0}</Bank_Culture_id>", request.Bank_Culture_id);
            sb.Append("<BankCultureDetailsResponse>");
            if (list != null)
            {
                foreach (var item in list)
                {
                    sb.Append("<BankCulture>");

                    sb.AppendFormat("<available>{0}</available>", item.available.ToString().ToLower());
                    sb.AppendFormat("<Bank_Culture_Detail_id>{0}</Bank_Culture_Detail_id>", item.Bank_Culture_Detail_id);
                    sb.AppendFormat("<Bank_Culture_id>{0}</Bank_Culture_id>", item.Bank_Culture_id);
                    sb.AppendFormat("<Company_Id>{0}</Company_Id>", item.Company_Id);
                    sb.AppendFormat("<Map_Name_Id>{0}</Map_Name_Id>", item.Map_Name_Id);
                    sb.AppendFormat("<Map_Name>{0}</Map_Name>", item.Map_Name);
                    sb.AppendFormat("<CreatedBy>{0}</CreatedBy>", item.CreatedBy);
                    sb.AppendFormat("<Culture_Type>{0}</Culture_Type>", item.Culture_Type);
                    sb.AppendFormat("<Group_Detail_Id>{0}</Group_Detail_Id>", item.Group_Detail_Id);
                    sb.AppendFormat("<SNo>{0}</SNo>", item.SNo);
                    sb.AppendFormat("<Vendor_Id>{0}</Vendor_Id>", item.Vendor_Id);

                    sb.Append("</BankCulture>");
                }
            }

            sb.Append("</BankCultureDetailsResponse>");

            return sb.ToString();
        }

        public async Task<SplitCultureResponse> UploadBankInvoiceSplit(IFormFile file, int CreatedBy)
        {
            var responseDetails = new SplitCultureResponse();
            string fileNameToSave = "";

            if (file != null && file.Length != 0)
            {
                var dirPath = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Bank Invoice", "SplitCulture");
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                string originalFileName = file.FileName.Replace(" ", "");
                string extension = Path.GetExtension(originalFileName);
                string nameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                fileNameToSave = Guid.NewGuid().ToString() + nameWithoutExt + extension;
                string filePath = Path.Combine(dirPath, fileNameToSave);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataSet ds = ExcelToDataSet(filePath);

                if (ds.Tables.Count == 0)
                {
                    responseDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return responseDetails;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    responseDetails.response = "Excel sheet is empty";
                    return responseDetails;
                }

                string xmlInput = BuildSplitCultureXml(ds.Tables[0]);

                var parameters = new DynamicParameters();
                parameters.Add("@Xml", xmlInput);
                parameters.Add("@Createdby", CreatedBy);

                var res = await _dbRepository.GetItemsAsync("SP_Bank_Invoice_Advice_Split_Culture_Upload", parameters);

                var result = JsonConvert.DeserializeObject<List<UploadResponse>>(res);
                var message = result?.FirstOrDefault()?.Validation;

                if (!string.IsNullOrWhiteSpace(res))
                {
                    if (message.Contains("success", System.StringComparison.OrdinalIgnoreCase))
                        responseDetails.response = message;
                    else
                    {
                        responseDetails.response = "Failed to import.";
                        responseDetails.errors = res
                            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
                            .ToList();
                    }
                }
                else
                {
                    responseDetails.response = "Failed";
                }
            }
            else
            {
                responseDetails.response = "File not found";
            }

            return responseDetails;
        }

        private string BuildSplitCultureXml(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return "<Root></Root>";

            var sb = new StringBuilder();
            sb.Append("<Root>");

            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<Table>");
                sb.AppendFormat("<Company_Code>{0}</Company_Code>", row["Company_Code"]);
                sb.AppendFormat("<Split_Type>{0}</Split_Type>", row["Split_Type"]);
                sb.AppendFormat("<Map_Name>{0}</Map_Name>", row["Map_Name"]);
                sb.Append("</Table>");
            }

            sb.Append("</Root>");
            return sb.ToString();
        }

        public static DataSet ExcelToDataSet(string filePath)
        {

            var dataSet = new DataSet();


            using var workbook = new XLWorkbook(filePath);

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
                            string columnName = cell.IsEmpty()
                                ? $"Column{cell.Address.ColumnNumber}"
                                : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {

                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(c => c.IsEmpty() ? string.Empty : c.GetValue<string>())
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
