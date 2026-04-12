using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Customer;
using System.Data;
using System.Text;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.BAL.Repository.Customer
{
    public class VendorServiceChargeRepository : IVendorServiceChargeRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public VendorServiceChargeRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
       
      
     
        public async Task<VendorServiceChargeResponse> Create(VendorServiceChargeRequest request)
        {
            VendorServiceChargeResponse serviceresponse = new VendorServiceChargeResponse();

            if (request == null || request.VendorServiceChargemaster == null || !request.VendorServiceChargemaster.Any())
            {
                serviceresponse.response = "Invalid request.";
            }

            var xmlInput = BuildServiceChargeXml(request);

            string storeProcedure = "sp_CreateUpdate_VendorServiceCharge";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@CreatedBy", request.Created_By);
            parameters.Add("@Company_ID", request.CompanyId);
            parameters.Add("@mode", request.Mode);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            string? msg = null;

            if (!string.IsNullOrWhiteSpace(res))
            {
                var arr = JArray.Parse(res);
                msg = arr[0]?["Error_Message"]?.ToString();
            }
            if (!string.IsNullOrWhiteSpace(msg))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(msg) && (msg.Contains("Service Charge Created Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Service Charge Updated Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Service Charge Deleted Successfully", StringComparison.OrdinalIgnoreCase)))
                    {
                        serviceresponse.response = msg;
                    }
                    else
                    {
                        serviceresponse.response = "Failed to " + request.Mode + ".";
                        serviceresponse.errors = msg
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    serviceresponse.response = "Error while processing response.";
                }
            }
            else
            {
                serviceresponse.response = "Failed";
            }

            return serviceresponse;
        }
        private string BuildServiceChargeXml(VendorServiceChargeRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<ServiceChargeDetail>");

            foreach (var row in request.VendorServiceChargemaster)
            {
                sb.Append("<ServiceCharge>");
                sb.AppendFormat("<Service_Charge_Type_Id>{0}</Service_Charge_Type_Id>", row.Service_Charge_Type_Id);
                sb.AppendFormat("<Billing_Type_Id>{0}</Billing_Type_Id>", row.Billing_Type_Id);
                sb.AppendFormat("<Cost_Center_Mapping_Id>{0}</Cost_Center_Mapping_Id>", row.Cost_Center_Mapping_Id);
                sb.AppendFormat("<MaxAmount>{0}</MaxAmount>", row.MaxAmount);
                sb.AppendFormat("<FromValue>{0}</FromValue>", row.FromValue);
                sb.AppendFormat("<ToValue>{0}</ToValue>", row.ToValue);
                sb.AppendFormat("<Effective_Date>{0}</Effective_Date>", row.Effective_Date);
            
                sb.Append("</ServiceCharge>");
            }

            sb.Append("</ServiceChargeDetail>");
            return sb.ToString();
        }

        public async Task<VendorServiceChargeResponse> FileUpload(IFormFile file, [FromForm] int CreatedBy)
        {
            //string ServiceChargeMaster,string ServiceChargeType,string SlabType, string SlabInnerType, int CreatedBy
            VendorServiceChargeResponse poDetails = new VendorServiceChargeResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "ServiceCharge");

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

                string storeProcedure = @"spImportVendorServiceCharge";
                var parameters = new DynamicParameters();

                parameters.Add("@xmlInput", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
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

        public async Task<DataSet> GetAllVendorServiceCharge(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
              ["@CompanyId"] = companyId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllVendorServiceCharge", parameters);
        }

        public async Task<List<GenDD>> GetAllBillingTypes()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetVendorBillingType");

            var res = await this._dbRepository.GetItemsAsync("USP_CommonDropDowns", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<GenDD>>(res) ?? new List<GenDD>();
            }

            return new List<GenDD>();
        }
        public async Task<List<GenDD>> GetAllVendorServiceType()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetVendorServiceType");

            var res = await this._dbRepository.GetItemsAsync("USP_CommonDropDowns", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<GenDD>>(res) ?? new List<GenDD>();
            }

            return new List<GenDD>();
        }
    }
}
