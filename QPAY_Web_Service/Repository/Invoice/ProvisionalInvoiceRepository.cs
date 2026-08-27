using Azure.Core;
using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.Invoice;
using static QPay.UI_Domain.Models.ActivationLwd;
using QPay.API;
using QPay.BAL.IRepository.Invoice;
using System.Linq.Expressions;
using QPay.API.Models;
using QPay.BAL.IRepository;



namespace QPay.BAL.Repository.Invoice
{
    public class ProvisionalInvoiceRepository : IProvisionalInvoiceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public ProvisionalInvoiceRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }
        public async Task<DataSet> GetProvisionalInvoice(int CompanyId, string payPeriodId, string createdBy)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@PayPeriodId"] = payPeriodId,
                ["@CreatedBy"] = createdBy,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAll_ProvisionalInvoice", parameters, 1500);
        }

        public async Task<InvoiceResponse> ProvisionalInvoiceSplit(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Invoice", "ProvisionalInvoiceSplit");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"ProvisionalInvoiceSplit_{CreatedBy}_{datePrefix}{extension}";

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
                    invoiceDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return invoiceDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerialize = ds.Tables[0];

                // Add extra columns that SQL expects
                if (!dtToSerialize.Columns.Contains("PayPeriod"))
                    dtToSerialize.Columns.Add("PayPeriod", typeof(string));

                if (!dtToSerialize.Columns.Contains("BatchId"))
                    dtToSerialize.Columns.Add("BatchId", typeof(int));

                if (!dtToSerialize.Columns.Contains("EmployeeId"))
                    dtToSerialize.Columns.Add("EmployeeId", typeof(int));

                if (!dtToSerialize.Columns.Contains("CompanyId"))
                    dtToSerialize.Columns.Add("CompanyId", typeof(int));

                if (!dtToSerialize.Columns.Contains("PayPeriodId"))
                    dtToSerialize.Columns.Add("PayPeriodId", typeof(int));

                if (!dtToSerialize.Columns.Contains("IsNotMatching"))
                    dtToSerialize.Columns.Add("IsNotMatching", typeof(int));

                // Assign default values
                foreach (DataRow row in dtToSerialize.Rows)
                {
                    row["PayPeriod"] = payperiod;   // or actual PayPeriod from UI
                    row["BatchId"] = 0;
                    row["EmployeeId"] = 0;
                    row["CompanyId"] = CompanyId;  // matches your SQL insert
                    row["PayPeriodId"] = 0;
                    row["IsNotMatching"] = 0;
                }

                // Convert to XML
                using var xmlWriter = new StringWriter();
                dtToSerialize.TableName = "Table";  // Required for SQL XQuery
                DataSet xmlDS = new DataSet("NewDataSet");
                xmlDS.Tables.Add(dtToSerialize.Copy());

                xmlDS.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string storeProcedure = "USP_Provisional_Invoice_Split";
                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);
                parameters.Add("@Action", "Split");

                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {

                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully.", StringComparison.OrdinalIgnoreCase))
                        {
                            invoiceDetails.response = message;
                        }
                        else
                        {
                            invoiceDetails.response = "Failed to import.";
                            invoiceDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        invoiceDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    invoiceDetails.response = "Failed";
                }

            }
            else
            {
                invoiceDetails.response = "File not found";
            }
            return invoiceDetails;
        }

        //public async Task<InvoiceResponse> PerformaInvoiceMerge(string CompanyId, string PayPeriodId,
        //    string MAP_NAME_ID, string MergeLot, string CreatedBy, string Remarks, string Merged_Input_No,
        //    string Data_From)
        //{
        //    InvoiceResponse invoiceDetails = new InvoiceResponse();

        //    string storeProcedure = "USP_Performa_Invoice_Merge";
        //    var parameters = new DynamicParameters();

        //    parameters.Add("@COMPANY_ID", CompanyId);
        //    parameters.Add("@PAYPERIOD_ID", PayPeriodId);
        //    parameters.Add("@MAP_NAME_ID", MAP_NAME_ID);
        //    parameters.Add("@MERGIED_LOT", MergeLot);
        //    parameters.Add("@MERGIED_INPUT_NO", Merged_Input_No);
        //    parameters.Add("@USER_ID", CreatedBy);
        //    parameters.Add("@Remarks", Remarks);
        //    parameters.Add("@Data_From", Data_From);

        //    var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

        //    if (!string.IsNullOrWhiteSpace(res))
        //    {
        //        try
        //        {
        //            var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
        //            var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

        //            if (!string.IsNullOrWhiteSpace(message) &&
        //                message.Contains("Merged successfully with lot number", StringComparison.OrdinalIgnoreCase))
        //            {
        //                invoiceDetails.response = message;
        //            }
        //            else
        //            {
        //                invoiceDetails.response = "Failed to import.";
        //                invoiceDetails.errors = res
        //                    ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        //                    .ToList() ?? new List<string> { "Unknown error." };
        //            }
        //        }
        //        catch
        //        {
        //            invoiceDetails.response = "Error while processing response.";
        //        }
        //    }
        //    else
        //    {
        //        invoiceDetails.response = "Failed";
        //    }

        //    return invoiceDetails;
        //}

        public async Task<InvoiceResponse> ProvisionalInvoiceInitiate(ProvisionalInvoiceInitiateRequest provisionalrequest)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse(); 
            string storeProcedure = "Proc_ManageInvoiceEmployeeDetail_NewUI";
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "CreateInvoice",
                ["@UserId"] = provisionalrequest.CreatedBy,
                ["@CompanyId"] = provisionalrequest.CompanyId,
                ["@CostCenterMappingId"] = provisionalrequest.Map_Name_Id,
                ["@PayPeriodId"] = provisionalrequest.PayPeriodId,
                ["@LotNumber"] = provisionalrequest.LotNo,
                ["@InputNumber"] = provisionalrequest.Input_No,
                ["@IsActive"] = provisionalrequest.Isactive,
                ["@CreatedBy"] = provisionalrequest.CreatedBy,
                ["@ModifiedBy"] = provisionalrequest.CreatedBy,
                ["@PageNo"] = "1",
                ["@PageSize"] = "10",
                ["@Map_Name"] = provisionalrequest.Map_Name,
                ["@Pay_Period"] = provisionalrequest.PayPeriod,
                ["@InvoiceCultureId"] = provisionalrequest.InvoiceCulture_Id
            };
            
            DataSet ds = _dbRepository.ExecuteStoredProcedureToDataSetAsync(storeProcedure, parameters, 1500);
            var res = ds.Tables[1].Rows[0]["Result"].ToString();
            if (!string.IsNullOrWhiteSpace(res))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        invoiceDetails.response = res;
                    }
                }
                catch
                {
                    invoiceDetails.response = "Error while processing response.";
                }
            }
            else
            {
                invoiceDetails.response = "Failed";
            }

            return invoiceDetails;
        }

        public async Task<InvoiceResponse> UpdateMapName(IFormFile file, [FromForm] string CompanyId,
            [FromForm] string payperiod, [FromForm] string CreatedBy, [FromForm] string payperiodId)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["UAEPath"].ToString(), "Invoice", "UpdateMap");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"UpdateMap_{CompanyId}_{payperiodId}_{datePrefix}{extension}";

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
                    invoiceDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return invoiceDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerialize = ds.Tables[0];

                // Add extra columns that SQL expects
                if (!dtToSerialize.Columns.Contains("COMPANY_ID"))
                    dtToSerialize.Columns.Add("COMPANY_ID", typeof(string));

                if (!dtToSerialize.Columns.Contains("PAY_PERIOD_ID"))
                    dtToSerialize.Columns.Add("PAY_PERIOD_ID", typeof(int));

                // Assign default values
                foreach (DataRow row in dtToSerialize.Rows)
                {
                    row["COMPANY_ID"] = CompanyId;   // or actual PayPeriod from UI
                    row["PAY_PERIOD_ID"] = payperiodId;
                }

                // Convert to XML
                using var xmlWriter = new StringWriter();
                dtToSerialize.TableName = "Table";  // Required for SQL XQuery
                DataSet xmlDS = new DataSet("NewDataSet");
                xmlDS.Tables.Add(dtToSerialize.Copy());

                xmlDS.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string storeProcedure = "Proc_Upload_Mapname_Change_Request";
                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {

                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully.", StringComparison.OrdinalIgnoreCase))
                        {
                            invoiceDetails.response = message;
                        }
                        else
                        {
                            invoiceDetails.response = "Failed to import.";
                            invoiceDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        invoiceDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    invoiceDetails.response = "Failed";
                }

            }
            else
            {
                invoiceDetails.response = "File not found";
            }
            return invoiceDetails;
        }

        //public async Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
        //   [FromForm] string payperiodId, [FromForm] string CreatedBy)
        //{
        //    InvoiceResponse invoiceDetails = new InvoiceResponse();

        //    if (file != null && file.Length != 0)
        //    {
        //        var uploadsFolder = Path.Combine(_configuration["UAEPath"].ToString(), "Invoice", "Attributes");
        //        if (!Directory.Exists(uploadsFolder))
        //            Directory.CreateDirectory(uploadsFolder);

        //        var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
        //        var originalFileName = Path.GetFileName(file.FileName);
        //        var extension = Path.GetExtension(originalFileName);
        //        var newFileName = $"Attributes_{CompanyId}_{payperiodId}_{datePrefix}{extension}";

        //        var filePath = Path.Combine(uploadsFolder, newFileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //        DataSet ds = new DataSet("DocumentElement");
        //        ds = ExcelToDataSet(filePath);
        //        //Convert dt to XML
        //        if (ds.Tables.Count == 0)
        //        {
        //            invoiceDetails.response = "Excel sheet is empty or not formatted correctly.";
        //            return invoiceDetails;
        //        }

        //        DataTable dtToSerialize = ds.Tables[0];

        //        if (!dtToSerialize.Columns.Contains("Company_Id"))
        //            dtToSerialize.Columns.Add("Company_Id", typeof(string));

        //        if (!dtToSerialize.Columns.Contains("PayPeriod_Id"))
        //            dtToSerialize.Columns.Add("PayPeriod_Id", typeof(int));

        //        // Add extra columns that SQL expects
        //        if (!dtToSerialize.Columns.Contains("Narration"))
        //            dtToSerialize.Columns.Add("Narration", typeof(string));

        //        if (!dtToSerialize.Columns.Contains("PO_Number"))
        //            dtToSerialize.Columns.Add("PO_Number", typeof(string));

        //        if (!dtToSerialize.Columns.Contains("GL_Code"))
        //            dtToSerialize.Columns.Add("GL_Code", typeof(string));

        //        if (!dtToSerialize.Columns.Contains("Cost_Center_Name"))
        //            dtToSerialize.Columns.Add("Cost_Center_Name", typeof(string));

        //        if (!dtToSerialize.Columns.Contains("Client_SPOC_Name"))
        //            dtToSerialize.Columns.Add("Client_SPOC_Name", typeof(string));

        //        if (!dtToSerialize.Columns.Contains("Work_Order_Number"))
        //            dtToSerialize.Columns.Add("Work_Order_Number", typeof(string));

        //        foreach (DataRow row in dtToSerialize.Rows)
        //        {
        //            row["Company_Id"] = CompanyId;   // or actual PayPeriod from UI
        //            row["PayPeriod_Id"] = payperiodId;
        //        }

        //        foreach (DataRow row in dtToSerialize.Rows)
        //        {
        //            foreach (DataColumn col in dtToSerialize.Columns)
        //            {
        //                if (row.IsNull(col))
        //                    row[col] = string.Empty; // replace DBNull with empty string
        //            }
        //        }


        //        // Convert to XML
        //        using var xmlWriter = new StringWriter();
        //        dtToSerialize.TableName = "Table";  // Required for SQL XQuery
        //        DataSet xmlDS = new DataSet("NewDataSet");
        //        xmlDS.Tables.Add(dtToSerialize.Copy());

        //        xmlDS.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
        //        string xmlInput = xmlWriter.ToString();
        //        string storeProcedure = "Proc_Upload_Invoice_Attributes";
        //        var parameters = new DynamicParameters();

        //        parameters.Add("@XML_File", xmlInput);
        //        parameters.Add("@CreatedBy", CreatedBy);

        //        var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

        //        if (!string.IsNullOrWhiteSpace(res))
        //        {
        //            try
        //            {
        //                var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
        //                var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

        //                if (!string.IsNullOrWhiteSpace(message) &&
        //                    message.Contains("Row(s) Uploaded Successfully.", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    invoiceDetails.response = message;
        //                }
        //                else
        //                {
        //                    invoiceDetails.response = "Failed to import.";
        //                    invoiceDetails.errors = res
        //                        ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        //                        .ToList() ?? new List<string> { "Unknown error." };
        //                }
        //            }
        //            catch
        //            {
        //                invoiceDetails.response = "Error while processing response.";
        //            }
        //        }
        //        else
        //        {
        //            invoiceDetails.response = "Failed";
        //        }
        //    }
        //    else
        //    {
        //        invoiceDetails.response = "File not found";
        //    }
        //    return invoiceDetails;
        //}


        public class ResponseModel
        {
            public string Result { get; set; }
            public string Error_Message { get; set; }
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

       public string GenerateXmlFromEmployees(List<ActivationEmployeelist> employees)
        {
            var ds = new DataSet("NewDataSet");
            var dt = new DataTable("EmployeeMaster");
            dt.Columns.Add("EMPLOYEE_CODE", typeof(string));
            dt.Columns.Add("REMARKS", typeof(string));

            foreach (var emp in employees)
            {
                var row = dt.NewRow();
                row["EMPLOYEE_CODE"] = emp.EMPLOYEE_CODE;
                row["REMARKS"] = emp.REMARKS;
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                return sw.ToString();
            }
        }


        public string GenerateXmlFromEmployeesLWD(List<LWDEmployeelist> employees)
        {
            var ds = new DataSet("NewDataSet");
            var dt = new DataTable("EmployeeMaster");
            dt.Columns.Add("Employee_Code", typeof(string));
            dt.Columns.Add("First_Name", typeof(string));
            dt.Columns.Add("DoJ", typeof(string));
            dt.Columns.Add("Last_Working_Day", typeof(string));
            dt.Columns.Add("Reason_Of_Leaving", typeof(string));
            dt.Columns.Add("RELIEVING_LETTERYESNO", typeof(string));

            foreach (var emp in employees)
            {
                var row = dt.NewRow();
                row["Employee_Code"] = emp.Employee_Code;
                row["First_Name"] = emp.First_Name;
                row["DoJ"] = emp.DoJ;
                row["Last_Working_Day"] = emp.Last_Working_Day;
                row["Reason_Of_Leaving"] = emp.Reason_Of_Leaving;
                row["RELIEVING_LETTERYESNO"] = emp.RELIEVING_LETTER_YES_NO;
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                return sw.ToString();
            }
        }

        public static DataTable ReadExcelWorksheetToDataTable(IXLWorksheet worksheet, bool hasHeader = true)
        {
            var dataTable = new DataTable();
            var firstRowUsed = worksheet.FirstRowUsed();
            var row = firstRowUsed.RowUsed();

            int columnCount = worksheet.LastColumnUsed().ColumnNumber();

            // Add columns
            foreach (var cell in row.Cells(1, columnCount))
            {
                dataTable.Columns.Add(hasHeader ? cell.GetValue<string>() : $"Column {cell.Address.ColumnNumber}");
            }

            // Start reading after header if exists
            var firstDataRow = hasHeader ? row.RowBelow() : row;

            foreach (var dataRow in worksheet.Rows(firstDataRow.RowNumber(), worksheet.LastRowUsed().RowNumber()))
            {
                var data = new object[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    data[i] = dataRow.Cell(i + 1).Value;
                }
                dataTable.Rows.Add(data);
            }

            return dataTable;
        }

    }
}
