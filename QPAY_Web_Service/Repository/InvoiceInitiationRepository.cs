using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Admin;
using QPay.UI.Common;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Invoice.Invoice;

namespace QPay.BAL.Repository
{
    public class InvoiceInitiationRepository: IInvoiceInitiationRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _config;
        private readonly IPayRegisterRepository _payRegisterRepository;
        
        public InvoiceInitiationRepository(DbRepository dbRepository, IConfiguration config, IPayRegisterRepository payRegisterRepository)
        {
            this._dbRepository = dbRepository;
            this._config = config;
            this._payRegisterRepository = payRegisterRepository;
          
        }

        public async Task<List<RemarksResponse>> getRemarksByReqNo(RequestModel requestModel)
        {
            string storeProcedure = "[dbo].[sp_GetAllRemarksByReqNo]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@ReqNo", requestModel.Req_No);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<RemarksResponse>>(res);
            return list?.ToList() ?? new List<RemarksResponse>();
        }


        public async Task<List<CommonUI>> GetTaxTypes(string action)
        {
       
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", action);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<CommonUI>(); // return empty object if no result
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<CommonUI>>(res);
                return list?.ToList() ?? new List<CommonUI>();
            }
            catch (JsonException ex)
            {
                // log the error if you have logging available
                // _logger.LogError(ex, "Failed to deserialize POQuantityUI response");
                return new List<CommonUI>();
            }

        }

        public async Task<List<InvoiceInitiationUI>> Search(int? Company_Id, string PayPeriod, int? TaxTypeId)
        {
            string storeProcedure = "[dbo].[sp_GetAllInvoiceInitiateDetails2]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@PayPeriodId", PayPeriod ?? (object)DBNull.Value);
            parameter.Add("@CompanyId", Company_Id ?? (object)DBNull.Value);
            parameter.Add("@TaxTypeId", TaxTypeId ?? (object)DBNull.Value);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<InvoiceInitiationUI>>(res);
            return list?.ToList() ?? new List<InvoiceInitiationUI>();
            //return res?.ToList() ?? new List<InvoiceInitiationUI>
            //{
            //    Error_Message = string.Empty
            //};
        }

       
        public async Task<List<InitiationRequestUI>> InitiationSearch(InitiationRequestModel initiationRequestModel)
        {
            string storeProcedure = "[dbo].[SP_Invoice_Initiation_search]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Company_Id", initiationRequestModel.Company_Id ?? (object)DBNull.Value);
            parameter.Add("@PayPeriod_Id", initiationRequestModel.PayPeriod_Id ?? (object)DBNull.Value);            
            parameter.Add("@InvoiceType", initiationRequestModel.InvoiceType ?? (object)DBNull.Value);
            parameter.Add("@ActionType", initiationRequestModel.ActionType ?? (object)DBNull.Value);
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<InitiationRequestUI>>(res);
            return list?.ToList() ?? new List<InitiationRequestUI>();
        }

        public async Task<List<InitiationRequestUI>> InitiationSearchAllot(InvoiceDetailModel invoiceDetailModel)
        {
            ///Allottment
            var param = new DynamicParameters();
            param.Add("@UserId", invoiceDetailModel.userId);
            var allot = await _dbRepository.GetItemsAsync("SP_AutoAllocation_Invoice", param);

            string storeProcedure = "[dbo].[SP_Invoice_Initiation_search_Allot_test]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@InvoiceType", invoiceDetailModel.InvoiceType ?? (object)DBNull.Value);
            parameter.Add("@ActionType", invoiceDetailModel.ActionType ?? (object)DBNull.Value);
            parameter.Add("@UserId", invoiceDetailModel.userId ?? (object)DBNull.Value);            
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<InitiationRequestUI>>(res);
            return list?.ToList() ?? new List<InitiationRequestUI>();
        }

        public async Task<List<InitiationRequestUI>> InvoiceQCDetail(int userId)
        {
            string storeProcedure = "[dbo].[SP_Billing_dashboard_Detail]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@UserId", userId);
            
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<InitiationRequestUI>>(res);
            return list?.ToList() ?? new List<InitiationRequestUI>();
        }

        public async Task<List<InvoiceDashboardDto>> GetAllInvoiceAllotDetails(InvoiceDetailModel invoiceDetailModel)
        {
            string storeProcedure = "[dbo].[SP_Invoice_Initiation_search_Allot]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@InvoiceType", invoiceDetailModel.InvoiceType ?? (object)DBNull.Value);
            parameter.Add("@ActionType", invoiceDetailModel.ActionType ?? (object)DBNull.Value);
            parameter.Add("@UserId", invoiceDetailModel.userId ?? (object)DBNull.Value);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            var list = JsonConvert.DeserializeObject<List<InvoiceDashboardDto>>(res);
            return list?.ToList() ?? new List<InvoiceDashboardDto>();
        }
        public List<Double?> GetUniqueColumnValues(DataTable table, string columnName)
        {
            return table.AsEnumerable()
                        .Select(row => row.Field<Double?>(columnName))
                        .Where(value => value != null)
                        .Distinct()
                        .ToList();
        }

        public List<Int64?> GetUniqueColumnValuesByInt(DataTable table, string columnName)
        {
            return table.AsEnumerable()
                        .Select(row => row.Field<Int64?>(columnName))
                        .Where(value => value != null)
                        .Distinct()
                        .ToList();
        }

        public async Task<FileResponse> InitiationSearchExport(IntiationExportRequest intiationExportRequest)
        {
            FileResponse fileResponse = new FileResponse();

            //var parameter = new DynamicParameters();
            //parameter.Add("@CompanyId", intiationExportRequest.Company_Id ?? (object)DBNull.Value);
            //parameter.Add("@PayperiodId", intiationExportRequest.PayPeriod_Id ?? (object)DBNull.Value);
            //parameter.Add("@RequestNo", intiationExportRequest.LotNo ?? (object)DBNull.Value);
            //parameter.Add("@Lotno", intiationExportRequest.ReqNo ?? (object)DBNull.Value);
            //parameter.Add("@Invoice_Type", intiationExportRequest.Invoice_Type ?? (object)DBNull.Value);

            //var res = await _dbRepository.GetItemsAsync("[dbo].[sp_PayRegister_Lot_RequestWise]", parameter);

            if (intiationExportRequest.InvoiceCultureType != "SPLIT")
            {

                if (intiationExportRequest.Data_From == "OI")
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@Company_ID", intiationExportRequest.Company_Id ?? (object)DBNull.Value);
                    parameter.Add("@Pay_Frequency_Detail_Id", intiationExportRequest.PayPeriod_Id ?? (object)DBNull.Value);
                    parameter.Add("@INPUTNUMBER", intiationExportRequest.LotNo ?? (object)DBNull.Value);
                    parameter.Add("@RequestNo", intiationExportRequest.ReqNo ?? (object)DBNull.Value);
                    parameter.Add("@Invoice_Type", intiationExportRequest.Invoice_Type ?? (object)DBNull.Value);

                    var res = await _dbRepository.GetItemsSecondaryAsync("[dbo].[sp_OtherIncome_Report_PONUMBER_Request_ExportToExcel]", parameter);
                    DataTable payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                    if (payregister_dt != null)
                    {
                        if (payregister_dt.Rows.Count > 0)
                        {

                            DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
                            List<string> RemoveColums = new List<string>();
                            DataRow dtrow = payregister_dt.NewRow();
                            foreach (DataColumn column in payregister_dt.Columns)
                            {
                                var value = lastRow[column];

                                if (column.ColumnName.ToLower() == ("Input_Number").ToLower())
                                {
                                    var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                                    dtrow[column] = column_Unique[0];
                                }
                                else
                                {

                                    if (column.DataType.Name == "Double")
                                    {
                                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column));
                                        dtrow[column] = columnsum;
                                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                                        {
                                            RemoveColums.Add(column.ToString());
                                        }


                                    }
                                    else if (column.DataType.Name == "Int64")
                                    {
                                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column));
                                        dtrow[column] = columnsum;
                                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                                        {
                                            RemoveColums.Add(column.ToString());
                                        }



                                    }
                                    else
                                    {
                                        // dtrow[column]="";
                                    }
                                }
                            }
                            var emptyColumns = payregister_dt.Columns.Cast<DataColumn>()
                                               .Where(col => payregister_dt.AsEnumerable().All(row =>
                                               {
                                                   var value = row[col];
                                                   return value == null || string.IsNullOrWhiteSpace(value.ToString());
                                               }))
                                                .Select(col => col.ColumnName)
                                                .ToList();
                            foreach (var columnName in emptyColumns)
                                payregister_dt.Columns.Remove(columnName);

                            double service = 0.0;
                            double ctc = 0.0;
                            if (payregister_dt.Columns.Contains("SERCG"))
                            {
                                // service = payregister_dt.AsEnumerable()
                                //.Where(row => !string.IsNullOrWhiteSpace(row.Field<string>("SERCG")))
                                //.Sum(row => Convert.ToDouble(row.Field<string>("SERCG")));
                                if (payregister_dt.Columns["SERCG"].DataType.Name == "Double")
                                {
                                    service = payregister_dt.AsEnumerable()
                                        .Where(row => row.Field<double?>("SERCG").HasValue)
                                        .Sum(row => row.Field<double?>("SERCG").Value);
                                }
                            }

                            if (payregister_dt.Columns.Contains("CTC"))
                            {
                                //   service = payregister_dt.AsEnumerable()
                                //.Where(row => !string.IsNullOrWhiteSpace(row.Field<string>("CTC")))
                                //.Sum(row => Convert.ToDouble(row.Field<string>("CTC")));

                                if (payregister_dt.Columns["CTC"].DataType.Name == "Double")
                                {
                                    ctc = payregister_dt.AsEnumerable()
                                    .Where(row => row.Field<double?>("CTC").HasValue)
                                    .Sum(row => row.Field<double?>("CTC").Value);
                                }
                            }
                            payregister_dt.Rows.Add(dtrow);

                            foreach (var item in RemoveColums)
                            {
                                payregister_dt.Columns.Remove(item);
                            }
                            using var workbook = new XLWorkbook();
                            {
                                var ws = workbook.AddWorksheet(payregister_dt, "Other Income");
                                ws.Table(0).ShowAutoFilter = false;
                                ws.Table(0).Theme = XLTableTheme.None;
                                ws.Row(1).InsertRowsAbove(3);
                                ws.SheetView.FreezeRows(4);
                                //ws.SheetView.FreezeColumns(6);


                                var comayName = _payRegisterRepository.CompanyNameByCode(Convert.ToInt32(intiationExportRequest.Company_Id));
                                var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();

                                ws.Range("A1:Z1").Merge();
                                ws.Range("A2:Z2").Merge();
                                ws.Range("A3:Z3").Merge();


                                ws.Cell(1, 1).Value = comapny.Client_Name;
                                ws.Cell(1, 1).Style.Font.Bold = true;
                                ws.Cell(1, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
                                ws.Cell(2, 1).Value = "ONETIME FOR THE MONTH OF " + intiationExportRequest.Pay_Period;
                                ws.Cell(2, 1).Style.Font.Bold = true;
                                ws.Cell(2, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
                                var headerRange = ws.Row(4);
                                headerRange.Style.Font.Bold = true;


                                var lastrow = ws.LastRowUsed().RowNumber();
                                int lastCol = ws.LastColumnUsed().ColumnNumber();
                                var rowRange = ws.Range(4, 1, lastrow, lastCol); // Rows 2–5, all used columns
                                rowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                rowRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                ws.Cell(lastrow, 1).Value = "Grand Total";
                                ws.Columns().AdjustToContents(); // Auto fit all columns
                                ws.Rows().AdjustToContents();    // Auto fit all rows


                                using (MemoryStream stream = new MemoryStream())
                                {
                                    workbook.SaveAs(stream);
                                    stream.Seek(0, SeekOrigin.Begin);
                                    var bytes = Convert.ToBase64String(stream.ToArray());
                                    //  FileResponse fileResponse = new FileResponse();
                                    fileResponse.FileName = "Other Income.xlsx";
                                    fileResponse.File = bytes;

                                }
                            }
                        }
                        else
                        {
                            fileResponse.File = "No";
                            fileResponse.FileName = "Not Existing";
                        }
                    }
                    else
                    {
                        fileResponse.File = "No";
                        fileResponse.FileName = "Not Existing";
                    }


                }
                else
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@Company_Id", intiationExportRequest.Company_Id ?? (object)DBNull.Value);
                    parameter.Add("@Pay_Period_Id", intiationExportRequest.PayPeriod_Id ?? (object)DBNull.Value);
                    parameter.Add("@Lot_No", intiationExportRequest.LotNo ?? (object)DBNull.Value);
                    parameter.Add("@RequestNo", intiationExportRequest.ReqNo ?? (object)DBNull.Value);
                    parameter.Add("@Invoice_Type", intiationExportRequest.Invoice_Type ?? (object)DBNull.Value);

                    var res = await _dbRepository.GetItemsAsync("[dbo].[sp_PayRegister_Lot_RequestWise]", parameter);
                    DataTable payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);

                    try
                    {
                        //payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                        if (payregister_dt != null)
                        {
                            if (payregister_dt.Rows.Count > 0)
                            {
                                DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
                                List<string> RemoveColums = new List<string>();
                                DataRow dtrow = payregister_dt.NewRow();
                                foreach (DataColumn column in payregister_dt.Columns)
                                {
                                    var value = lastRow[column];

                                    if (column.DataType.Name == "Double")
                                    {

                                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column)) ?? 0;
                                        dtrow[column] = columnsum;
                                        if (column.ColumnName.ToLower() == "lot_number")
                                        {
                                            var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
                                            dtrow[column] = column_Unique[0];

                                        }
                                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                                        {
                                            RemoveColums.Add(column.ToString());
                                        }
                                    }
                                    else if (column.DataType.Name == "Int64")
                                    {
                                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column)) ?? 0;
                                        dtrow[column] = columnsum;
                                        if (column.ColumnName.ToLower() == "lot_number")
                                        {
                                            var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                                            dtrow[column] = column_Unique[0];

                                        }
                                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                                        {
                                            RemoveColums.Add(column.ToString());
                                        }


                                    }
                                    else
                                    {
                                        dtrow[column] = "";
                                    }
                                }

                                payregister_dt.Rows.Add(dtrow);
                                foreach (var item in RemoveColums)
                                {
                                    payregister_dt.Columns.Remove(item);
                                }
                                var emptyColumns = payregister_dt.Columns.Cast<DataColumn>()
                                               .Where(col => payregister_dt.AsEnumerable().All(row =>
                                               {
                                                   var value = row[col];
                                                   return value == null || string.IsNullOrWhiteSpace(value.ToString());
                                               }))
                                                .Select(col => col.ColumnName)
                                                .ToList();
                                foreach (var columnName in emptyColumns)
                                    payregister_dt.Columns.Remove(columnName);


                                var comayName = _payRegisterRepository.CompanyNameByCode(Convert.ToInt32(intiationExportRequest.Company_Id));
                                var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();



                                //  wb.Worksheets.Add(dataTable);
                                if (payregister_dt.Columns.Count > 1)
                                {
                                    using var workbook = new XLWorkbook();
                                    {
                                        var ws = workbook.AddWorksheet(payregister_dt, "PayRegister");
                                        ws.Table(0).ShowAutoFilter = false;
                                        ws.Table(0).Theme = XLTableTheme.None;
                                        //ws.SheetView.FreezeRows(4);
                                        //ws.SheetView.FreezeColumns(2);

                                        ws.Row(1).InsertRowsAbove(3);
                                        ws.Range("A1:Z1").Merge();
                                        ws.Range("A2:Z2").Merge();
                                        ws.Range("A3:Z3").Merge();

                                        var usedRange = ws.RangeUsed();

                                        if (usedRange != null)
                                        {
                                            foreach (var cell in usedRange.Cells())
                                            {
                                                cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                cell.Style.Border.TopBorderColor = XLColor.Black;
                                                cell.Style.Border.BottomBorderColor = XLColor.Black;
                                                cell.Style.Border.LeftBorderColor = XLColor.Black;
                                                cell.Style.Border.RightBorderColor = XLColor.Black;
                                            }
                                        }

                                        ws.Cell(1, 1).Value = comapny.Client_Name;
                                        ws.Cell(1, 1).Style.Font.Bold = true;
                                        ws.Cell(2, 1).Value = string.Format("SALARY FOR THE MONTH OF {0}", intiationExportRequest.Pay_Period);
                                        ws.Cell(2, 1).Style.Font.Bold = true;
                                        var lastrow = ws.LastRowUsed().RowNumber();

                                        //if (ctc!=null && service!=null)
                                        //{
                                        //var Total = ctc+service;
                                        //var toal_GST = Total*(18.0/100.0);
                                        ws.Cell(lastrow, 1).Value = "Grand Total";




                                        using (MemoryStream stream = new MemoryStream())
                                        {
                                            workbook.SaveAs(stream);
                                            var bytes = Convert.ToBase64String(stream.ToArray());
                                            //  FileResponse fileResponse = new FileResponse();
                                            fileResponse.FileName = "PayRegister.xlsx";
                                            fileResponse.File = bytes;

                                        }

                                    }

                                }
                                else
                                {
                                    using (MemoryStream stream = new MemoryStream())
                                    {

                                        using var workbook = new XLWorkbook();
                                        {
                                            workbook.SaveAs(stream);
                                            var bytes = Convert.ToBase64String(stream.ToArray());

                                            fileResponse.FileName = "PayRegister.xlsx";
                                            fileResponse.File = bytes;
                                            fileResponse = fileResponse;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                fileResponse.File = "No";
                                fileResponse.FileName = "Not Existing";
                            }
                        }
                        else
                        {
                            fileResponse.File = "No";
                            fileResponse.FileName = "Not Existing";
                        }

                    }
                    catch (Exception ex)
                    {
                        payregister_dt.Columns.Add("Exception", typeof(string));
                        payregister_dt.Rows.Add(string.Format("{0},{1},{2}", ex.Message, ex.StackTrace, ex.InnerException));

                    }


                }
                return fileResponse;
            }
            else
            {
                var parameter = new Dictionary<string, object?>
                {
                    ["@CompanyId"] = intiationExportRequest.Company_Id,
                    ["@PayperiodId"] = intiationExportRequest.PayPeriod_Id,
                    ["@RequestNo"] = intiationExportRequest.ReqNo,
                    ["@Lotno"] = intiationExportRequest.LotNo

                    //["@Action"] = "Search",
                    //["@CreatedBy"] = userId,
                };
                var res = _dbRepository.ExecuteStoredProcedureToDataSetSecondaryAsync("sp_SplitPayregister1", parameter);
                int i = 1;
                using var workbook = new XLWorkbook();
                {
                    string sheetName = "";
                    foreach (DataTable payregister_dt in res.Tables)
                    {
                       
                        if (payregister_dt.Columns.Count == 1)
                        {
                            sheetName = payregister_dt.Rows[0][0].ToString();
                        }
                        else
                        {
                            if (payregister_dt.Rows.Count > 0)
                            {
                                var ws = workbook.AddWorksheet(payregister_dt, sheetName + "-"  + i.ToString());
                                ws.Table(0).ShowAutoFilter = false;
                                ws.Table(0).Theme = XLTableTheme.None;
                            }
                        }
                        i++;

                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        var bytes = Convert.ToBase64String(stream.ToArray());
                        //  FileResponse fileResponse = new FileResponse();
                        fileResponse.FileName = "SplitRegister_"+System.DateTime.Now.ToString("ddMMyyyyhhmmss")+".xlsx";
                        fileResponse.File = bytes;

                    }
                }


                //if (intiationExportRequest.Data_From == "OI")
                //{
                //    var parameter = new DynamicParameters();
                //    parameter.Add("@Company_ID", intiationExportRequest.Company_Id ?? (object)DBNull.Value);
                //    parameter.Add("@Pay_Frequency_Detail_Id", intiationExportRequest.PayPeriod_Id ?? (object)DBNull.Value);
                //    parameter.Add("@INPUTNUMBER", intiationExportRequest.LotNo ?? (object)DBNull.Value);
                //    parameter.Add("@RequestNo", intiationExportRequest.ReqNo ?? (object)DBNull.Value);
                //    parameter.Add("@Invoice_Type", intiationExportRequest.Invoice_Type ?? (object)DBNull.Value);

                //    var res = await _dbRepository.GetItemsSecondaryAsync("[dbo].[sp_OtherIncome_Report_PONUMBER_Request_ExportToExcel]", parameter);
                //    DataTable payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);

                //    if (payregister_dt != null)
                //    {
                //        if (payregister_dt.Rows.Count > 0)
                //        {

                //            DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
                //            List<string> RemoveColums = new List<string>();
                //            DataRow dtrow = payregister_dt.NewRow();
                //            foreach (DataColumn column in payregister_dt.Columns)
                //            {
                //                var value = lastRow[column];

                //                if (column.ColumnName.ToLower() == ("Input_Number").ToLower())
                //                {
                //                    var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                //                    dtrow[column] = column_Unique[0];
                //                }
                //                else
                //                {

                //                    if (column.DataType.Name == "Double")
                //                    {
                //                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column));
                //                        dtrow[column] = columnsum;
                //                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                //                        {
                //                            RemoveColums.Add(column.ToString());
                //                        }


                //                    }
                //                    else if (column.DataType.Name == "Int64")
                //                    {
                //                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column));
                //                        dtrow[column] = columnsum;
                //                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                //                        {
                //                            RemoveColums.Add(column.ToString());
                //                        }



                //                    }
                //                    else
                //                    {
                //                        // dtrow[column]="";
                //                    }
                //                }
                //            }
                //            var emptyColumns = payregister_dt.Columns.Cast<DataColumn>()
                //                               .Where(col => payregister_dt.AsEnumerable().All(row =>
                //                               {
                //                                   var value = row[col];
                //                                   return value == null || string.IsNullOrWhiteSpace(value.ToString());
                //                               }))
                //                                .Select(col => col.ColumnName)
                //                                .ToList();
                //            foreach (var columnName in emptyColumns)
                //                payregister_dt.Columns.Remove(columnName);

                //            double service = 0.0;
                //            double ctc = 0.0;
                //            if (payregister_dt.Columns.Contains("SERCG"))
                //            {
                //                // service = payregister_dt.AsEnumerable()
                //                //.Where(row => !string.IsNullOrWhiteSpace(row.Field<string>("SERCG")))
                //                //.Sum(row => Convert.ToDouble(row.Field<string>("SERCG")));
                //                if (payregister_dt.Columns["SERCG"].DataType.Name == "Double")
                //                {
                //                    service = payregister_dt.AsEnumerable()
                //                        .Where(row => row.Field<double?>("SERCG").HasValue)
                //                        .Sum(row => row.Field<double?>("SERCG").Value);
                //                }
                //            }

                //            if (payregister_dt.Columns.Contains("CTC"))
                //            {
                //                //   service = payregister_dt.AsEnumerable()
                //                //.Where(row => !string.IsNullOrWhiteSpace(row.Field<string>("CTC")))
                //                //.Sum(row => Convert.ToDouble(row.Field<string>("CTC")));

                //                if (payregister_dt.Columns["CTC"].DataType.Name == "Double")
                //                {
                //                    ctc = payregister_dt.AsEnumerable()
                //                    .Where(row => row.Field<double?>("CTC").HasValue)
                //                    .Sum(row => row.Field<double?>("CTC").Value);
                //                }
                //            }
                //            payregister_dt.Rows.Add(dtrow);

                //            foreach (var item in RemoveColums)
                //            {
                //                payregister_dt.Columns.Remove(item);
                //            }
                //            using var workbook = new XLWorkbook();
                //            {
                //                var ws = workbook.AddWorksheet(payregister_dt, "Other Income");
                //                ws.Table(0).ShowAutoFilter = false;
                //                ws.Table(0).Theme = XLTableTheme.None;
                //                ws.Row(1).InsertRowsAbove(3);
                //                ws.SheetView.FreezeRows(4);
                //                //ws.SheetView.FreezeColumns(6);


                //                var comayName = _payRegisterRepository.CompanyNameByCode(Convert.ToInt32(intiationExportRequest.Company_Id));
                //                var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();

                //                ws.Range("A1:Z1").Merge();
                //                ws.Range("A2:Z2").Merge();
                //                ws.Range("A3:Z3").Merge();


                //                ws.Cell(1, 1).Value = comapny.Client_Name;
                //                ws.Cell(1, 1).Style.Font.Bold = true;
                //                ws.Cell(1, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
                //                ws.Cell(2, 1).Value = "ONETIME FOR THE MONTH OF " + intiationExportRequest.Pay_Period;
                //                ws.Cell(2, 1).Style.Font.Bold = true;
                //                ws.Cell(2, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
                //                var headerRange = ws.Row(4);
                //                headerRange.Style.Font.Bold = true;


                //                var lastrow = ws.LastRowUsed().RowNumber();
                //                int lastCol = ws.LastColumnUsed().ColumnNumber();
                //                var rowRange = ws.Range(4, 1, lastrow, lastCol); // Rows 2–5, all used columns
                //                rowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //                rowRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //                ws.Cell(lastrow, 1).Value = "Grand Total";                          
                //                ws.Columns().AdjustToContents(); // Auto fit all columns
                //                ws.Rows().AdjustToContents();    // Auto fit all rows


                //                using (MemoryStream stream = new MemoryStream())
                //                {
                //                    workbook.SaveAs(stream);
                //                    stream.Seek(0, SeekOrigin.Begin);
                //                    var bytes = Convert.ToBase64String(stream.ToArray());
                //                    //  FileResponse fileResponse = new FileResponse();
                //                    fileResponse.FileName = "Other Income.xlsx";
                //                    fileResponse.File = bytes;

                //                }
                //            }
                //        }
                //        else
                //        {
                //            fileResponse.File = "No";
                //            fileResponse.FileName = "Not Existing";
                //        }
                //    }
                //    else
                //    {
                //        fileResponse.File = "No";
                //        fileResponse.FileName = "Not Existing";
                //    }


                //}
                //else
                //{
                //    var parameter = new DynamicParameters();
                //    parameter.Add("@Company_Id", intiationExportRequest.Company_Id ?? (object)DBNull.Value);
                //    parameter.Add("@Pay_Period_Id", intiationExportRequest.PayPeriod_Id ?? (object)DBNull.Value);
                //    parameter.Add("@Lot_No", intiationExportRequest.LotNo ?? (object)DBNull.Value);
                //    parameter.Add("@RequestNo", intiationExportRequest.ReqNo ?? (object)DBNull.Value);
                //    parameter.Add("@Invoice_Type", intiationExportRequest.Invoice_Type ?? (object)DBNull.Value);

                //    //var res = await _dbRepository.GetItemsAsync("[dbo].[sp_PayRegister_Lot_RequestWise]", parameter);
                //    var res = await _dbRepository.GetItemsAsync("[dbo].[sp_PayRegister_Lot_RequestWise]", parameter);
                //    DataTable payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);

                //    try
                //    {
                //        //payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                //        if (payregister_dt != null)
                //        {
                //            if (payregister_dt.Rows.Count > 0)
                //            {
                //                DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
                //                List<string> RemoveColums = new List<string>();
                //                DataRow dtrow = payregister_dt.NewRow();
                //                foreach (DataColumn column in payregister_dt.Columns)
                //                {
                //                    var value = lastRow[column];

                //                    if (column.DataType.Name == "Double")
                //                    {

                //                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column)) ?? 0;
                //                        dtrow[column] = columnsum;
                //                        if (column.ColumnName.ToLower() == "lot_number")
                //                        {
                //                            var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
                //                            dtrow[column] = column_Unique[0];

                //                        }
                //                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                //                        {
                //                            RemoveColums.Add(column.ToString());
                //                        }

                //                    }
                //                    else if (column.DataType.Name == "Int64")
                //                    {
                //                        var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column)) ?? 0;
                //                        dtrow[column] = columnsum;
                //                        if (column.ColumnName.ToLower() == "lot_number")
                //                        {
                //                            var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                //                            dtrow[column] = column_Unique[0];

                //                        }
                //                        if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                //                        {
                //                            RemoveColums.Add(column.ToString());
                //                        }


                //                    }
                //                    else
                //                    {
                //                        dtrow[column] = "";
                //                    }
                //                }

                //                payregister_dt.Rows.Add(dtrow);
                //                foreach (var item in RemoveColums)
                //                {
                //                    payregister_dt.Columns.Remove(item);
                //                }
                //                var emptyColumns = payregister_dt.Columns.Cast<DataColumn>()
                //                               .Where(col => payregister_dt.AsEnumerable().All(row =>
                //                               {
                //                                   var value = row[col];
                //                                   return value == null || string.IsNullOrWhiteSpace(value.ToString());
                //                               }))
                //                                .Select(col => col.ColumnName)
                //                                .ToList();
                //                foreach (var columnName in emptyColumns)
                //                    payregister_dt.Columns.Remove(columnName);



                //                var comayName =_payRegisterRepository.CompanyNameByCode(Convert.ToInt32(intiationExportRequest.Company_Id));
                //                var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();




                //                //  wb.Worksheets.Add(dataTable);
                //                if (payregister_dt.Columns.Count > 1)
                //                {
                //                    using var workbook = new XLWorkbook();
                //                    {

                //                                var ws = workbook.AddWorksheet(payregister_dt, "PayRegister");
                //                                ws.Table(0).ShowAutoFilter = false;
                //                                ws.Table(0).Theme = XLTableTheme.None;
                //                                //ws.SheetView.FreezeRows(4);
                //                                //ws.SheetView.FreezeColumns(2);

                //                                ws.Row(1).InsertRowsAbove(3);
                //                                ws.Range("A1:Z1").Merge();
                //                                ws.Range("A2:Z2").Merge();
                //                                ws.Range("A3:Z3").Merge();

                //                                var usedRange = ws.RangeUsed();

                //                                if (usedRange != null)
                //                                {
                //                                    foreach (var cell in usedRange.Cells())
                //                                    {
                //                                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //                                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                //                                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //                                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                //                                        cell.Style.Border.TopBorderColor = XLColor.Black;
                //                                        cell.Style.Border.BottomBorderColor = XLColor.Black;
                //                                        cell.Style.Border.LeftBorderColor = XLColor.Black;
                //                                        cell.Style.Border.RightBorderColor = XLColor.Black;
                //                                    }
                //                                }

                //                                ws.Cell(1, 1).Value = comapny.Client_Name;
                //                                ws.Cell(1, 1).Style.Font.Bold = true;
                //                                ws.Cell(2, 1).Value = string.Format("SALARY FOR THE MONTH OF {0}", intiationExportRequest.Pay_Period);
                //                                ws.Cell(2, 1).Style.Font.Bold = true;
                //                                var lastrow = ws.LastRowUsed().RowNumber();

                //                                //if (ctc!=null && service!=null)
                //                                //{
                //                                //var Total = ctc+service;
                //                                //var toal_GST = Total*(18.0/100.0);
                //                                ws.Cell(lastrow, 1).Value = "Grand Total";




                //                        using (MemoryStream stream = new MemoryStream())
                //                        {
                //                            workbook.SaveAs(stream);
                //                            var bytes = Convert.ToBase64String(stream.ToArray());
                //                            //  FileResponse fileResponse = new FileResponse();
                //                            fileResponse.FileName = "PayRegister.xlsx";
                //                            fileResponse.File = bytes;

                //                        }

                //                    }

                //                }
                //                else
                //                {
                //                    using (MemoryStream stream = new MemoryStream())
                //                    {

                //                        using var workbook = new XLWorkbook();
                //                        {
                //                            workbook.SaveAs(stream);
                //                            var bytes = Convert.ToBase64String(stream.ToArray());

                //                            fileResponse.FileName = "PayRegister.xlsx";
                //                            fileResponse.File = bytes;
                //                            fileResponse = fileResponse;
                //                        }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                fileResponse.File = "No";
                //                fileResponse.FileName = "Not Existing";
                //            }
                //        }
                //        else
                //        {
                //            fileResponse.File = "No";
                //            fileResponse.FileName = "Not Existing";
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        payregister_dt.Columns.Add("Exception", typeof(string));
                //        payregister_dt.Rows.Add(string.Format("{0},{1},{2}", ex.Message, ex.StackTrace, ex.InnerException));

                //    }

                //    //using var workbook = new XLWorkbook();
                //    //{
                //    //    var ws = workbook.AddWorksheet(list, "PayRegister_Regular");
                //    //    ws.Table(0).ShowAutoFilter = false;
                //    //    ws.Table(0).Theme = XLTableTheme.None;
                //    //    using (MemoryStream stream = new MemoryStream())
                //    //    {
                //    //        workbook.SaveAs(stream);
                //    //        var bytes = Convert.ToBase64String(stream.ToArray());
                //    //       // FileResponse fileResponse = new FileResponse();
                //    //        fileResponse.FileName = "Regular_PayRegister";
                //    //        fileResponse.File = bytes;
                //    //        return fileResponse;//File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PayRegister.xlsx");
                //    //    }
                //    //}
                //}
            }
            return fileResponse;
            
        }
        public async Task<InvoiceRequestResponseModel> InvoiceRequestRevoke(int reqNo, string invoiceType,int userId)
        {
            InvoiceRequestResponseModel responseModel = new InvoiceRequestResponseModel();
            var parameter = new DynamicParameters();
            parameter.Add("@Req_No", reqNo);
            parameter.Add("@Invoice_Type", invoiceType);
            parameter.Add("@CreatedBy", userId);
            string storeProcedure = "[dbo].[SP_PROC_Invoice_Request_revoked]" ?? "";
            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
            if (res != null)
            {
                responseModel = JsonConvert.DeserializeObject<List<InvoiceRequestResponseModel>>(res).FirstOrDefault();
                return responseModel;
            }
            else
            {
                responseModel.Error_Message = "Invoice revoked  falied";
                return responseModel;
            }
        }

        public async Task<InvoiceInitiationUI> InvoiceInitiate(int? TaxTypeId, string xml, string action, int userId)
        {
            InvoiceInitiationUI invoiceInitiationUI = new InvoiceInitiationUI();
            string storeProcedure = "[dbo].[Proc_ManageGstInvoiceInitiate_Online]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@xmlInput", xml ?? (object)DBNull.Value);
            parameter.Add("@mode", action ?? (object)DBNull.Value);
            parameter.Add("@CreatedBy", userId);
            try
            {
                var res =await _dbRepository.GetItemsAsync(storeProcedure, parameter);
                if(res!=null)
                {
                    var invoice= JsonConvert.DeserializeObject<List<InvoiceInitiationUI>>(res).FirstOrDefault();
                    if(invoice.Error_Message == "GST Invoice Initiated Successfully")
                    {
                        var param=new DynamicParameters();
                        param.Add("@UserId", userId);
                        var allot = await _dbRepository.GetItemsAsync("SP_AutoAllocation_Invoice", parameter);
                    }
                    return invoice;
                }
                else
                {
                    invoiceInitiationUI.Error_Message = "Invoice Geneated falied";
                    return invoiceInitiationUI;
                }
               

            }
            catch(Exception ex)
            {
              return  new InvoiceInitiationUI
                {
                    Error_Message = "GST Invoice not Initiated" 
                };
            }            
        }

        public async Task<InvoiceInitiationUI> ProformaToActualInvoiceInitiate(int? TaxTypeId, string xml, string action, int userId)
        {
            InvoiceInitiationUI invoiceInitiationUI = new InvoiceInitiationUI();
            string storeProcedure = "[dbo].[Proc_ManageGstInvoiceInitiate_Online_Detail]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@xmlInput", xml ?? (object)DBNull.Value);
            parameter.Add("@mode", action ?? (object)DBNull.Value);
            parameter.Add("@CreatedBy", userId);
            try
            {
                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
                if (res != null)
                {
                    var invoice = JsonConvert.DeserializeObject<List<InvoiceInitiationUI>>(res).FirstOrDefault();
                    if (invoice.Error_Message == "GST Invoice Initiated Successfully")
                    {
                        var param = new DynamicParameters();
                        param.Add("@UserId", userId);
                        var allot = await _dbRepository.GetItemsAsync("SP_AutoAllocation_Invoice", parameter);
                    }
                    return invoice;
                }
                else
                {
                    invoiceInitiationUI.Error_Message = "Invoice Geneated falied";
                    return invoiceInitiationUI;
                }


            }
            catch (Exception ex)
            {
                return new InvoiceInitiationUI
                {
                    Error_Message = "GST Invoice not Initiated"
                };
            }
        }
        
        public async Task<InvoiceInitiationUI> PostInvoiceQCDetail( string xml, int userId)
        {
            InvoiceInitiationUI invoiceInitiationUI = new InvoiceInitiationUI();
            string storeProcedure = "[dbo].[SP_PROC_Invoice_QC_Verification]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@XmlData", xml ?? (object)DBNull.Value);            
            parameter.Add("@CreatedBy", userId);
            try
            {
                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);
                if (res != null)
                {
                    var invoice = JsonConvert.DeserializeObject<List<InvoiceInitiationUI>>(res).FirstOrDefault();
                    return invoice;
                }
                else
                {
                    invoiceInitiationUI.Error_Message = "Invoice Geneated falied";
                    return invoiceInitiationUI;
                }


            }
            catch (Exception ex)
            {
                return new InvoiceInitiationUI
                {
                    Error_Message = "GST Invoice not Initiated"
                };
            }
        }
        public async Task<FileResponse> ExportToExcel(int? CompanyId, string PayPeriodId, int? TaxTypeId)
        {
            var fileResponse = new FileResponse();
            var parameter = new DynamicParameters();
            parameter.Add("@CompanyId",  CompanyId);
            parameter.Add("@PayPeriodId",  PayPeriodId);
            parameter.Add("@TaxTypeId",  TaxTypeId);

            try
            {
                // Get the JSON result from the repository
                var res = await _dbRepository.GetItemsAsync("Proc_GetAllInvoiceInitiateDetails2ExportToExcel", parameter);

                if (!string.IsNullOrEmpty(res))
                {
                    // Deserialize JSON into DataTable
                    var dt = JsonConvert.DeserializeObject<DataTable>(res) ?? new DataTable();

                    if (dt.Rows.Count > 0)
                    {
                        using var wb = new XLWorkbook();
                        wb.Worksheets.Add(dt, "InvoiceInitiate");

                        using var memoryStream = new MemoryStream();
                        wb.SaveAs(memoryStream);
                        var bytes = Convert.ToBase64String(memoryStream.ToArray());
                        fileResponse.File = bytes;
                        fileResponse.FileName = "InvoiceInitiate.xlsx";
                    }
                    else
                    {
                        fileResponse.File = "No";
                        fileResponse.FileName = "NoData.xlsx";
                    }
                }
                else
                {
                    fileResponse.File = "No";
                    fileResponse.FileName = "NoData.xlsx";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to export Invoice Initiate to Excel: " + ex.Message, ex);
            }

            return fileResponse;
        }
        //public async Task<string> ProvisionalInvoiceInitiate(string xml, int CreatedBy)
        //{
        //    InvoiceResponse invoiceDetails = new InvoiceResponse();
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Action", "CreateInvoice");
        //    parameters.Add("@XmlData", xml);
        //    parameters.Add("@UserId", CreatedBy);

        //    var res = await this._dbRepository.GetItemsAsync("Proc_QzoneInvoiceRequest_PRO", parameters);
        //    return res;
        //}

        public async Task<string> ProvisionalInvoiceInitiate(UI.Models.Invoice.ProvisionalInvoiceInitiateRequest provisionalrequest)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();
            string storeProcedure = "Proc_ManageInvoiceEmployeeDetail_NewUI";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "CreateInvoice");
            parameters.Add("@UserId", provisionalrequest.CreatedBy);
            parameters.Add("@CompanyId", provisionalrequest.CompanyId);
            parameters.Add("@CostCenterMappingId", provisionalrequest.Map_Name_Id);
            parameters.Add("@PayPeriodId", provisionalrequest.PayPeriodId);
            parameters.Add("@LotNumber", provisionalrequest.LotNo);
            parameters.Add("@InputNumber", provisionalrequest.Input_No);
            parameters.Add("@IsActive", provisionalrequest.Isactive);
            parameters.Add("@CreatedBy", provisionalrequest.CreatedBy);
            parameters.Add("@ModifiedBy", provisionalrequest.CreatedBy);
            parameters.Add("@PageNo", "1");
            parameters.Add("@PageSize", "10");
            parameters.Add("@Map_Name", provisionalrequest.Map_Name);
            parameters.Add("@Pay_Period", provisionalrequest.PayPeriod);
            parameters.Add("@InvoiceCultureId", provisionalrequest.InvoiceCulture_id);
            parameters.Add("@PO_Number", provisionalrequest.PO_Number);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            return res;
        }

        public async Task<string> VendorInvoiceInitiate(string xml, int CreatedBy)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Initiate");
            parameters.Add("@xmlData", xml);
            parameters.Add("@Created_By", CreatedBy);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageVendorGstInvoiceInitiate_NewUI", parameters);
            return res;
        }
        public async Task<string> MiscInvoiceInitiate(string xml, int CreatedBy)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Import");
            parameters.Add("@XmlData", xml);
            parameters.Add("@UserId", CreatedBy);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageMiscInvoiceInitiate", parameters);
            return res;
        }
        public async Task<DataSet> DraftExporttoExcel (InvoiceDetailModel invoiceDetailModel)
        {
            var parameters = new Dictionary<string, object?>
            {
            ["@InvoiceType"]= invoiceDetailModel.InvoiceType,
            ["@ActionType"]= invoiceDetailModel.ActionType,
            ["@UserId"]= invoiceDetailModel.userId
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Invoice_Initiation_search_Allot_Test", parameters, 1500);
        }
    }
}

