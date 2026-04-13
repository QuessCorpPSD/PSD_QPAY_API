using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Invoice;
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
    public class EInvoiceRepository : IEInvoiceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public EInvoiceRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }
        public async Task<DataSet> GetAllInvoiceDetails(int companyId, int payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Get",
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
                //["@UserId"] = userId,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageEInvoice_NewUI", parameters, 1500);
        }

        public async Task<DataSet> EInvoiceExport(int companyId, int payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "Export",
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
                //["@UserId"] = userId,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_ManageEInvoice_NewUI_2", parameters, 1500);
        }

        public DataSet GetInvoiceData(int invoiceId)
        {

            //var parameters = new DynamicParameters();
            //parameters.Add("@Company_Id", 0);
            //parameters.Add("@InvoiceId", invoiceId);
            //parameters.Add("@Action", "GetInvoiceHtml");
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = 0,
                ["@InvoiceId"] = invoiceId,
                ["@Action"] = "GetInvoiceHtml"
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetInvoiceDetails", parameters, 1500);
        }

        public UI.Invoice.EInvoice GetEInvoiceData(string invoiceIds, string UserId, string Action)
        {
            UI.Invoice.EInvoice einvoice = this._dbRepository.GetEInvoiceData(invoiceIds, UserId, Action);
            if (einvoice != null)
            {
                return einvoice;
            }
            else
            {
                throw new Exception("No data found for the given Invoices");
            }
        }

        public string SaveBatchResponse(int StatusCode, string ResponseMessage, string Response, string ResponseXml, string InvoiceIds, string Mode, string UserId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = Mode,
                ["@StatusCode"] = StatusCode,
                ["@ResponseMessage"] = ResponseMessage,
                ["@Response"] = Response,
                ["@XmlData"] = ResponseXml,
                ["@InvoiceIds"] = InvoiceIds,
                //["@Mode"] = Mode,
                ["@QzoneUserId"] = UserId,
            };
            return _dbRepository.GetString("Proc_ManageEInvoice_NewUI", parameters);
        }
        public async Task<DataSet> GetEInvoiceError(int invoiceId)
        {
            var parameters = new Dictionary<string, object?>
            {


                ["@Invoice_Id"] = invoiceId,
                ["@Company_Id"] = 0,
                ["@Pay_Period_Id"] = "0",
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Get_EInvoice_Error_Invoicewise", parameters, 1500);

        }

        public async Task<DataSet> GetEInvoiceErrorHover(int invoiceId)
        {
            var parameters = new Dictionary<string, object?>
            {


                ["@Invoice_Id"] = invoiceId,
                ["@Company_Id"] = 0,
                ["@Pay_Period_Id"] = "0",
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Get_EInvoice_Error_Invoicewise", parameters, 1500);

        }
        public async Task<InvoiceNumberLotUI> IRNStatusGenerationUpdate(string Invoice_Number)
        {
            string procedure = "SP_PayRegister_Invoice";
            var parameter = new DynamicParameters();
            parameter.Add("@Flag", "IRNUpdate");
            parameter.Add("@InvoiceNumber", Invoice_Number);
            var res = await this._dbRepository.GetItemsAsync(procedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {
                var resultList = JsonConvert.DeserializeObject<List<InvoiceNumberLotUI>>(res);

                return resultList.FirstOrDefault() ?? new InvoiceNumberLotUI();
            }
            else
            {
                return new InvoiceNumberLotUI();
            }
        }
        public async Task<InvoiceDetail> GetInvoiceDetailByInvoiceId(int invoiceId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Invoice_Id", invoiceId);
            string storedProcedure = "SP_IRN_GeneratedStatus_InvoiceDetail";

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {
                var resultList = JsonConvert.DeserializeObject<List<InvoiceDetail>>(res);

                return resultList.FirstOrDefault() ?? new InvoiceDetail();
            }
            else
            {
                return new InvoiceDetail();
            }

        }
        public async Task<ClientPeriodUI> CompanyPayPeriod(int payperiod)
        {
            var parameters = new DynamicParameters();
            string storeProcedure = "SP_GetCompanyCodeAndPayPeriod";
            parameters.Add("@PayPeriod", payperiod);
            var res = await this._dbRepository.GetItemsSecondaryAsync(storeProcedure, parameters);

            if (res != null)
            {
                var company = JsonConvert.DeserializeObject<List<ClientPeriodUI>>(res);
                return company.FirstOrDefault() ?? new ClientPeriodUI { Company_Code = "", Pay_Period = "" };
            }
            else
            {
                return new ClientPeriodUI { Company_Code = "", Pay_Period = "" };
            }
        }
        public async Task<List<UI.Invoice.InvoiceColors>> GetAllInvoiceTypeColors()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("Proc_IRN_Invoice_colors", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<UI.Invoice.InvoiceColors>>(res) ?? new List<UI.Invoice.InvoiceColors>();
            }

            return new List<UI.Invoice.InvoiceColors>();
        }

        //public FileResponse PayRegisterDownload(int companyCode, int pay_period_Id, string pay_period)
        //{
        //    FileResponse fileResponse = new FileResponse();
        //    DataTable payregister_dt = new DataTable();
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@Company_Id", companyCode);
        //    parameters.Add("@Pay_Period_Id", pay_period_Id);

        //    string storeProcedure = "";
        //    storeProcedure = "sp_PayRegister";

        //    var res = this._dbRepository.GetItemsSecondaryAsync(storeProcedure, parameters).Result;
        //    if (res != null)
        //    {
        //        try
        //        {
        //            payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);
        //            if (payregister_dt != null)
        //            {
        //                if (payregister_dt.Rows.Count > 0)
        //                {
        //                    DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
        //                    List<string> RemoveColums = new List<string>();
        //                    DataRow dtrow = payregister_dt.NewRow();
        //                    foreach (DataColumn column in payregister_dt.Columns)
        //                    {
        //                        var value = lastRow[column];

        //                        if (column.DataType.Name == "Double")
        //                        {

        //                            var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column)) ?? 0;
        //                            dtrow[column] = columnsum;
        //                            if (column.ColumnName.ToLower() == "lot_number")
        //                            {
        //                                var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
        //                                dtrow[column] = column_Unique[0];

        //                            }
        //                            if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
        //                            {
        //                                RemoveColums.Add(column.ToString());
        //                            }
        //                        }
        //                        else if (column.DataType.Name == "Int64")
        //                        {
        //                            var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column)) ?? 0;
        //                            dtrow[column] = columnsum;
        //                            if (column.ColumnName.ToLower() == "lot_number")
        //                            {
        //                                var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
        //                                dtrow[column] = column_Unique[0];

        //                            }
        //                            if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
        //                            {
        //                                RemoveColums.Add(column.ToString());
        //                            }


        //                        }
        //                        else
        //                        {
        //                            dtrow[column] = "";
        //                        }
        //                    }

        //                    payregister_dt.Rows.Add(dtrow);
        //                    foreach (var item in RemoveColums)
        //                    {
        //                        payregister_dt.Columns.Remove(item);
        //                    }
        //                    var emptyColumns = payregister_dt.Columns.Cast<DataColumn>()
        //                                   .Where(col => payregister_dt.AsEnumerable().All(row =>
        //                                   {
        //                                       var value = row[col];
        //                                       return value == null || string.IsNullOrWhiteSpace(value.ToString());
        //                                   }))
        //                                    .Select(col => col.ColumnName)
        //                                    .ToList();
        //                    foreach (var columnName in emptyColumns)
        //                        payregister_dt.Columns.Remove(columnName);



        //                    var comayName = CompanyNameByCode(companyCode);
        //                    var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();

        //                    DataTable payregistersummary_dt = new DataTable();
        //                    if (payregister_dt.Columns.Count > 1)
        //                    {
        //                        using var workbook = new XLWorkbook();
        //                        {
        //                            for (int i = 0; i < 2; i++)
        //                            {
        //                                if (i == 0)
        //                                {
        //                                    var ws = workbook.AddWorksheet(payregister_dt, "PayRegister");
        //                                    ws.Table(0).ShowAutoFilter = false;
        //                                    ws.Table(0).Theme = XLTableTheme.None;

        //                                    ws.Row(1).InsertRowsAbove(3);
        //                                    ws.Range("A1:Z1").Merge();
        //                                    ws.Range("A2:Z2").Merge();
        //                                    ws.Range("A3:Z3").Merge();

        //                                    var usedRange = ws.RangeUsed();

        //                                    if (usedRange != null)
        //                                    {
        //                                        foreach (var cell in usedRange.Cells())
        //                                        {
        //                                            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        //                                            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //                                            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        //                                            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //                                            cell.Style.Border.TopBorderColor = XLColor.Black;
        //                                            cell.Style.Border.BottomBorderColor = XLColor.Black;
        //                                            cell.Style.Border.LeftBorderColor = XLColor.Black;
        //                                            cell.Style.Border.RightBorderColor = XLColor.Black;
        //                                        }
        //                                    }

        //                                    ws.Cell(1, 1).Value = comapny.Client_Name;
        //                                    ws.Cell(1, 1).Style.Font.Bold = true;
        //                                    ws.Cell(2, 1).Value = string.Format("SALARY FOR THE MONTH OF {0}", pay_period);
        //                                    ws.Cell(2, 1).Style.Font.Bold = true;
        //                                    var lastrow = ws.LastRowUsed().RowNumber();

        //                                    ws.Cell(lastrow, 1).Value = "Grand Total";

        //                                    var totalsummary = GetPayRegisterSummary(companyCode, pay_period_Id);
        //                                    if (totalsummary != null)
        //                                    {
        //                                        if (totalsummary.Rows.Count > 0)
        //                                        {

        //                                            int row = 2;
        //                                            int cell = 5;
        //                                            double total = 0.0;
        //                                            double gst = 0.0;
        //                                            foreach (DataColumn item in totalsummary.Columns)
        //                                            {
        //                                                var columnName = item.ColumnName.ToString();
        //                                                if (columnName == "TOTAL COST TO COMPANY")
        //                                                {
        //                                                    columnName = string.Format("SALARY FOR THE MONTH OF {0}", pay_period);
        //                                                }
        //                                                var value = Convert.ToDouble(totalsummary.Rows[0][item.ColumnName]);
        //                                                if (Convert.ToDouble(value) > 0)
        //                                                {
        //                                                    total = total + value;
        //                                                    ws.Cell(lastrow + row, 4).Value = columnName;
        //                                                    var column = ws.Cell(lastrow + row, 4);
        //                                                    column.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        //                                                    column.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //                                                    column.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        //                                                    column.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //                                                    ws.Cell(lastrow + row, cell).Value = value;
        //                                                    var ctc_cell = ws.Cell(lastrow + row, cell);
        //                                                    ctc_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        //                                                    ctc_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //                                                    ctc_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        //                                                    ctc_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        //                                                    row++;

        //                                                }

        //                                            }
        //                                            ws.Cell(lastrow + row, 4).Value = "Sub Total";
        //                                            var Sub_title = ws.Cell(lastrow + row, 4);
        //                                            Sub_title.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        //                                            Sub_title.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //                                            Sub_title.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        //                                            Sub_title.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //                                            ws.Cell(lastrow + row, 5).Value = total;
        //                                            var sub_total = ws.Cell(lastrow + row, 5);
        //                                            sub_total.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        //                                            sub_total.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //                                            sub_total.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        //                                            sub_total.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //                                            row++;
        //                                            //cell++;

        //                                            ws.Cell(lastrow + row, 4).Value = "GST";
        //                                            var gst_title = ws.Cell(lastrow + row, 4);
        //                                            gst_title.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        //                                            gst_title.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //                                            gst_title.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        //                                            gst_title.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //                                            gst = total * (18.0 / 100.0);

        //                                            ws.Cell(lastrow + row, 5).Value = gst;
        //                                            var gst_value = ws.Cell(lastrow + row, 5);
        //                                            gst_value.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        //                                            gst_value.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        //                                            gst_value.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        //                                            gst_value.Style.Border.RightBorder = XLBorderStyleValues.Thin;

        //                                            row++;
        //                                            // cell++;

        //                                            ws.Cell(lastrow + row, 4).Value = "Total";
        //                                            ws.Cell(lastrow + row, 5).Value = total + gst;

        //                                            ws.Cell(lastrow + row, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //                                            ws.Cell(lastrow + row, 4).Style.Border.OutsideBorderColor = XLColor.Black;


        //                                            ws.Cell(lastrow + row, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //                                            ws.Cell(lastrow + row, 5).Style.Border.OutsideBorderColor = XLColor.Black;

        //                                        }
        //                                    }
        //                                }

        //                            }

        //                            using (MemoryStream stream = new MemoryStream())
        //                            {
        //                                workbook.SaveAs(stream);
        //                                var bytes = Convert.ToBase64String(stream.ToArray());
        //                                //  FileResponse fileResponse = new FileResponse();
        //                                fileResponse.FileName = "PayRegister.xlsx";
        //                                fileResponse.File = bytes;

        //                            }

        //                        }

        //                    }
        //                    else
        //                    {
        //                        using (MemoryStream stream = new MemoryStream())
        //                        {

        //                            using var workbook = new XLWorkbook();
        //                            {
        //                                workbook.SaveAs(stream);
        //                                var bytes = Convert.ToBase64String(stream.ToArray());

        //                                fileResponse.FileName = "PayRegister.xlsx";
        //                                fileResponse.File = bytes;
        //                                fileResponse = fileResponse;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    fileResponse.File = "No";
        //                    fileResponse.FileName = "Not Existing";
        //                }
        //            }
        //            else
        //            {
        //                fileResponse.File = "No";
        //                fileResponse.FileName = "Not Existing";
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            payregister_dt.Columns.Add("Exception", typeof(string));
        //            payregister_dt.Rows.Add(string.Format("{0},{1},{2}", ex.Message, ex.StackTrace, ex.InnerException));

        //        }
        //    }
        //    else
        //    {
        //        fileResponse.File = "No";
        //        fileResponse.FileName = "Not Existing";
        //    }

        //    return fileResponse;
        //}

        public DataTable PayRegisterDownload(int companyCode, int pay_period_Id, string pay_period)
        {
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_Id);

            string storeProcedure = "";
            storeProcedure = "sp_PayRegister";

            var res = this._dbRepository.GetItemsSecondaryAsync(storeProcedure, parameters).Result;
            if (res != null)
            {
                try
                {
                    payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);
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



                            var comayName = CompanyNameByCode(companyCode);
                            var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();

                            DataTable payregistersummary_dt = new DataTable();
                            if (payregister_dt.Columns.Count > 1)
                            {
                                using var workbook = new XLWorkbook();
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        if (i == 0)
                                        {
                                            var ws = workbook.AddWorksheet(payregister_dt, "PayRegister");
                                            ws.Table(0).ShowAutoFilter = false;
                                            ws.Table(0).Theme = XLTableTheme.None;

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
                                            ws.Cell(2, 1).Value = string.Format("SALARY FOR THE MONTH OF {0}", pay_period);
                                            ws.Cell(2, 1).Style.Font.Bold = true;
                                            var lastrow = ws.LastRowUsed().RowNumber();

                                            ws.Cell(lastrow, 1).Value = "Grand Total";

                                            var totalsummary = GetPayRegisterSummary(companyCode, pay_period_Id);
                                            if (totalsummary != null)
                                            {
                                                if (totalsummary.Rows.Count > 0)
                                                {

                                                    int row = 2;
                                                    int cell = 5;
                                                    double total = 0.0;
                                                    double gst = 0.0;
                                                    foreach (DataColumn item in totalsummary.Columns)
                                                    {
                                                        var columnName = item.ColumnName.ToString();
                                                        if (columnName == "TOTAL COST TO COMPANY")
                                                        {
                                                            columnName = string.Format("SALARY FOR THE MONTH OF {0}", pay_period);
                                                        }
                                                        var value = Convert.ToDouble(totalsummary.Rows[0][item.ColumnName]);
                                                        if (Convert.ToDouble(value) > 0)
                                                        {
                                                            total = total + value;
                                                            ws.Cell(lastrow + row, 4).Value = columnName;
                                                            var column = ws.Cell(lastrow + row, 4);
                                                            column.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                            column.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                            column.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                            column.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                            ws.Cell(lastrow + row, cell).Value = value;
                                                            var ctc_cell = ws.Cell(lastrow + row, cell);
                                                            ctc_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                            ctc_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                            ctc_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                            ctc_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                                            row++;

                                                        }

                                                    }
                                                    ws.Cell(lastrow + row, 4).Value = "Sub Total";
                                                    var Sub_title = ws.Cell(lastrow + row, 4);
                                                    Sub_title.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                    Sub_title.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                    Sub_title.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                    Sub_title.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                    ws.Cell(lastrow + row, 5).Value = total;
                                                    var sub_total = ws.Cell(lastrow + row, 5);
                                                    sub_total.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                    sub_total.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                    sub_total.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                    sub_total.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                    row++;
                                                    //cell++;

                                                    ws.Cell(lastrow + row, 4).Value = "GST";
                                                    var gst_title = ws.Cell(lastrow + row, 4);
                                                    gst_title.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                    gst_title.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                    gst_title.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                    gst_title.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                    gst = total * (18.0 / 100.0);

                                                    ws.Cell(lastrow + row, 5).Value = gst;
                                                    var gst_value = ws.Cell(lastrow + row, 5);
                                                    gst_value.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                    gst_value.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                    gst_value.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                    gst_value.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                    row++;
                                                    // cell++;

                                                    ws.Cell(lastrow + row, 4).Value = "Total";
                                                    ws.Cell(lastrow + row, 5).Value = total + gst;

                                                    ws.Cell(lastrow + row, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                                    ws.Cell(lastrow + row, 4).Style.Border.OutsideBorderColor = XLColor.Black;


                                                    ws.Cell(lastrow + row, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                                    ws.Cell(lastrow + row, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                                                }
                                            }
                                        }

                                    }

                                }

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    payregister_dt.Columns.Add("Exception", typeof(string));
                    payregister_dt.Rows.Add(string.Format("{0},{1},{2}", ex.Message, ex.StackTrace, ex.InnerException));

                }
            }
            return payregister_dt;
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

        public string CompanyNameByCode(int company_Id)
        {
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", company_Id);
            string storeProcedure = "Sp_GetCompany_name";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res != null)
            {
                return res;
            }
            return "";
        }

        public DataTable GetPayRegisterSummary(int companyCode, int pay_period_Id)
        {
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            string storeProcedure = "sp_PayregisterPSDSummary";
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_Id);
            var res = this._dbRepository.GetItemsSecondaryAsync(storeProcedure, parameters).Result;
            if (res != null)
            {

                try
                {
                    payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                }
                catch (Exception ex)
                {

                }
            }
            return payregister_dt;
        }
        public async Task<DataTable> GetInvoiceSummaryByInvoiceId(string InvoiceNumber)
        {
            DataTable data = new DataTable();
            var paramter = new DynamicParameters();
            paramter.Add("@invoiceNumber", InvoiceNumber);
            var result = await _dbRepository.GetItemsAsync("SpInvoiceDetails_Report_InvoiceWiseSummary", paramter);
            data = (DataTable)JsonConvert.DeserializeObject<DataTable>(result);
            return data;
        }

        public async Task<DataSet> GetConsolidateInvoiceSummary(int companyId, int payperiodid)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = companyId,
                ["@Pay_Period_id"] = payperiodid,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SpInvoiceDetails_Report_Companywise", parameters, 1500);
        }

        public async Task<DataSet> NetPaySummaryByCompanyIDAndPayperiodId( int companyId,int pay_period_Id)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = companyId,
                ["@Pay_Period_id"] = pay_period_Id,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SpInvoiceDetails_Report_Companywise", parameters, 1500);
        }

        public async Task<DataSet> GetConsolidatePayRegister(int companyId, int payperiodid)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = companyId,
                ["@Pay_Period_id"] = payperiodid,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SpInvoiceDetails_Report_Companywise", parameters, 1500);
        }

        public async Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
           [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "Invoice", "Attributes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Attributes_{CompanyId}_{payperiodId}_{datePrefix}{extension}";

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

                DataTable dtToSerialize = ds.Tables[0];

                if (!dtToSerialize.Columns.Contains("Company_Id"))
                    dtToSerialize.Columns.Add("Company_Id", typeof(int));

                if (!dtToSerialize.Columns.Contains("PayPeriod_Id"))
                    dtToSerialize.Columns.Add("PayPeriod_Id", typeof(int));

                if (!dtToSerialize.Columns.Contains("Invoice_Number"))
                    dtToSerialize.Columns.Add("Invoice_Number", typeof(string));

                // Add extra columns that SQL expects
                if (!dtToSerialize.Columns.Contains("Narration"))
                    dtToSerialize.Columns.Add("Narration", typeof(string));

                if (!dtToSerialize.Columns.Contains("PO_Number"))
                    dtToSerialize.Columns.Add("PO_Number", typeof(string));

                //if (!dtToSerialize.Columns.Contains("GL_Code"))
                //    dtToSerialize.Columns.Add("GL_Code", typeof(string));

                //if (!dtToSerialize.Columns.Contains("Cost_Center_Name"))
                //    dtToSerialize.Columns.Add("Cost_Center_Name", typeof(string));

                //if (!dtToSerialize.Columns.Contains("Client_SPOC_Name"))
                //    dtToSerialize.Columns.Add("Client_SPOC_Name", typeof(string));

                //if (!dtToSerialize.Columns.Contains("Work_Order_Number"))
                //    dtToSerialize.Columns.Add("Work_Order_Number", typeof(string));

                foreach (DataRow row in dtToSerialize.Rows)
                {
                    row["Company_Id"] = CompanyId;   // or actual PayPeriod from UI
                    row["PayPeriod_Id"] = payperiodId;
                }

                foreach (DataRow row in dtToSerialize.Rows)
                {
                    foreach (DataColumn col in dtToSerialize.Columns)
                    {
                        if (row.IsNull(col))
                            row[col] = string.Empty; // replace DBNull with empty string
                    }
                }


                // Convert to XML
                using var xmlWriter = new StringWriter();
                dtToSerialize.TableName = "Table";  // Required for SQL XQuery
                DataSet xmlDS = new DataSet("NewDataSet");
                xmlDS.Tables.Add(dtToSerialize.Copy());

                xmlDS.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string storeProcedure = "Proc_Upload_GSTInvoice_Attributes";
                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

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
    }
}
