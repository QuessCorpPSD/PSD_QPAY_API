using Azure;
using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
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

        public PayRegisterRepository(DbRepository dbRepository)
        {
            this._dbRepository=dbRepository;
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

        public FileResponse GetOtherIncomePayRegister(int companyCode, int pay_period_Id, int lotNumber)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_ID", companyCode);
            parameters.Add("@Pay_Frequency_Detail_Id", pay_period_Id);
            parameters.Add("@PO_NUMBER", "");
            parameters.Add("@INPUTNUMBER", 0);
            string storeProcedure = "sp_OtherIncome_Report_PONUMBER_ExportToExcel";
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
                            var ctc = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("CTC"));
                            var service = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("SERCG"));
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

        public FileResponse GetQCOtherIncomePayRegister(int companyCode, int pay_period_Id, int lotNumber, string pay_period)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_ID", companyCode);
            parameters.Add("@Pay_Frequency_Detail_Id", pay_period_Id);
            parameters.Add("@PO_NUMBER", "");
            parameters.Add("@INPUTNUMBER", 0);
            string storeProcedure = "sp_OtherIncome_Report_PONUMBER_ExportToExcel";
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
                            ws.Row(1).InsertRowsAbove(3);
                            ws.SheetView.FreezeRows(4);
                            ws.SheetView.FreezeColumns(6);
                            var ctc = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("CTC"));
                            var service = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("SERCG"));
                            var comayName = CompanyNameByCode(companyCode);
                            var comapny = JsonConvert.DeserializeObject<List<ClientModel>>(comayName).FirstOrDefault();
                            //ws.Style.Border.TopBorder= XLBorderStyleValues.Thick;
                            //ws.Style.Border.LeftBorder= XLBorderStyleValues.Thick;
                            //ws.Style.Border.RightBorder= XLBorderStyleValues.Thick;
                            //ws.Style.Border.BottomBorder= XLBorderStyleValues.Thick;
                            //ws.Style.Border.RightBorderColor = XLColor.Black;
                            //ws.Style.Border.LeftBorderColor = XLColor.Black;
                            //ws.Style.Border.TopBorderColor = XLColor.Black;
                            //ws.Style.Border.BottomBorderColor = XLColor.Black;
                            //ws.Range("A1:B1").Merge();
                            //ws.Range("A1:C1").Merge();
                            ws.Range("A1:Z1").Merge();
                            ws.Range("A2:Z2").Merge();
                            ws.Range("A3:Z3").Merge();
                            

                            ws.Cell(1, 1).Value = comapny.Client_Name;
                           // ws.Cell(1, 1).Style.Font.Bold=true;
                          //  ws.Cell(1, 1).Style.Font.Underline=XLFontUnderlineValues.Single;
                            ws.Cell(2, 1).Value ="SLAIT FOR THE MONTH OF " +pay_period;
                            ws.Cell(2, 1).Style.Font.Bold=true;
                            ws.Cell(2, 1).Style.Font.Underline=XLFontUnderlineValues.Single;
                            var headerRange = ws.Range("A1:D1");
                            headerRange.Style.Font.Bold = true;


                            //headerRange.Style.Font.Underline=XLFontUnderlineValues.Single;

                            headerRange.Style.Border.TopBorder= XLBorderStyleValues.None;
                            headerRange.Style.Border.LeftBorder= XLBorderStyleValues.None;
                            headerRange.Style.Border.RightBorder= XLBorderStyleValues.None;
                            headerRange.Style.Border.BottomBorder= XLBorderStyleValues.None;
                            //  ws.FirstRowUsed();
                            var lastrow = ws.LastRowUsed().RowNumber();
                            ws.Style.Font.Bold = true;
                            //ws.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                            //ws.Style.Border.OutsideBorderColor = XLColor.Black;
                           
                            if (ctc!=null && service!=null)
                            {
                                var Total = ctc+service;
                                var toal_GST = Total*(18.0/100.0);
                                

                                ws.Cell(lastrow + 3, 4).Value = comapny.Client_Name;



                                var clinet_cell = ws.Cell(lastrow + 3, 4);
                                //clinet_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //clinet_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //clinet_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //clinet_cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                //clinet_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //clinet_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //clinet_cell.Style.Border.LeftBorderColor = XLColor.Black;
                                //clinet_cell.Style.Border.RightBorderColor = XLColor.Black;

                                clinet_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                clinet_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 3, 5).Value = ctc;
                                var ctc_cell = ws.Cell(lastrow + 3, 5);

                                //ctc_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //ctc_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //ctc_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //ctc_cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                //ctc_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //ctc_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //ctc_cell.Style.Border.LeftBorderColor = XLColor.Black;
                                //ctc_cell.Style.Border.RightBorderColor = XLColor.Black;

                                ctc_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                ctc_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 4, 4).Value = "Service Charge:";
                                var Service_cell = ws.Cell(lastrow + 4, 4);

                                //Service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //Service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //Service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //Service_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                //Service_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //Service_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //Service_cell.Style.Border.LeftBorderColor = XLColor.Black;

                                Service_cell.Style.Border.OutsideBorderColor = XLColor.Black;
                                Service_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 4, 5).Value = service;
                                var service_cell = ws.Cell(lastrow + 4, 5);

                                //service_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //service_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //service_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //service_cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                //service_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //service_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //service_cell.Style.Border.LeftBorderColor = XLColor.Black;
                                //service_cell.Style.Border.RightBorderColor = XLColor.Black;

                                service_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                service_cell.Style.Border.OutsideBorderColor = XLColor.Black;


                                ws.Cell(lastrow + 5, 4).Value = "Sub Total";
                                var empty_cell = ws.Cell(lastrow + 5, 4);

                                //empty_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //empty_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //empty_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //empty_cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                //empty_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //empty_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //empty_cell.Style.Border.LeftBorderColor = XLColor.Black;
                                //empty_cell.Style.Border.RightBorderColor = XLColor.Black;

                                empty_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                empty_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 5, 5).Value = Total;
                                var Total_cell = ws.Cell(lastrow + 5, 5);
                                //Total_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //Total_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //Total_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //Total_cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                //Total_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //Total_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //Total_cell.Style.Border.LeftBorderColor = XLColor.Black;
                                //Total_cell.Style.Border.RightBorderColor = XLColor.Black;

                                Total_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                Total_cell.Style.Border.OutsideBorderColor = XLColor.Black;


                                ws.Cell(lastrow + 6, 4).Value = "GST";
                                var Total1_cell = ws.Cell(lastrow + 6, 4);
                                //Total1_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //Total1_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //Total1_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //Total1_cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                //Total1_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //Total1_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //Total1_cell.Style.Border.LeftBorderColor = XLColor.Black;
                                //Total1_cell.Style.Border.RightBorderColor = XLColor.Black;

                                Total1_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                Total1_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 6, 5).Value = toal_GST;
                                var toal_GST_cell = ws.Cell(lastrow + 6, 5);
                                //toal_GST_cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                                //toal_GST_cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                                //toal_GST_cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                                //toal_GST_cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                                //toal_GST_cell.Style.Border.TopBorderColor = XLColor.Black;
                                //toal_GST_cell.Style.Border.BottomBorderColor = XLColor.Black;
                                //toal_GST_cell.Style.Border.LeftBorderColor = XLColor.Black;
                                //toal_GST_cell.Style.Border.RightBorderColor = XLColor.Black;

                                toal_GST_cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                toal_GST_cell.Style.Border.OutsideBorderColor = XLColor.Black;

                                ws.Cell(lastrow + 7, 4).Value = "Total";
                                ws.Cell(lastrow + 7, 5).Value = Total+toal_GST;

                                ws.Cell(lastrow + 7, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                ws.Cell(lastrow + 7, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                            }
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
        public FileResponse PayRegisterDownload(int companyCode, int pay_period_Id, int lotNumber,string pay_period)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_Id);
            parameters.Add("@Lot_No", lotNumber);
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
                                    dtrow[column]="";
                                }
                            }
                            var ctc = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("TOTAL COST TO COMPANY"));
                            var service = payregister_dt.AsEnumerable().Sum(row => row.Field<double?>("Service_charge"));
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
                                            ws.SheetView.FreezeRows(4);
                                            ws.SheetView.FreezeColumns(2);

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

                                            if (ctc!=null && service!=null)
                                            {
                                                var Total = ctc+service;
                                                var toal_GST = Total*(18.0/100.0);
                                                ws.Cell(lastrow, 1).Value = "Grand Total";

                                                ws.Cell(lastrow + 3, 4).Value = string.Format("SALARY FOR THE MONTH OF {0}", pay_period); ;



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

                                                ws.Cell(lastrow + 5, 4).Value = "Sub Total";
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

                                                ws.Cell(lastrow + 6, 4).Value = "GST";
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

                                                ws.Cell(lastrow + 7, 4).Value = "Total";
                                                ws.Cell(lastrow + 7, 5).Value = Total+toal_GST;

                                                ws.Cell(lastrow + 7, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                                ws.Cell(lastrow + 7, 4).Style.Border.OutsideBorderColor = XLColor.Black;
                                                

                                                ws.Cell(lastrow + 7, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                                ws.Cell(lastrow + 7, 5).Style.Border.OutsideBorderColor = XLColor.Black;

                                            }

                                           


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

        public PayRegisterResponse PayRegisterUpload(PayRegisterUI payRegisterUI)
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
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!="")
            {
                payRegisterUploadResponse = JsonConvert.DeserializeObject<List<PayRegisterResponse>>(res).FirstOrDefault();

            }
            return payRegisterUploadResponse;
        }

        public FileResponse ReconPayRegister(int companyCode, int pay_period_Id, int lotNumber)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyCode);
            parameters.Add("@Pay_Period_Id", pay_period_Id);
            parameters.Add("@Lot_No", lotNumber);            

            string storeProcedure = "sp_PayRegister_Recon_lotWise";
            var res = this._dbRepository.GetItemsAsync(storeProcedure, parameters).Result;
            if (res!=null)
            {
                try
                {
                    payregister_dt =(DataTable)JsonConvert.DeserializeObject<DataTable>(res);

                    if (payregister_dt.Rows.Count>0)
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

                                fileResponse.FileName="ReConPayRegister.xlsx";
                                fileResponse.File=bytes;
                                fileResponse=fileResponse;
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
                }
                catch (Exception ex)
                {
                    payregister_dt.Columns.Add("Exception", typeof(string));
                    payregister_dt.Rows.Add(string.Format("{0},{1},{2}", ex.Message, ex.StackTrace, ex.InnerException));
                    using var workbook = new XLWorkbook();
                    {
                        var ws = workbook.AddWorksheet(payregister_dt, "ReConPayRegister");
                        using (MemoryStream stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var bytes = Convert.ToBase64String(stream.ToArray());

                            fileResponse.FileName="ReConPayRegister.xlsx";
                            fileResponse.File=bytes;
                            fileResponse=fileResponse;
                        }
                        //ws.Table(0).ShowAutoFilter = false;
                    }

                    }
            }

            return fileResponse;
        }

    }
}
