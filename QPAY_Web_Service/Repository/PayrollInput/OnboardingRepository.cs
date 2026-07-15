using ClosedXML.Excel;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QPay.API.Models;
using QPay.DAL.Repository;
using QPay.DTo.Models.PayrollInput;
using QPay.IRepository.iRepository.PayrollInput;
using QPay.UI.Common;
using System.Data;

namespace QPay.IRepository.Repository.PayrollInput
{
    public class OnboardingRepository : IOnboardingRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly ILogger<OnboardingRepository> _logger;
        private readonly string[] _companyCode;
        private readonly IConfiguration _configuration;


        public OnboardingRepository(DbRepository dbRepository, ILogger<OnboardingRepository> logger, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._logger = logger;
            this._configuration = configuration;
            this._companyCode = _configuration.GetSection("OtherIncome:companyCode").Get<string[]>() ?? Array.Empty<string>();
        }
        public async Task<List<Onboarding>> GetAllOnboardingDetails(string companyId, string? payPeriod)
        {
            this._logger.LogInformation("Requesting Gell All Onboarding " + companyId, payPeriod);
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyCode", companyId);
            parameters.Add("@PayPeriod", payPeriod);
          
            try
            {
                this._logger.LogInformation("Starting the store procedure execution " + companyId, payPeriod);
                var res = await this._dbRepository.GetItemsAsync("Proc_Onboarding_data", parameters);
                this._logger.LogInformation("completed the store procedure execution " + companyId, payPeriod);
                if (!string.IsNullOrEmpty(res))
                {
                    return JsonConvert.DeserializeObject<List<Onboarding>>(res) ?? new List<Onboarding>();
                }
            }
            catch(Exception ex)
            {
                this._logger.LogInformation("GetAllOnboardingDetails exception : " + string.Format("StackTrace : {0} , Message : {1} , InnerException : {2}",ex.StackTrace,ex.Message,ex.InnerException));
            }
           

            return new List<Onboarding>();
        }

        public DataSet GetNewJoineeTemplate(int companyId, int payPeriodId, int flag, int mapNameId)
        {
            DataSet ds = this._dbRepository.GetNewJoineeDataSet(companyId, payPeriodId, flag, mapNameId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }

        }

        public async Task<string> MoveToQpay(string xmlString, int companyId, string payPeriod, int payPeriodId, string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@xml", xmlString);
            parameters.Add("@User", userId);
            parameters.Add("@PayPeriod", payPeriod);
            parameters.Add("@Payperiod_Id", payPeriodId);
            parameters.Add("@Company_Id", companyId);


            var res = await this._dbRepository.GetItemsAsync("Proc_Transfer_Employee", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<string> PostValidateOfferId(string xmlString)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OFFERXML", xmlString);

            var res = await this._dbRepository.GetItemsAsync("USP_OFFERID_CheckPoints", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<string> PostRollbackOfferId(string xmlString, string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@XMLDATA", xmlString);
            parameters.Add("@CREATEDBY", userId);

            var res = await this._dbRepository.GetItemsAsync("USP_UPLOAD_OFFERID_ROLLBACK", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
        public async Task<string> PostNewJoineeData(string xmlString, string companyCode, int companyId, int payPeriodId, string filePath, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@XML", xmlString);
            parameters.Add("@user", userId);
            parameters.Add("@COMPANY_CODE", companyCode);
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Payperiod_Id", payPeriodId);
            parameters.Add("@InputType", "1");
            parameters.Add("@LotNumber", "0");
            parameters.Add("@Filepath", filePath);


            var res = await this._dbRepository.GetItemsAsync("PROC_BULK_UPLOAD_HARBOUR_EMPLOYEE", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public List<PayperiodDD> GetCurrentPayperiod(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            var res = this._dbRepository.GetItemsAsync("Proc_GetCurrentPayperiod", parameters).Result;
            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<PayperiodDD>>(res) ?? new List<PayperiodDD>();
            }

            return new List<PayperiodDD>();
        }


        public async Task<string> PostOneTimeInputData(string xmlString, int companyId, int payPeriodId, string filePath, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@payperiod_Id", payPeriodId);
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@User", userId);
            parameters.Add("@xml", xmlString);
            parameters.Add("@Flag", '1');
            parameters.Add("@InputType", "1");
            parameters.Add("@LotNumber", "0");
            parameters.Add("@FilePath", filePath);



            var res = await this._dbRepository.GetItemsAsync("PROC_BULK_UPLOAD_ONE_TIME_INPUT_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public async Task<List<FinalSubmission>> GetAllFinalSubmitDetails(int companyId, int payPeriodId, string Action, string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@InputLotNumber", '0');
            parameters.Add("@Action", Action);
            parameters.Add("@userid", userId);


            var res = await this._dbRepository.GetItemsAsync("Proc_Manage_FinalSubmit_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<FinalSubmission>>(res) ?? new List<FinalSubmission>();
            }

            return new List<FinalSubmission>();
        }
        public async Task<FileResponse> AttributeTemplate(int flagId, int companyId, int payperiodId, int lotno, string createdBy,string XML)
        {
            FileResponse fileResponse = new FileResponse();
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@FLAG", flagId);
                parameter.Add("@COMPANY_ID", companyId);
                parameter.Add("@PAYPERIOD_ID", payperiodId);
                parameter.Add("@LOTNUMBER", lotno);
                parameter.Add("@CreadtedBy", createdBy);
                parameter.Add("@XML", XML);
                var res = await this._dbRepository.GetItemsAsync("USP_PAYROLL_ATTRIBUTE_CHANGE", parameter);

                if (!string.IsNullOrEmpty(res))
                {
                    var response = JsonConvert.DeserializeObject<DataTable>(res) ?? new DataTable();
                    using var workbook = new XLWorkbook();
                    {
                        var ws = workbook.AddWorksheet(response, "Attribute");
                        ws.Table(0).ShowAutoFilter = false;
                        ws.Table(0).Theme = XLTableTheme.None;
                        using (MemoryStream stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            var bytes = Convert.ToBase64String(stream.ToArray());                            
                            fileResponse.FileName = string.Format("Attribute_{0}.xlsx",System.DateTime.Now.ToString("ddMMyyyyhhmmss")); 
                            fileResponse.File = bytes;
                        }
                    }
                }
                else
                {
                    fileResponse.File = "N";
                }
            }
            catch(Exception ex)
            {
                fileResponse.FileName = ex.Message;
                fileResponse.File = "N";
            }

            return fileResponse;

        }
        public async Task<string> PostFinalSubmission(int companyId, int payPeriodId, string LotNos, string userId, string remarks)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@InputLotNumber", LotNos);
            parameters.Add("@Action", "Edit");
            parameters.Add("@userid", userId);
            parameters.Add("@Remarks", remarks);

            var res = await this._dbRepository.GetItemsAsync("Proc_Manage_FinalSubmit_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public string GetRegisterFilename(int companyId, int payPeriodId, int lotNumber, string inputType, int flag)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@InputLotNumber", lotNumber);
            parameters.Add("@Input_type", inputType);
            parameters.Add("@LoginUser", "1");
            parameters.Add("@FileName", "");
            parameters.Add("@Flag", flag);

            var res = this._dbRepository.GetItemsAsync("Proc_Manage_Attached_Salary_Register", parameters).Result;

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public DataSet GetNewJoineeEmployeeId(int companyId, string payPeriod, int lotNumber)
        {
            DataSet ds = this._dbRepository.GetEmployeeIDDataSet(companyId, payPeriod, lotNumber);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given company and pay period.");
            }

        }
        //public DataSet GetConsolidatePayRegister(int companyId, int payPeriodId, string lotNumber)
        //{
        //    DataSet ds = this._dbRepository.GetConsolidatePayRegisterDataSet(companyId, payPeriodId, lotNumber);
        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        return ds;
        //    }
        //    else
        //    {
        //        throw new Exception("No data found for the given company and pay period.");
        //    }

        //}

        public FileResponse GetConsolidatePayRegister(int companyId, string companyCode, string payPeriod, int payPeriodId, string lotNumber)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();
            //string storeProcedure = string.Empty;
            //var parameters = new Dictionary<string, object?>
            //{
            //    ["@Company_Id"] = companyId,
            //    ["@Pay_Period_Id"] = payPeriodId,
            //    ["@Lot_No"] = lotNumber,
            //};
            //return _dbRepository.ExecuteStoredProcedureToDataSetAsyncSecondary("sp_PayRegister_MultipleLot", parameters, 1500);
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Pay_Period_Id", payPeriodId);
            parameters.Add("@Lot_No", lotNumber);


            string storeProcedure = "";
            storeProcedure = "sp_PayRegister_MultipleLot";

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



                            var comayName = CompanyNameByCode(companyId);
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
                                            ws.Cell(2, 1).Value = string.Format("SALARY FOR THE MONTH OF {0}", payPeriod);
                                            ws.Cell(2, 1).Style.Font.Bold = true;
                                            var lastrow = ws.LastRowUsed().RowNumber();

                                            ws.Cell(lastrow, 1).Value = "Grand Total";
                                        }

                                    }

                                    using (MemoryStream stream = new MemoryStream())
                                    {
                                        workbook.SaveAs(stream);
                                        var bytes = Convert.ToBase64String(stream.ToArray());
                                        //  FileResponse fileResponse = new FileResponse();
                                        fileResponse.FileName = "Consolidated_PayRegister.xlsx";
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
            else
            {
                fileResponse.File = "No";
                fileResponse.FileName = "Not Existing";
            }

                return fileResponse;
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


        //public async Task<DataSet> GetConsolidatePayRegisterOT(int companyId, int payPeriodId, string lotNumber)
        //{
        //    string storeProcedure = string.Empty;
        //        var parameters = new Dictionary<string, object?>
        //        {
        //            ["@Company_ID"] = companyId,
        //            ["@Pay_Frequency_Detail_Id"] = payPeriodId,
        //            ["@PO_NUMBER"] = "",
        //            ["@INPUTNUMBER"] = lotNumber,
        //        };
        //        return _dbRepository.ExecuteStoredProcedureToDataSetAsyncSecondary("sp_OtherIncome_Report_PONUMBER_ExportToExcel", parameters, 1500);
        //}

        public FileResponse GetConsolidatePayRegisterOT(int companyId, string companyCode, string payPeriod, int payPeriodId, string lotNumber)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable payregister_dt = new DataTable();

            var parameters = new DynamicParameters();
            parameters.Add("@Company_ID", companyId);
            parameters.Add("@Pay_Frequency_Detail_Id", payPeriodId);
            parameters.Add("@PO_NUMBER", "");
            parameters.Add("@INPUTNUMBER", lotNumber);


            string storeProcedure = "";
            storeProcedure = "sp_OtherIncome_Report_PONUMBER_ExportToExcel";

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
                                    //dtrow[column] = "";
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



                            var comayName = CompanyNameByCode(companyId);
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
                                            ws.Cell(2, 1).Value = string.Format("SALARY FOR THE MONTH OF {0}", payPeriod);
                                            ws.Cell(2, 1).Style.Font.Bold = true;
                                            var lastrow = ws.LastRowUsed().RowNumber();

                                            ws.Cell(lastrow, 1).Value = "Grand Total";
                                        }

                                    }

                                    using (MemoryStream stream = new MemoryStream())
                                    {
                                        workbook.SaveAs(stream);
                                        var bytes = Convert.ToBase64String(stream.ToArray());
                                        //  FileResponse fileResponse = new FileResponse();
                                        fileResponse.FileName = "Consolidated_PayRegister_OtherIncome.xlsx";
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

                                        fileResponse.FileName = "Consolidated_PayRegister_OtherIncome.xlsx";
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
            else
            {
                fileResponse.File = "No";
                fileResponse.FileName = "Not Existing";
            }

            return fileResponse;
        }

        public async Task<DataSet> EmployeeTemplateImport(string xmlInput, string userId, int companyId, int payPeriodId, int inputId, int lotNo)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@XML"] = xmlInput,
                ["@user"] = userId,
                ["@Company_Id"] = companyId,
                ["@Payperiod_Id"] = payPeriodId,
                ["@InputType"] = inputId,
                ["@LotNumber"] = lotNo
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_Uploade_EmployeeCode_Revised_NewUI", parameters, 1500);
        }

        public async Task<DataSet> GetRevisedTemplate(int companyId, int payPeriodId, int mapNameId, int inputId, int lotNo)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Payperiod_Id"] = payPeriodId,
                ["@MapNameId"] = mapNameId,
                ["@InputType"] = inputId,
                ["@LotNumber"] = lotNo
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_view_Excel_Format_Bulk_Revised", parameters, 1500);
        }

        public async Task<string> PostRevisedInput(string xmlString, string userId, string companyCode, int companyId, int payPeriodId, int inputType, int lotNo, string filePath)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@XML", xmlString);
            parameters.Add("@user", userId);
            parameters.Add("@COMPANY_CODE", companyCode);
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Payperiod_Id", payPeriodId);
            parameters.Add("@InputType", inputType);
            parameters.Add("@LotNumber", lotNo);
            parameters.Add("@FilePath", filePath);

            var res = "No data found";
            if (inputType == 1)
            {
                res = await this._dbRepository.GetItemsAsync("PROC_BULK_UPLOAD_REVISEDINPUT_QzoneNewUI", parameters);
            }
            else
            {
                res = await this._dbRepository.GetItemsAsync("PROC_BULK_UPLOAD_ONE_TIME_INPUT_OTHER_REVISED", parameters);
            }

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }


        public async Task<DataSet> GetInputautomationReport(int companyId, int payPeriodId, int inputId, int lotNo)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_Id"] = companyId,
                ["@Pay_Period_Id"] = payPeriodId,
                ["@InputLotNumber"] = inputId,
                ["@InputType"] = lotNo
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("InputAutomation_Custom_Report", parameters, 1500);
        }

        public async Task<string> PostCustomerConfirmation(int companyId, int payPeriodId, string LotNos, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@PayPeriod_Id", payPeriodId);
            parameters.Add("@InputLotNumber", LotNos);
            parameters.Add("@Action", "CUSTOMERCONFIRM");
            parameters.Add("@userid", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_Manage_FinalSubmit_NEWUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public async Task<string> PostFinalSubmissionLotMerge(FinalSubmitMerge request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@COMPANY_ID", request.CompanyId);
            parameters.Add("@PAYPERIOD_ID", request.PayPeriodId);
            parameters.Add("@MERGIED_LOT", request.MergedLots);
            parameters.Add("@USER_ID", request.CreatedBy);
            parameters.Add("@Remarks", request.Remarks);
            parameters.Add("@Data_From", request.InputType);

            var res = await this._dbRepository.GetItemsAsync("USP_Performa_Invoice_MergeLOT", parameters);
            return res;

        }
    }
}
