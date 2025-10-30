using Azure;
using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.DAL.Repository;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QPay.BAL.Repository
{
    public class PayRegisterRepository : IPayRegisterRepository 
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _config;
        private readonly string[] _grandTotalWhilteList;
        private readonly string[] _companyCode; 

        public PayRegisterRepository(DbRepository dbRepository, IConfiguration config)
        {
            this._dbRepository=dbRepository;
            this._config = config;
            this._grandTotalWhilteList = _config.GetSection("whiteListedColumn:columnName").Get<string[]>() ?? Array.Empty<string>();
            this._companyCode = _config.GetSection("OtherIncome:companyCode").Get<string[]>() ?? Array.Empty<string>();


        }

        public string CompanyNameByCode(int company_Id)
        {
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", company_Id);
            string storeProcedure = "Sp_GetCompany_name";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!=null)
            {
                return res;
            }
            return "";
        }

        public FileResponse GetOtherIncomePayRegister(int companyCode, int pay_period_Id, int lotNumber,string company_Code)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            string storeProcedure = "";
            if (company_Code != "" && _companyCode.Contains(company_Code))
            {
                parameters.Add("@Company_ID", companyCode);
                parameters.Add("@Pay_Frequency_Detail_Id", pay_period_Id);
                parameters.Add("@PO_NUMBER", "");
                parameters.Add("@INPUTNUMBER", lotNumber);
                storeProcedure = "sp_OtherIncome_Report_PONUMBER_ExportToExcel";
            }
            else
            {
                parameters.Add("@Company_ID", companyCode);
                parameters.Add("@Pay_Frequency_Detail_Id", pay_period_Id);
                parameters.Add("@PO_NUMBER", "");
                parameters.Add("@INPUTNUMBER", lotNumber);
                storeProcedure = "sp_OtherIncome_Report_PONUMBER_ExportToExcel";
            }

                var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!=null)
            {
                payregister_dt =(DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                if(payregister_dt!=null)
                {
                    if (payregister_dt.Rows.Count>0)
                    {

                        DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
                        List<string> RemoveColums = new List<string>();
                        DataRow dtrow = payregister_dt.NewRow();
                        foreach (DataColumn column in payregister_dt.Columns)
                        {
                            var value = lastRow[column];

                            if (column.DataType.Name=="Double")
                            {
                                var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
                                if (column_Unique.Count()>1 || column.ColumnName.ToLower() == ("Service_charge").ToLower())
                                {
                                    var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column));
                                    dtrow[column]=columnsum;
                                    if (Convert.ToString(columnsum).ToLower()==("0").ToLower())
                                    {
                                        RemoveColums.Add(column.ToString());
                                    }
                                }
                                else
                                {
                                    dtrow[column]=column_Unique[0];
                                    if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                                    {
                                        RemoveColums.Add(column.ToString());
                                    }
                                }
                            }
                            else if (column.DataType.Name=="Int64")
                            {
                                var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                                if (column_Unique.Count()>1)
                                {

                                    var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column));
                                    dtrow[column]=columnsum;
                                    if (Convert.ToString(columnsum).ToLower()==("0").ToLower())
                                    {
                                        RemoveColums.Add(column.ToString());
                                    }

                                }
                                else
                                {
                                    dtrow[column]=column_Unique[0];
                                    if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                                    {
                                        RemoveColums.Add(column.ToString());
                                    }
                                }

                            }
                            else
                            {
                               // dtrow[column]="";
                            }
                        }

                        foreach (var item in RemoveColums)
                        {
                            payregister_dt.Columns.Remove(item);
                        }
                        using var workbook = new XLWorkbook();
                        {
                            var ws = workbook.AddWorksheet(payregister_dt, "Other Income");
                            ws.Table(0).ShowAutoFilter = false;
                            ws.Table(0).Theme = XLTableTheme.None;
                            //var ctc = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("CTC"));
                            double service = 0.0;
                            double ctc = 0.0;

                            if (payregister_dt.Columns.Contains("SERCG"))
                            {
                                service = payregister_dt.AsEnumerable()
                                    .Where(row => row.Field<double?>("SERCG").HasValue)
                                    .Sum(row => row.Field<double?>("SERCG").Value);
                            }

                            if (payregister_dt.Columns.Contains("CTC"))
                            {
                                ctc = payregister_dt.AsEnumerable()
                                    .Where(row => row.Field<double?>("CTC").HasValue)
                                    .Sum(row => row.Field<double?>("CTC").Value);
                            }

                            var comayName = CompanyNameByCode(companyCode);
                            var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                            ws.Row(1).InsertRowsAbove(1);
                            ws.Range("A1:B1").Merge();
                            ws.Cell(1, 1).Value = comapny.Client_Name;
                            ws.Cell(1, 1).Style.Font.Bold=true;
                            var lastrow = ws.LastRowUsed().RowNumber();

                            if (ctc!=null && service!=null)
                            {
                                var Total = ctc+service;
                                var toal_GST = Total*(18.0/100.0);
                              //  ws.Cell(lastrow, 1).Value = "Grand Total";
                            
                                ws.Cell(lastrow + 3, 4).Value = comapny.Client_Name;



                                var clinet_cell = ws.Cell(lastrow + 3, 4);
                                clinet_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                clinet_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                clinet_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                clinet_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;



                                ws.Cell(lastrow + 3, 5).Value = ctc;
                                var ctc_cell = ws.Cell(lastrow + 3, 5);
                                ctc_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                ctc_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                ctc_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                ctc_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                ws.Cell(lastrow + 4, 4).Value = "Service Charge:";
                                var Service_cell = ws.Cell(lastrow + 4, 4);
                                Service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                Service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                Service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                Service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                ws.Cell(lastrow + 4, 5).Value = service;
                                var service_cell = ws.Cell(lastrow + 4, 5);
                                service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                ws.Cell(lastrow + 5, 4).Value = "Total";
                                var empty_cell = ws.Cell(lastrow + 5, 4);
                                empty_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                empty_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                empty_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                empty_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                ws.Cell(lastrow + 5, 5).Value = Total;
                                var Total_cell = ws.Cell(lastrow + 5, 5);
                                Total_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                Total_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                Total_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                Total_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                ws.Cell(lastrow + 6, 4).Value = "Total GST";
                                var Total1_cell = ws.Cell(lastrow + 6, 4);
                                Total1_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                Total1_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                Total1_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                Total1_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                ws.Cell(lastrow + 6, 5).Value = toal_GST;
                                var toal_GST_cell = ws.Cell(lastrow + 6, 5);
                                toal_GST_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                toal_GST_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                toal_GST_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                toal_GST_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;


                            }
                           
                            using (MemoryStream stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var bytes = Convert.ToBase64String(stream.ToArray());
                                //  FileResponse fileResponse = new FileResponse();
                                fileResponse.FileName="Other Income.xlsx";
                                fileResponse.File=bytes;

                            }
                        }
                    }
                    else
                    {
                        fileResponse.File="No";
                        fileResponse.FileName="Not Existing";
                    }
                }
                else
                {
                    fileResponse.File="No";
                    fileResponse.FileName="Not Existing";
                }
            }
            else
            {
                fileResponse.File="No";
                fileResponse.FileName="Not Existing";
            }

            return fileResponse;
        }
        bool ColumnExists(DataTable table, string columnName)
        {
            return table.Columns.Contains(columnName);
        }
        public FileResponse GetQCOtherIncomePayRegister(int companyCode, int pay_period_Id, int lotNumber, string pay_period,string Company_Code)
        {
           
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            string storeProcedure = "";
            if (Company_Code != "" && _companyCode.Contains(Company_Code))
            {
                parameters.Add("@Company_ID", companyCode);
                parameters.Add("@Pay_Frequency_Detail_Id", pay_period_Id);
                parameters.Add("@PO_NUMBER", "");
                parameters.Add("@INPUTNUMBER", lotNumber);
                storeProcedure = "sp_OtherIncome_Report_PONUMBER_ExportToExcel";

            }
            else
            {
                
                parameters.Add("@Company_ID", companyCode);
                parameters.Add("@Pay_Frequency_Detail_Id", pay_period_Id);
                parameters.Add("@PayCode", "");
                parameters.Add("@Inputno", lotNumber);
                storeProcedure = "sp_OtherIncome_Report_Pivot_ExportToExcel_PSD";
                
            }
            //parameters.Add("@Company_ID", companyCode);
            //parameters.Add("@Pay_Frequency_Detail_Id", pay_period_Id);
            //parameters.Add("@PO_NUMBER", "");
            //parameters.Add("@INPUTNUMBER", lotNumber);
            //string storeProcedure = "sp_OtherIncome_Report_PONUMBER_ExportToExcel";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!=null)
            {
                payregister_dt =(DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                if (payregister_dt!=null)
                {
                    if (payregister_dt.Rows.Count>0)
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

                                    //var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
                                    //if (column_Unique.Count()>1 || column.ColumnName.ToLower() == ("Service_charge").ToLower())
                                    //{

                                    //}
                                    //else
                                    //{
                                    //    dtrow[column]=column_Unique[0];
                                    //    if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                                    //    {
                                    //        RemoveColums.Add(column.ToString());
                                    //    }
                                    //}
                                }
                                else if (column.DataType.Name == "Int64")
                                {
                                    var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column));
                                    dtrow[column] = columnsum;
                                    if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                                    {
                                        RemoveColums.Add(column.ToString());
                                    }

                                    //var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                                    //if (column_Unique.Count()>1)
                                    //{


                                    //}
                                    //else
                                    //{
                                    //    dtrow[column]=column_Unique[0];
                                    //    if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                                    //    {
                                    //        RemoveColums.Add(column.ToString());
                                    //    }
                                    //}

                                }
                                else
                                {
                                    // dtrow[column]="";
                                }
                            }
                        }
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
                            
                           
                            var comayName = CompanyNameByCode(companyCode);
                            var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                            
                            ws.Range("A1:Z1").Merge();
                            ws.Range("A2:Z2").Merge();
                            ws.Range("A3:Z3").Merge();
                            

                            ws.Cell(1, 1).Value = comapny.Client_Name;
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
                            ws.Cell(2, 1).Value ="ONETIME FOR THE MONTH OF " +pay_period;
                            ws.Cell(2, 1).Style.Font.Bold=true;
                            ws.Cell(2, 1).Style.Font.Underline=XLFontUnderlineValues.Single;
                            var headerRange = ws.Row(4);
                            headerRange.Style.Font.Bold = true;

                            
                            var lastrow = ws.LastRowUsed().RowNumber();
                            int lastCol = ws.LastColumnUsed().ColumnNumber();
                            var rowRange = ws.Range(4, 1, lastrow, lastCol); // Rows 2–5, all used columns
                            rowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            rowRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            ws.Cell(lastrow, 1).Value = "Grand Total";
                            //ws.Cell(lastrow, 1).Style.Font.Bold = true;
                            //ws.Cell(lastrow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                            //ws.Cell(lastrow, 1).Style.Border.OutsideBorderColor = XLColor.Black;
                            if (ctc>0 )
                            {
                                var Total = ctc+service;
                                var toal_GST = Total*(18.0/100.0);


                                ws.Cell(lastrow + 2, 4).Value = "ONETIME FOR THE MONTH OF " + pay_period;//comapny.Client_Name;
                                var clinet_cell = ws.Cell(lastrow + 2, 4);
                                clinet_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                clinet_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 2, 5).Value = ctc;
                                var ctc_cell = ws.Cell(lastrow + 2, 5);
                                ctc_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                ctc_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 3, 4).Value = "Service Charge:";
                                var Service_cell = ws.Cell(lastrow + 3, 4);
                                Service_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                Service_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 3, 5).Value = service;
                                var service_cell = ws.Cell(lastrow + 3, 5);
                                service_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                service_cell.Style.Border.OutsideBorderColor = XLColor.Black;


                                ws.Cell(lastrow + 4, 4).Value = "Sub Total";
                                var empty_cell = ws.Cell(lastrow + 4, 4);
                                empty_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                empty_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 4, 5).Value = Total;
                                var Total_cell = ws.Cell(lastrow + 4, 5);                                
                                Total_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                Total_cell.Style.Border.OutsideBorderColor = XLColor.Black;


                                ws.Cell(lastrow + 5, 4).Value = "GST";
                                var Total1_cell = ws.Cell(lastrow + 5, 4);
                                Total1_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                Total1_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 5, 5).Value = toal_GST;
                                var toal_GST_cell = ws.Cell(lastrow + 5, 5);                                
                                toal_GST_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                toal_GST_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 6, 4).Value = "Total";
                                ws.Cell(lastrow + 6, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                ws.Cell(lastrow + 6, 4).Style.Border.OutsideBorderColor = XLColor.Black;
                                ws.Cell(lastrow + 6, 5).Value = Total+toal_GST;
                                ws.Cell(lastrow + 6, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                ws.Cell(lastrow + 6, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                            }
                            ws.Columns().AdjustToContents(); // Auto fit all columns
                            ws.Rows().AdjustToContents();    // Auto fit all rows
                            //var usedRange = ws.RangeUsed();

                            //if (usedRange != null)
                            //{
                            //    // Apply medium border to all sides of each cell
                            //    usedRange.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            //    usedRange.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                            //    usedRange.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            //    usedRange.Style.Border.RightBorder = XLBorderStyleValues.Medium;

                            //    // Optional: set color
                            //    usedRange.Style.Border.TopBorderColor = XLColor.RichBlack;
                            //    usedRange.Style.Border.BottomBorderColor = XLColor.RichBlack;
                            //    usedRange.Style.Border.LeftBorderColor = XLColor.RichBlack;
                            //    usedRange.Style.Border.RightBorderColor = XLColor.RichBlack;
                            //}

                            using (MemoryStream stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                stream.Seek(0, SeekOrigin.Begin);
                                var bytes = Convert.ToBase64String(stream.ToArray());
                                //  FileResponse fileResponse = new FileResponse();
                                fileResponse.FileName="Other Income.xlsx";
                                fileResponse.File=bytes;

                            }
                        }
                    }
                    else
                    {
                        fileResponse.File="No";
                        fileResponse.FileName="Not Existing";
                    }
                }
                else
                {
                    fileResponse.File="No";
                    fileResponse.FileName="Not Existing";
                }
            }
            else
            {
                fileResponse.File="No";
                fileResponse.FileName="Not Existing";
            }

            return fileResponse;
        }

        public PayRegisterQzoneResponse GetFileNameFromQzone(int companyCode, int pay_period_Id, int lotNumber)
        {
            string query = "select Company_Id,PayPeriod_id,Lot_Number,FileName from tbl_InputStatus_LotNumber where Company_Id='" + companyCode + "' and PayPeriod_id='"+ pay_period_Id + "' and Lot_Number='"+ lotNumber + "'";
            var res = _dbRepository.QueryMultiAsync(query).Result;
            try
            {
              var  responses = JsonConvert.DeserializeObject<List<PayRegisterQzoneResponse>>(res).FirstOrDefault()
                                                 ;
                return responses;
                //var files= responses.FirstOrDefault();

            }
            catch (System.Text.Json.JsonException ex)
            {
                // Log the error if needed
                return new PayRegisterQzoneResponse();
            }
            
        }
        public DataTable GetPayRegisterSummary(int companyCode, int pay_period_Id, int lotNumber)
        {
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            string storeProcedure = "sp_PayregisterLotwisePSDSummary";
            parameters.Add("@Company_Id", companyCode);
                parameters.Add("@Pay_Period_Id", pay_period_Id);
                parameters.Add("@Lot_number", lotNumber);
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
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
        public FileResponse PayRegisterDownload(int companyCode, int pay_period_Id, int lotNumber,string pay_period,int revised)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_Id);
            parameters.Add("@Lot_No", lotNumber);
            //parameters.Add("@Revised", revised);
            parameters.Add("@isInvoice", 0);

            string storeProcedure = "sp_PayRegister_LotWise";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!=null)
            {
                try
                {
                    payregister_dt =(DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                    if(payregister_dt!=null )
                    {
                        if (payregister_dt.Rows.Count>0)
                        {
                            DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
                            List<string> RemoveColums = new List<string>();
                            DataRow dtrow = payregister_dt.NewRow();
                            foreach (DataColumn column in payregister_dt.Columns)
                            {
                                var value = lastRow[column];

                                if (column.DataType.Name=="Double")
                                {

                                    var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column)) ?? 0;
                                    dtrow[column] = columnsum;
                                    if(column.ColumnName.ToLower()== "lot_number")
                                    {
                                        var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
                                        dtrow[column] = column_Unique[0];

                                    }
                                    if (Convert.ToString(columnsum).ToLower() == ("0").ToLower())
                                    {
                                        RemoveColums.Add(column.ToString());
                                    }

                                    //var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
                                    //if (column_Unique.Count()>1 || this._grandTotalWhilteList.Contains(column.ColumnName)) //column.ColumnName.ToLower() == ("Service_charge").ToLower())
                                    //{
                                    //    var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column));
                                    //    dtrow[column]=columnsum;
                                    //    if (Convert.ToString(columnsum).ToLower()==("0").ToLower())
                                    //    {
                                    //        RemoveColums.Add(column.ToString());
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    if (dtrow[""].ToString().ToLower() == "lot_number")
                                    //    {
                                    //        dtrow[column] = column_Unique[0];
                                    //    }
                                    //    if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                                    //    {
                                    //        RemoveColums.Add(column.ToString());
                                    //    }
                                    //}
                                }
                                else if (column.DataType.Name=="Int64")
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
                                    //var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                                    //if (column_Unique.Count()>1)
                                    //{


                                    //}
                                    //else
                                    //{
                                    //    dtrow[column]=column_Unique[0];
                                    //    if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                                    //    {
                                    //        RemoveColums.Add(column.ToString());
                                    //    }
                                    //}

                                }
                                else
                                {
                                    dtrow[column]="";
                                }
                            }
                            //var service = 0.0;
                           // var ctc = 0.0;// payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("TOTAL COST TO COMPANY"));
                             //service = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("Service_charge"));
                            //if (payregister_dt.Columns.Contains("Service_charge"))
                            //{
                            //    service = payregister_dt.AsEnumerable()
                            //        .Sum(row => row.Field<double?>("Service_charge") ?? 0);
                            //}
                            //if (payregister_dt.Columns.Contains("TOTAL COST TO COMPANY"))
                            //{
                            //    ctc = payregister_dt.AsEnumerable()
                            //        .Sum(row => row.Field<double?>("TOTAL COST TO COMPANY") ?? 0);
                            //}
                            payregister_dt.Rows.Add(dtrow);
                            // Process the value as needed
                            foreach (var item in RemoveColums)
                            {
                                payregister_dt.Columns.Remove(item);
                            }

                            

                            var comayName = CompanyNameByCode(companyCode);
                            var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                            var parameter = new DynamicParameters();
                            parameter.Add("@Company_code", companyCode);
                            parameter.Add("@Pay_Period", pay_period_Id);
                            parameter.Add("@Lot_No", lotNumber);
                            DataTable payregistersummary_dt = new DataTable();
                            var payregistersummary = _dbRepository.GetItemsAsync("SP_PayRegister_LotWise_Summary", parameter).Result;
                            if (payregistersummary!=null)
                            {
                                payregistersummary_dt=(DataTable)JsonConvert.DeserializeObject<DataTable>(payregistersummary);
                            }


                            //  wb.Worksheets.Add(dataTable);
                            if (payregister_dt.Columns.Count > 1)
                            {
                                using var workbook = new XLWorkbook();
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        if (i==0)
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
                                            ws.Cell(1, 1).Style.Font.Bold=true;
                                            ws.Cell(2, 1).Value = string.Format("SALARY FOR THE MONTH OF {0}", pay_period);
                                            ws.Cell(2, 1).Style.Font.Bold = true;
                                            var lastrow = ws.LastRowUsed().RowNumber();

                                            //if (ctc!=null && service!=null)
                                            //{
                                                //var Total = ctc+service;
                                                //var toal_GST = Total*(18.0/100.0);
                                                ws.Cell(lastrow, 1).Value = "Grand Total";

                                               // ws.Cell(lastrow + 3, 4).Value = string.Format("SALARY FOR THE MONTH OF {0}", pay_period);



                                            //var clinet_cell = ws.Cell(lastrow + 3, 4);
                                            //clinet_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                            //clinet_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                            //clinet_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                            //clinet_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                            var totalsummary=  GetPayRegisterSummary(companyCode, pay_period_Id, lotNumber);
                                                if(totalsummary!=null)
                                                {
                                                    if(totalsummary.Rows.Count>0)
                                                    {

                                                        int row = 2;
                                                        int cell = 5;
                                                        double total=0.0;
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
                                                        //if(item.ColumnName.ToLower()== "TOTAL COST TO COMPANY")
                                                        //{

                                                        //}

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


                                                //ws.Cell(lastrow + 3, 5).Value = ctc;
                                                //var ctc_cell = ws.Cell(lastrow + 3, 5);
                                                //ctc_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                //ctc_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                //ctc_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                //ctc_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                //ws.Cell(lastrow + 4, 4).Value = "Service Charge:";
                                                //var Service_cell = ws.Cell(lastrow + 4, 4);
                                                //Service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                //Service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                //Service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                //Service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                //ws.Cell(lastrow + 4, 5).Value = service;
                                                //var service_cell = ws.Cell(lastrow + 4, 5);
                                                //service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                //service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                //service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                //service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                //ws.Cell(lastrow + 5, 4).Value = "Sub Total";
                                                //var empty_cell = ws.Cell(lastrow + 5, 4);
                                                //empty_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                //empty_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                //empty_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                //empty_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                //ws.Cell(lastrow + 5, 5).Value = Total;
                                                //var Total_cell = ws.Cell(lastrow + 5, 5);
                                                //Total_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                //Total_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                //Total_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                //Total_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                //ws.Cell(lastrow + 6, 4).Value = "GST";
                                                //var Total1_cell = ws.Cell(lastrow + 6, 4);
                                                //Total1_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                //Total1_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                //Total1_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                //Total1_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                //ws.Cell(lastrow + 6, 5).Value = toal_GST;
                                                //var toal_GST_cell = ws.Cell(lastrow + 6, 5);
                                                //toal_GST_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                                                //toal_GST_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                                //toal_GST_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                                //toal_GST_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                                                //ws.Cell(lastrow + 7, 4).Value = "Total";
                                                //ws.Cell(lastrow + 7, 5).Value = Total+toal_GST;

                                                //ws.Cell(lastrow + 7, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                                //ws.Cell(lastrow + 7, 4).Style.Border.OutsideBorderColor = XLColor.Black;


                                                //ws.Cell(lastrow + 7, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                                //ws.Cell(lastrow + 7, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                                            //}

                                           


                                        }

                                        else
                                        {
                                            var ws = workbook.AddWorksheet(payregistersummary_dt, "Pay Register Summary");
                                        }


                                    }

                                    using (MemoryStream stream = new MemoryStream())
                                    {
                                        workbook.SaveAs(stream);
                                        var bytes = Convert.ToBase64String(stream.ToArray());
                                        //  FileResponse fileResponse = new FileResponse();
                                        fileResponse.FileName="PayRegister.xlsx";
                                        fileResponse.File=bytes;

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

                                        fileResponse.FileName="PayRegister.xlsx";
                                        fileResponse.File=bytes;
                                        fileResponse=fileResponse;
                                    }
                                }
                            }
                        }
                        else
                        {
                            fileResponse.File="No";
                            fileResponse.FileName="Not Existing";
                        }
                    }
                    else
                    {
                        fileResponse.File="No";
                        fileResponse.FileName="Not Existing";
                    }

                }
                catch (Exception ex)
                {
                    payregister_dt.Columns.Add("Exception", typeof(string));
                    payregister_dt.Rows.Add(string.Format("{0},{1},{2}", ex.Message, ex.StackTrace, ex.InnerException));

                }
            }
            else
            {
                fileResponse.File="No";
                fileResponse.FileName="Not Existing";
            }
                //ListtoDataTableConverter listtoDataTableConverter = new ListtoDataTableConverter();
                //payregister_dt= listtoDataTableConverter.ToDataTable(payreg);
                return fileResponse;
        }
        public FileResponse ExternalPayRegister(int companyCode, int pay_period_Id)
        {
            FileResponse fileResponse = new FileResponse();
            
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_Id);
            string storeProcedure = "sp_PayRegister";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res != null)
            {
               var external_payregister = (DataTable)JsonConvert.DeserializeObject<DataTable>(res)?? new DataTable();
                if (external_payregister.Rows.Count > 0)
                {
                    using var workbook = new XLWorkbook();
                    {
                        var ws = workbook.AddWorksheet(external_payregister, "PayRegister");
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
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
                    fileResponse.FileName = "Pay Register not available";
                    fileResponse.File = "No";
                }
               
            
       
            }
            
            return fileResponse;
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

        public async Task<PayRegisterResponse> PayRegisterUpload(PayRegisterUI payRegisterUI)
        {
            PayRegisterResponse payRegisterUploadResponse = new PayRegisterResponse();

            var parameters = new DynamicParameters();
            parameters.Add("@InputLotNumber", payRegisterUI.LotNumber);
            parameters.Add("@PayPeriod_Id", payRegisterUI.Pay_Period_id);
            parameters.Add("@Company_Id", payRegisterUI.CompanyCode);
            parameters.Add("@Input_type", payRegisterUI.Input_type);
            parameters.Add("@LoginUser", payRegisterUI.LoginUser);
            parameters.Add("@FileName", payRegisterUI.FileName);
            parameters.Add("@FilePath", payRegisterUI.FilePath);

            string storeProcedure = "SP_PayRegister_Upload_Process";
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            if (res!="")
            {
                payRegisterUploadResponse = JsonConvert.DeserializeObject<List<PayRegisterResponse>>(res).FirstOrDefault();

            }
            return payRegisterUploadResponse;
        }

        public FileResponse IncrementReport(int companyCode, int pay_period_Id, int lotNumber, int revised, string processcategory)
        {
            FileResponse fileResponse = new FileResponse();
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyCode);
            parameters.Add("@PayPeriodId", pay_period_Id);
            parameters.Add("@LotNo", lotNumber);
            parameters.Add("@Revised", revised);
            string storeProcedure = "PROC_ExportIncrementDetailsLotwise";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            DataTable payregister_dt = new DataTable();
            if (res != null)
            {
                try
                {
                    payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);
                    if (payregister_dt != null)
                    {
                        if (payregister_dt.Rows.Count > 0)
                        {
                            using var workbook = new XLWorkbook();
                            {
                                var ws = workbook.AddWorksheet(payregister_dt, "Increment");
                                ws.Table(0).ShowAutoFilter = true;
                                ws.Table(0).Theme = XLTableTheme.None;
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    workbook.SaveAs(stream);
                                    var bytes = Convert.ToBase64String(stream.ToArray());

                                    fileResponse.FileName = "Increment.xlsx";
                                    fileResponse.File = bytes;
                                    //fileResponse = fileResponse;
                                }
                            }
                        }
                        else
                        {
                            fileResponse.IncrementFile = "No";
                        }
                    }
                    else
                    {
                        fileResponse.IncrementFile = "No";
                    }

                }
                catch { }
                
            }
            return fileResponse;
        }

        public FileResponse ReconPayRegister(int companyCode, int pay_period_Id, int lotNumber,int revised,string processcategory)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_Id);
            parameters.Add("@Lot_No", lotNumber);
            parameters.Add("@Revised", revised);
            //parameters.Add("@Process_Category", "");


            string storeProcedure = "sp_PayRegister_Recon_lotWise";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res != null)
            {
                try
                {
                    payregister_dt = (DataTable)JsonConvert.DeserializeObject<DataTable>(res);

                    if (payregister_dt.Rows.Count > 0)
                    {
                        DataRow lastRow = payregister_dt.Rows[payregister_dt.Rows.Count - 1];
                        // List<string> RemoveColums = new List<string>();
                        // DataRow dtrow = payregister_dt.NewRow();
                        //foreach (DataColumn column in payregister_dt.Columns)
                        //{
                        //    var value = lastRow[column];

                        //    if (column.DataType.Name=="Double")
                        //    {
                        //        var column_Unique = GetUniqueColumnValues(payregister_dt, column.ColumnName);
                        //        if (column_Unique.Count()>1 || column.ColumnName.ToLower() == ("Service_charge").ToLower())
                        //        {
                        //            var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>(column));
                        //            dtrow[column]=columnsum;
                        //            if (Convert.ToString(columnsum).ToLower()==("0").ToLower())
                        //            {
                        //                RemoveColums.Add(column.ToString());
                        //            }
                        //        }
                        //        else
                        //        {
                        //            dtrow[column]=column_Unique[0];
                        //            if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                        //            {
                        //                RemoveColums.Add(column.ToString());
                        //            }
                        //        }
                        //    }
                        //    else if (column.DataType.Name=="Int64")
                        //    {
                        //        var column_Unique = GetUniqueColumnValuesByInt(payregister_dt, column.ColumnName);
                        //        if (column_Unique.Count()>1)
                        //        {
                        //            var columnsum = payregister_dt.AsEnumerable().Sum(row => row.Field<Int64?>(column));
                        //            dtrow[column]=columnsum;
                        //            if (Convert.ToString(columnsum).ToLower()==("0").ToLower())
                        //            {
                        //                RemoveColums.Add(column.ToString());
                        //            }

                        //        }
                        //        else
                        //        {
                        //            dtrow[column]=column_Unique[0];
                        //            if (Convert.ToString(column_Unique[0]).ToLower()==("0").ToLower())
                        //            {
                        //                RemoveColums.Add(column.ToString());
                        //            }
                        //        }

                        //    }
                        //    else
                        //    {
                        //        dtrow[column]="";
                        //    }
                        //}
                        //var ctc = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("TOTAL COST TO COMPANY"));
                        //var service = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("Service_charge"));
                        //payregister_dt.Rows.Add(dtrow);
                        //// Process the value as needed
                        //foreach (var item in RemoveColums)
                        //{
                        //    payregister_dt.Columns.Remove(item);
                        //}

                        //var comayName = CompanyNameByCode(companyCode);
                        //var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                        using var workbook = new XLWorkbook();
                        {
                            var ws = workbook.AddWorksheet(payregister_dt, "ReConPayRegister");
                            ws.Table(0).ShowAutoFilter = true;
                            ws.Table(0).Theme = XLTableTheme.None;
                            using (MemoryStream stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var bytes = Convert.ToBase64String(stream.ToArray());

                                fileResponse.FileName = "ReConPayRegister.xlsx";
                                fileResponse.File = bytes;
                                fileResponse = fileResponse;
                            }
                            // ws.SheetView.FreezeRows(2);
                            //ws.SheetView.FreezeColumns(2);
                            //  wb.Worksheets.Add(dataTable);
                            //if (payregister_dt.Columns.Count > 1)
                            //{

                            //    ws.Row(1).InsertRowsAbove(1);
                            //    ws.Range("A1:B1").Merge();
                            //    ws.Cell(1, 1).Value = comapny.Client_Name;
                            //    ws.Cell(1, 1).Style.Font.Bold=true;
                            //    var lastrow = ws.LastRowUsed().RowNumber();

                            //    if (ctc!=null && service!=null)
                            //    {
                            //        var Total = ctc+service;
                            //        var toal_GST = Total*(18.0/100.0);
                            //        ws.Cell(lastrow, 1).Value = "Grand Total";

                            //        ws.Cell(lastrow + 3, 4).Value = comapny.Client_Name;



                            //        var clinet_cell = ws.Cell(lastrow + 3, 4);
                            //        clinet_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        clinet_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        clinet_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        clinet_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;



                            //        ws.Cell(lastrow + 3, 5).Value = ctc;
                            //        var ctc_cell = ws.Cell(lastrow + 3, 5);
                            //        ctc_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        ctc_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        ctc_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        ctc_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                            //        ws.Cell(lastrow + 4, 4).Value = "Service Charge:";
                            //        var Service_cell = ws.Cell(lastrow + 4, 4);
                            //        Service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        Service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        Service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        Service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                            //        ws.Cell(lastrow + 4, 5).Value = service;
                            //        var service_cell = ws.Cell(lastrow + 4, 5);
                            //        service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                            //        ws.Cell(lastrow + 5, 4).Value = "";
                            //        var empty_cell = ws.Cell(lastrow + 5, 4);
                            //        empty_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        empty_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        empty_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        empty_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                            //        ws.Cell(lastrow + 5, 5).Value = Total;
                            //        var Total_cell = ws.Cell(lastrow + 5, 5);
                            //        Total_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        Total_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        Total_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        Total_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                            //        ws.Cell(lastrow + 6, 4).Value = "Total";
                            //        var Total1_cell = ws.Cell(lastrow + 6, 4);
                            //        Total1_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        Total1_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        Total1_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        Total1_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                            //        ws.Cell(lastrow + 6, 5).Value = toal_GST;
                            //        var toal_GST_cell = ws.Cell(lastrow + 6, 5);
                            //        toal_GST_cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            //        toal_GST_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            //        toal_GST_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            //        toal_GST_cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                            //    }

                            //    using (MemoryStream stream = new MemoryStream())
                            //    {
                            //        workbook.SaveAs(stream);
                            //        var bytes = Convert.ToBase64String(stream.ToArray());
                            //        //  FileResponse fileResponse = new FileResponse();
                            //        fileResponse.FileName="PayRegister.xlsx";
                            //        fileResponse.File=bytes;

                            //    }

                            //}
                            //else
                            //{

                            //}
                        }
                    }
                    else
                    {
                        fileResponse.File = "No";
                        fileResponse.FileName = "This Lot there no Recon Register";
                    }
                }
                catch (Exception ex)
                {
                    fileResponse.File = "No";
                    fileResponse.FileName = ex.Message;
                    //payregister_dt.Columns.Add("Exception", typeof(string));
                    //payregister_dt.Rows.Add(string.Format("{0},{1},{2}", ex.Message, ex.StackTrace, ex.InnerException));
                    //using var workbook = new XLWorkbook();
                    //{
                    //    var ws = workbook.AddWorksheet(payregister_dt, "ReConPayRegister");
                    //    using (MemoryStream stream = new MemoryStream())
                    //    {
                    //        workbook.SaveAs(stream);
                    //        var bytes = Convert.ToBase64String(stream.ToArray());

                    //        fileResponse.FileName = "ReConPayRegister.xlsx";
                    //        fileResponse.File = bytes;
                    //        fileResponse = fileResponse;
                    //    }
                    //    //ws.Table(0).ShowAutoFilter = false;
                    //}

                }
            }
            else {
                fileResponse.File = "No";
                fileResponse.FileName = "This Lot there no Recon Register";
            }
                return fileResponse;
        }

    }
}
