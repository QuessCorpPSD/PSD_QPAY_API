using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.UI.Common;
using QPay.UI.Customer;
using QPay.UI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Customer.Company;
namespace QPay.DAL.Repository
{
    public class DbRepository
    {
        //DbRepository<T> 
        private readonly string _connectionString;
        private readonly string _connectionReconString;
        private readonly string _secondaryString;

        public DbRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            _connectionReconString = configuration.GetConnectionString("ReConDBConnection")??"";
            _secondaryString = configuration.GetConnectionString("SecondaryConnection") ?? "";

        }
        private IDbConnection Connection => new SqlConnection(_connectionString);
        private IDbConnection ConnectionRecon => new SqlConnection(_connectionReconString);
        private IDbConnection ConnectionSecondary => new SqlConnection(_secondaryString);

        public async Task<object> QueryAsync(string query)
        {
            using (var dbConnection = Connection)
            {
                string sql = query;
                dbConnection.Open();
                var TEST = await dbConnection.QueryFirstOrDefaultAsync<string>(sql);

                return TEST??"";
            }
        }

        public async Task<string> QueryMultiAsync(string query)
        {
            using (var dbConnection = Connection)
            {
                string sql = query;
                dbConnection.Open();
                var result = await dbConnection.QueryAsync(sql, null, null, 100, CommandType.Text).ConfigureAwait(false);
                var TEST = JsonConvert.SerializeObject(result);
                return TEST??"";
            }
        }
        public DataSet GetDataSetAsync(int companyCode, int pay_period_id, int lot, int inputType)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                connection.Open();
                using var command = new SqlCommand("InputAutomation_Custom_Report", connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Company_Id", companyCode);
                    command.Parameters.AddWithValue("@Pay_Period_Id", pay_period_id);
                    command.Parameters.AddWithValue("@InputLotNumber", lot);
                    command.Parameters.AddWithValue("@InputType", inputType);
                    command.CommandTimeout=1500;
                    //if (param != null)
                    //{
                    //    foreach (var prop in param.GetType().GetProperties())
                    //    {
                    //        command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(param) ?? DBNull.Value);
                    //    }
                    //}

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                    connection.Close();
                }
            }
            return ds;
        }
        public DataSet GetNewJoineeDataSet(int companyId, int payPeriodId, int flag, int mapNameId)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("Proc_view_Excel_Format", connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Flag", flag);
                    command.Parameters.AddWithValue("@Employee_Id", 0);
                    command.Parameters.AddWithValue("@Company_Id", companyId);
                    command.Parameters.AddWithValue("@PayPeriod_Id", payPeriodId);
                    command.Parameters.AddWithValue("@MapNameId", mapNameId);
                    command.Parameters.AddWithValue("@ISFANDF", 0);
                    command.Parameters.AddWithValue("@InputType", 1);
                    command.Parameters.AddWithValue("@LotNumber", 0);
                    //command.CommandTimeout = 1500;

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
        public DataSet GetEmployeeIDDataSet(int companyId, string payPeriod, int lotNumber)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("sp_GetAllEmployeeDetails_QZone", connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Company_Id", companyId);
                    command.Parameters.AddWithValue("@PayPeriod", payPeriod);
                    command.Parameters.AddWithValue("@InputlotNo", lotNumber);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
        public DataSet GetConsolidatePayRegisterDataSet(int companyId, int payPeriodId, string lotNumber)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("sp_PayRegister_MultipleLot", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Company_Id", companyId);
                    command.Parameters.AddWithValue("@Pay_Period_Id", payPeriodId);
                    command.Parameters.AddWithValue("@Lot_No", lotNumber);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
        public DataSet GetEmployeeIncrementDataSet(int companyId, int payPeriodId, int InputType, int MapNameId)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("PROC_QZONE_EXCEL_VIEW_INCREMENT", connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Company_Id", companyId);
                    command.Parameters.AddWithValue("@PayPeriod_Id", payPeriodId);
                    command.Parameters.AddWithValue("@MapNameId", MapNameId);
                    command.Parameters.AddWithValue("@InputType", InputType);
                    command.CommandTimeout = 1500;

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
        public async Task<string> GetItemsReconAsync(string storeProcedureName, object param)
        {
            try
            {
                using (var dbConnection = ConnectionRecon)
                {

                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync(storeProcedureName, param, null, commandTimeout: 1000, CommandType.StoredProcedure);
                    var obj = JsonConvert.SerializeObject(result);
                    return obj;
                }
            }
            catch (SqlException ex)
            {
                // Log exception details (You can use a logging library like Serilog or NLog)
                //Console.WriteLine($"SQL Exception: {ex.Message}");
                //throw ex; // Rethrow the exception or return a custom error

                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
                // Handle other exceptions
                //Console.WriteLine($"Exception: {ex.Message}");
                //throw;
            }
        }

        //public async Task<IEnumerable<T>> GetItemsAsync<T>(string storedProcedureName, object param)
        //{
        //    using var dbConnection = Connection;
        //    dbConnection.Open();
        //    var result = await dbConnection.QueryAsync<T>(storedProcedureName, param, commandTimeout: 1000, commandType: CommandType.StoredProcedure);
        //    return result;
        //}

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string storeProcedureName, object param)
        {
            try
            {
                using (var dbConnection = Connection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync<T>(storeProcedureName, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                throw new Exception("Database operation failed: " + ex.Message);
            }
        }

        public async Task<string> GetItemsAsync(string storeProcedureName, object param)
        {
            try
            {
                using (var dbConnection = Connection)
                {

                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync(storeProcedureName, param, null, commandTimeout: 1000, CommandType.StoredProcedure);
                    var obj = JsonConvert.SerializeObject(result);
                    return obj;
                }
            }
            catch (SqlException ex)
            {
                // Log exception details (You can use a logging library like Serilog or NLog)
                //Console.WriteLine($"SQL Exception: {ex.Message}");
                //throw ex; // Rethrow the exception or return a custom error

                       return ex.Message;
                //    }
                //    catch (Exception ex)
                //    {
                //        return ex.Message;
                //        // Handle other exceptions
                //        //Console.WriteLine($"Exception: {ex.Message}");
                //        //throw;
                  }
                }

                // Example method to insert a new record
        public async Task<DbOperationResult> InsertItemAsync<T>(T model, string procedureName)
        {
           
            try
            {
                using var dbConnection = Connection;
                dbConnection.Open();

                var result = await dbConnection.ExecuteAsync(
                    procedureName,
                    model,
                    commandTimeout: 1000,
                    commandType: CommandType.StoredProcedure
                );
                return new DbOperationResult
                {
                    IsSuccess = result == 1,
                    Message = result == 1 ? "Inserted successfully" : "Insert failed"
                };
            }
            catch (SqlException ex)
            {
                // Ideally use ILogger, not Console
                return new DbOperationResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
                //throw;
            }
            catch (Exception ex)
            {
                return new DbOperationResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<int> BulkInsertItemAsync(List<T> model, string procedureName)
        {
            try
            {
                using (var dbConnection = Connection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.ExecuteAsync(procedureName, model, null, null, CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (SqlException ex)
            {
                // Log exception details
                Console.WriteLine($"SQL Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateItemAsync(T model, string procedureName)
        {
            try
            {
                using (var dbConnection = Connection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.ExecuteAsync(procedureName, model, null, null, CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (SqlException ex)
            {
                // Log exception details
                Console.WriteLine($"SQL Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteItemAsync(T model, string procedureName)
        {
            try
            {
                using (var dbConnection = Connection)
                {
                    dbConnection.Open();
                    var result = await dbConnection.ExecuteAsync(procedureName, model, null, null, CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (SqlException ex)
            {
                // Log exception details
                Console.WriteLine($"SQL Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public DataSet ExecuteStoredProcedureToDataSetAsync(
            string storedProcedureName,
            Dictionary<string, object> parameters,
            int commandTimeout = 1000)
        {
            var ds = new DataSet();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = commandTimeout;

                    // Add parameters dynamically
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds); // run in background
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL specific exception
                throw new Exception($"SQL Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Log general exception
                throw new Exception($"Error executing stored procedure: {ex.Message}", ex);
            }

            return ds;
        }

        public DataTable ExecuteStoredProcedureToDataTableAsync(
           string storedProcedureName,
           Dictionary<string, object> parameters,
           int commandTimeout = 1000)
        {
            var ds = new DataTable();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = commandTimeout;

                    // Add parameters dynamically
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(ds); // run in background
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL specific exception
                throw new Exception($"SQL Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Log general exception
                throw new Exception($"Error executing stored procedure: {ex.Message}", ex);
            }

            return ds;
        }
        public DataSet GetInvoiceRuleTemplate(int companyId, string siteName)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("Proc_Template_InvoiceRule", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyId", companyId);
                    command.Parameters.AddWithValue("@SiteName", siteName);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }


        public CompanyDetails GetAllCompanyDefaultBindData()//
        {
            CompanyDetails objCompanyRelatedData = new CompanyDetails();
            int i = 1;
            bool tableexists = true;

            using var connection = new SqlConnection(_connectionString);
            {
                try
                {
                    connection.Open();
                    using var command = new SqlCommand("sp_GetAllCompanyDefaultBindData", connection);
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@mode", 1);
                        command.Parameters.AddWithValue("@Value", 1);
                        command.CommandTimeout = 1500;

                        objCompanyRelatedData.GetEntityName = new List<EnumModel>();
                        objCompanyRelatedData.GetBankName = new List<EnumModel>();
                        objCompanyRelatedData.GetCompanyName = new List<EnumModel>();
                        objCompanyRelatedData.GetSegmentName = new List<EnumModel>();
                        objCompanyRelatedData.GetSubSegmentName = new List<EnumModel>();
                        objCompanyRelatedData.GetFinancialYear = new List<EnumModel>();
                        objCompanyRelatedData.GetAllCity = new List<EnumModel>();
                        objCompanyRelatedData.GetAllState = new List<EnumModel>();
                        objCompanyRelatedData.GetAllRegion = new List<EnumModel>();
                        objCompanyRelatedData.GetCompanyGroupCode = new List<EnumModel>();
                        objCompanyRelatedData.GetPfCode = new List<EnumModel>();
                        objCompanyRelatedData.GetCompanyType = new List<EnumModel>();
                        objCompanyRelatedData.GetReimbPayment = new List<EnumModel>();
                        objCompanyRelatedData.GetPayrollWithDecimal = new List<EnumModel>();
                        objCompanyRelatedData.GetPfCategory = new List<EnumModel>();
                        objCompanyRelatedData.GetServiceFeeWithDecimal = new List<EnumModel>();
                        objCompanyRelatedData.GetBankAdvice = new List<EnumModel>();
                        objCompanyRelatedData.GetVerticals = new List<EnumModel>();
                        objCompanyRelatedData.GetServiceChargeClubbing = new List<EnumModel>();
                        objCompanyRelatedData.GetBillingCompanyCodeList = new List<EnumModel>();

                        using var adapter = new SqlDataAdapter(command);
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.HasRows || tableexists)
                                {
                                    while (reader.Read())
                                    {
                                        EnumModel item = new EnumModel();
                                        switch (i)
                                        {
                                            case 1:
                                                item.Name = Convert.ToString(reader["Client_code"]);
                                                item.Value = Convert.ToString(reader["Client_Name"]);
                                                objCompanyRelatedData.GetCompanyName.Add(item);
                                                break;

                                            case 2:
                                                item.Name = Convert.ToString(reader["Entity_Name"]);
                                                item.Value = Convert.ToString(reader["Entity_Id"]);
                                                objCompanyRelatedData.GetEntityName.Add(item);
                                                break;

                                            case 3:
                                                item.Name = Convert.ToString(reader["Bank_Name"]);
                                                item.Value = Convert.ToString(reader["Bank_Id"]);
                                                objCompanyRelatedData.GetBankName.Add(item);
                                                break;

                                            case 4:
                                                item.Name = Convert.ToString(reader["Segment_name"]);
                                                item.Value = Convert.ToString(reader["Segment_id"]);
                                                objCompanyRelatedData.GetSegmentName.Add(item);
                                                break;

                                            case 5:
                                                item.Name = Convert.ToString(reader["Financial_Year_Name"]);
                                                item.Value = Convert.ToString(reader["Financial_Year_Id"]);
                                                objCompanyRelatedData.GetFinancialYear.Add(item);
                                                break;

                                            case 6:
                                                item.Name = Convert.ToString(reader["CITY_NAME"]);
                                                item.Value = Convert.ToString(reader["CITY_ID"]);
                                                objCompanyRelatedData.GetAllCity.Add(item);
                                                break;

                                            case 7:
                                                item.Name = Convert.ToString(reader["State_Name"]);
                                                item.Value = Convert.ToString(reader["State_Id"]);
                                                objCompanyRelatedData.GetAllState.Add(item);
                                                break;
                                            case 8:
                                                item.Name = Convert.ToString(reader["Region_Name"]);
                                                item.Value = Convert.ToString(reader["Region_Id"]);
                                                objCompanyRelatedData.GetAllRegion.Add(item);
                                                break;

                                            case 9:
                                                item.Name = Convert.ToString(reader["CompanyGroupCode"]);
                                                item.Value = Convert.ToString(reader["CompanyGroupId"]);
                                                objCompanyRelatedData.GetCompanyGroupCode.Add(item);
                                                break;

                                            case 10:
                                                item.Name = Convert.ToString(reader["PfCode"]);
                                                item.Value = Convert.ToString(reader["Id"]);
                                                objCompanyRelatedData.GetPfCode.Add(item);
                                                break;
                                            case 11:
                                                item.Name = Convert.ToString(reader["CompanyType"]);
                                                item.Value = Convert.ToString(reader["CompanyTypeId"]);
                                                objCompanyRelatedData.GetCompanyType.Add(item);
                                                break;
                                            case 12:
                                                item.Name = Convert.ToString(reader["GEN_vDescription"]);
                                                item.Value = Convert.ToString(reader["GEN_iID"]);
                                                objCompanyRelatedData.GetReimbPayment.Add(item);
                                                break;
                                            case 13:
                                                item.Name = Convert.ToString(reader["GEN_vDescription"]);
                                                item.Value = Convert.ToString(reader["GEN_iID"]);
                                                objCompanyRelatedData.GetPayrollWithDecimal.Add(item);
                                                break;
                                            case 14:
                                                item.Name = Convert.ToString(reader["GEN_vDescription"]);
                                                item.Value = Convert.ToString(reader["GEN_iID"]);
                                                objCompanyRelatedData.GetPfCategory.Add(item);
                                                break;
                                            case 15:
                                                item.Name = Convert.ToString(reader["GEN_vDescription"]);
                                                item.Value = Convert.ToString(reader["GEN_iID"]);
                                                objCompanyRelatedData.GetServiceFeeWithDecimal.Add(item);
                                                break;
                                            case 16:
                                                item.Name = Convert.ToString(reader["SubSegment_name"]);
                                                item.Value = Convert.ToString(reader["SubSegment_id"]);
                                                objCompanyRelatedData.GetSubSegmentName.Add(item);
                                                break;
                                            case 17:
                                                item.Name = Convert.ToString(reader["BankAdvice"]);
                                                item.Value = Convert.ToString(reader["BankAdviceId"]);
                                                objCompanyRelatedData.GetBankAdvice.Add(item);
                                                break;
                                            case 18:
                                                item.Name = Convert.ToString(reader["Vertical_Name"]);
                                                item.Value = Convert.ToString(reader["Vertical_Id"]);
                                                objCompanyRelatedData.GetVerticals.Add(item);
                                                break;
                                            case 19:
                                                item.Name = Convert.ToString(reader["ServiceChargeClubbing_Text"]);
                                                item.Value = Convert.ToString(reader["ServiceChargeClubbing_Id"]);
                                                objCompanyRelatedData.GetServiceChargeClubbing.Add(item);
                                                break;
                                            case 20:
                                                item.Name = Convert.ToString(reader["Company_Code"]);
                                                item.Value = Convert.ToString(reader["Company_Id"]);
                                                objCompanyRelatedData.GetBillingCompanyCodeList.Add(item);
                                                break;
                                        }


                                    }
                                    tableexists = reader.NextResult();
                                    i++;
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception e)
                {
                    connection.Close();
                    var a = e;
                }
                return objCompanyRelatedData;
            }

        }

        //public Company GetEditCopyData(int Companyid)
        //{
        //    Company getvalue = new Company();
        //    List<ContactPerson> lstContactPerson = new List<ContactPerson>();
        //    List<CompanyAddress> lstCompanyAddress = new List<CompanyAddress>();
           

        //    using var connection = new SqlConnection(_connectionString);
        //    {
        //        try
        //        {
        //            connection.Open();
        //            using var command = new SqlCommand("sp_GetCompanyDetailsData", connection);
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@mode", "Edit");
        //                command.Parameters.Add(new SqlParameter("Value1", Companyid));//Company Id
        //                command.Parameters.Add(new SqlParameter("Value2", ""));
        //                command.CommandTimeout = 1500;

        //                using var adapter = new SqlDataAdapter(command);
        //                {
        //                    int i = 1;
        //                    bool tableexists = true;

        //                    using (var reader = command.ExecuteReader())
        //                    {
        //                        while (reader.HasRows || tableexists)
        //                        {
        //                            while (reader.Read())
        //                            {
        //                                switch (i)
        //                                {
        //                                    case 1:
        //                                        #region Company Details
        //                                        getvalue.Client_Id = Convert.ToInt32(reader["Client_Id"]);
        //                                        getvalue.Company_Id = Convert.ToInt32(reader["Company_ID"]);
        //                                        getvalue.Company_Code = Convert.ToString(reader["Client_code"]);
        //                                        getvalue.Company_Name = Convert.ToString(reader["Client_Name"]);
        //                                        getvalue.Financial_Year_Id = Convert.ToInt32(reader["Financial_Year_Id"]);
        //                                        getvalue.Zone_Tagging = Convert.ToString(reader["Zone_Tagging"]);
        //                                        getvalue.Mis_Name = Convert.ToString(reader["Mis_Name"]);
        //                                        getvalue.PfCode_Id = string.IsNullOrEmpty(Convert.ToString(reader["PfCode_Id"])) ? 0 : Convert.ToInt32(reader["PfCode_Id"]);
        //                                        getvalue.Client_Since = Convert.ToString(reader["Client_Since"]);
        //                                        getvalue.Company_Active = Convert.ToBoolean(reader["Company_Active"]);
        //                                        getvalue.Payroll_Type = Convert.ToBoolean(reader["Payroll_Type"]);
        //                                        getvalue.Is_Zip_Documents = Convert.ToBoolean(reader["Is_Zip_Documents"]);
        //                                        getvalue.Invoicing_Type = Convert.ToBoolean(reader["Invoicing_Type"]);
        //                                        getvalue.Investment_Block_Date = Convert.ToString(reader["Investment_Block_Date"]);
        //                                        getvalue.Month_Days = Convert.ToBoolean(reader["Month_Days"]);
        //                                        getvalue.Salary_Fix_Days = Convert.ToInt32(reader["Salary_Fix_Days"]);
        //                                        getvalue.Business_Unit_Location_Id = Convert.ToInt32(reader["Business_Unit_Location_Id"]);
        //                                        getvalue.Business_Unit_Name_Id = Convert.ToInt32(reader["Business_Unit_Name_Id"]);
        //                                        getvalue.Attendance_Cycle_From = Convert.ToInt32(reader["Attendance_Cycle_From"]);
        //                                        getvalue.Attendance_Cycle_To = Convert.ToInt32(reader["Attendance_Cycle_To"]);
        //                                        getvalue.Is_PF_Remittance = Convert.ToBoolean(reader["Is_PF_Remittance"]);
        //                                        getvalue.Input_Date = Convert.ToInt32(reader["Input_Date"]);
        //                                        getvalue.Output_Date = Convert.ToInt32(reader["Output_Date"]);
        //                                        getvalue.Work_Days_Based_On = Convert.ToBoolean(reader["Work_Days_Based_On"]);
        //                                        getvalue.CTC = Convert.ToBoolean(reader["CTC"]);
        //                                        getvalue.Sourcing_Fee_Criteria_Type = Convert.ToBoolean(reader["Sourcing_Fee_Criteria_Type"]);
        //                                        getvalue.Sourcing_Fee = Convert.ToDecimal(reader["Sourcing_Fee"]);
        //                                        getvalue.Absorption_Fee_Criteria_Type = Convert.ToBoolean(reader["Absorption_Fee_Criteria_Type"]);
        //                                        getvalue.Absorption_Fee = Convert.ToDecimal(reader["Absorption_Fee"]);
        //                                        getvalue.Incentive_Type = Convert.ToInt32(reader["Incentive_Type"]);
        //                                        getvalue.Is_PO_Applicable = Convert.ToBoolean(reader["Is_PO_Applicable"]);
        //                                        getvalue.Is_PO_Wise_Batch = Convert.ToBoolean(reader["Is_PO_Wise_Batch"]); //Added By Vijay On 16/05/2019
        //                                        getvalue.Salary_SMS = Convert.ToBoolean(reader["Salary_SMS"]);
        //                                        getvalue.Dues_Based_On = Convert.ToBoolean(reader["Dues_Based_On"]);
        //                                        getvalue.Is_Insurance_Applicable = Convert.ToBoolean(reader["Is_Insurance_Applicable"]);
        //                                        getvalue.Invoice_Format = Convert.ToString(reader["Invoice_Format"]);
        //                                        getvalue.ReimbInvoiceFormat_Id = Convert.ToString(reader["ReimbInvoiceFormat_Id"]);
        //                                        getvalue.Segment_Id = Convert.ToInt32(reader["Segment_Id"]);
        //                                        getvalue.SubSegment_Id = Convert.ToInt32(reader["SubSegment_Id"]);
        //                                        getvalue.Payslip_Format = Convert.ToString(reader["Payslip_Format"]);
        //                                        getvalue.Mode_Of_Payment = Convert.ToBoolean(reader["Mode_Of_Payment"]);
        //                                        getvalue.TAT = Convert.ToInt32(reader["TAT"]);
        //                                        getvalue.Billing_Type = Convert.ToBoolean(reader["Billing_Type"]);
        //                                        getvalue.Is_RoundOff_Applicable = Convert.ToInt32(reader["Is_RoundOff_Applicable"]);
        //                                        getvalue.Deviation = Convert.ToBoolean(reader["Deviation"]);
        //                                        getvalue.Incharge = Convert.ToString(reader["Incharge"]);
        //                                        getvalue.Credit_Days_Upfront = Convert.ToInt64(reader["Credit_Days_Upfront"]);
        //                                        getvalue.Customer_Type = Convert.ToInt32(reader["Customer_Type"]);
        //                                        getvalue.Incentive_Date = Convert.ToInt32(reader["Incentive_Date"]);
        //                                        getvalue.SAP_Code = Convert.ToString(reader["SAP_Code"]);
        //                                        getvalue.Company_Contract_Id = Convert.ToInt32(reader["Company_Contract_id"]);
        //                                        getvalue.Contract_Start_Date = Convert.ToString(reader["Contract_Start_Date"]);
        //                                        getvalue.Contract_End_Date = Convert.ToString(reader["Contract_End_Date"]);
        //                                        getvalue.Service_Tax_Applicable = Convert.ToBoolean(reader["Service_Tax_Applicable"]);
        //                                        getvalue.Company_Service_Tax_Id = Convert.ToInt32(reader["Company_service_tax_id"]);
        //                                        getvalue.Service_Tax_Date = Convert.ToString(reader["service_tax_date"]);

        //                                        getvalue.Contract_File_Name = Convert.ToString(reader["Contract_File_Name"]);
        //                                        getvalue.Contract_Uploaded_File_Name = Convert.ToString(reader["Contract_Uploaded_File_Name"]);
        //                                        getvalue.Service_Tax_File_Name = Convert.ToString(reader["Service_Tax_File_Name"]);

        //                                        getvalue.Reimbursement_Type = Convert.ToInt32(reader["Reimbursement_Type"]);
        //                                        getvalue.Salary_Transfer_Date = Convert.ToInt32(reader["Salary_Transfer_Date"]);
        //                                        getvalue.Effective_Date = Convert.ToString(reader["Effective_Date"]);
        //                                        getvalue.Sales_Person = Convert.ToString(reader["Sales_Person"]);
        //                                        getvalue.Branch_Location = Convert.ToString(reader["Branch_Location"]);
        //                                        getvalue.Reimbursement_Date = Convert.ToInt32(reader["Reimbursement_Date"]);

        //                                        getvalue.Company_Bank_Details_Id = Convert.ToInt32(reader["Company_Bank_Details_Id"]);
        //                                        getvalue.Account_Number = Convert.ToString(reader["Account_Number"]);
        //                                        getvalue.IFSC_Code = Convert.ToString(reader["IFSC_Code"]);
        //                                        getvalue.Bank_Address = Convert.ToString(reader["Bank_Address"]);
        //                                        getvalue.Bank_Name = Convert.ToString(reader["Bank_Name"]);
        //                                        getvalue.Bank_Id = Convert.ToInt32(reader["Bank_Id"]);

        //                                        getvalue.City_Id = Convert.ToInt64(reader["City_Id"]);
        //                                        getvalue.City_Name = Convert.ToString(reader["City_Name"]);
        //                                        getvalue.Address = Convert.ToString(reader["Address"]);
        //                                        getvalue.Pin_Code = Convert.ToString(reader["Pin_Code"]);
        //                                        getvalue.Phone_Number = Convert.ToString(reader["Phone_Number"]);
        //                                        getvalue.PAN_Number = Convert.ToString(reader["PAN_Number"]);
        //                                        getvalue.TAN_Number = Convert.ToString(reader["TAN_Number"]);
        //                                        getvalue.Service_Tax_Number = Convert.ToString(reader["Service_Tax_Number"]);
        //                                        getvalue.PF_Code = Convert.ToString(reader["PF_Code"]);
        //                                        getvalue.ESI_Code = Convert.ToString(reader["ESI_Code"]);
        //                                        getvalue.PT_Code = Convert.ToString(reader["PT_Code"]);
        //                                        getvalue.Email_Id = Convert.ToString(reader["Email_Id"]);
        //                                        getvalue.Certificate_Number = Convert.ToString(reader["Certificate_Number"]);
        //                                        getvalue.Fax_Number = Convert.ToString(reader["Fax_Number"]);
        //                                        getvalue.Website_Name = Convert.ToString(reader["Website_Name"]);
        //                                        getvalue.State_Name = Convert.ToString(reader["State_Name"]);

        //                                        getvalue.Slabseries = Convert.ToInt32(reader["Slabseries"]);
        //                                        getvalue.Slabctc = Convert.ToInt32(reader["Slabctc"]);
        //                                        getvalue.SlabAttendance = Convert.ToInt32(reader["SlabAttendance"]);
        //                                        getvalue.SlabFixed = Convert.ToInt32(reader["SlabFixed"]);
        //                                        getvalue.Suplleseries = Convert.ToInt32(reader["Suplleseries"]);
        //                                        getvalue.Supllctc = Convert.ToInt32(reader["Supllctc"]);
        //                                        getvalue.SuplleFixed = Convert.ToInt32(reader["SuplleFixed"]);
        //                                        getvalue.Wages = Convert.ToBoolean(reader["Minimum_Wages"]);
        //                                        getvalue.Particulars = Convert.ToString(reader["Particulars"]);
        //                                        getvalue.Auto_Company_code = Convert.ToString(reader["Auto_Company_code"]);
        //                                        getvalue.Is_NonInvoice = Convert.ToBoolean(reader["Is_NonInvoice"]);
        //                                        getvalue.IsHeaderFooter = Convert.ToBoolean(reader["IsHeaderFooter"]);//Anant on 27July2018 for Choose header and footer
        //                                        getvalue.IsDecimal = Convert.ToBoolean(reader["IsDecimal"]);//Anant on 19 Oct 2019 for Choose to PDF in Decimal 
        //                                        getvalue.IsProforma = Convert.ToBoolean(reader["IsProforma"]);
        //                                        getvalue.Manual_NewJoinee = Convert.ToBoolean(reader["Manual_NewJoinee"]);
        //                                        //Rudra Changes
        //                                        getvalue.Sap_Customer_Code = Convert.ToString(reader["Sap_Customer_Code"]);
        //                                        getvalue.Profit_Center_Code = Convert.ToString(reader["Profit_Center_Code"]);
        //                                        //Rudra Changes

        //                                        getvalue.Inedge_charges_Criteria_Type = Convert.ToBoolean(reader["Inedge_charges_Criteria_Type"]);//Anant on 5Sep18 for Include Inedgecharges
        //                                        getvalue.Inedge_charges = Convert.ToDecimal(reader["Inedge_charges"]);// 

        //                                        getvalue.InEdge_Category = Convert.ToInt32(reader["InEdge_Category"]);//Anant on 31-Oct-18 for Include InEdge_Category
        //                                        getvalue.OnBoarding_Category = Convert.ToInt32(reader["OnBoarding_Category"]);//Anant on 31-Oct-18 for Include OnBoarding_Category
        //                                                                                                                      // getvalue. = Convert.ToString(reader["Profit_Center_Code"]);
        //                                        getvalue.CompanyGroupCode = reader["CompanyGroupCode"].ToString();
        //                                        getvalue.CompanyGroupName = reader["CompanyGroupName"].ToString();
        //                                        getvalue.WorkingHours = reader["WorkingHours"].ToString();
        //                                        //getvalue.Is_NonInvoice
        //                                        //getvalue.Is_NonInvoice = Convert.ToBoolean(reader["Is_NonInvoice"]);
        //                                        getvalue.IsBonusPayThroughFF = Convert.ToBoolean(reader["IsBonusPayThroughFF"]);
        //                                        getvalue.IsExtraWorkingDaysServiceFee = Convert.ToBoolean(reader["IsExtraWorkingDaysServiceFee"]);
        //                                        getvalue.AttendanceInputWithLeave = Convert.ToBoolean(reader["AttendanceInputWithLeave"]);
        //                                        getvalue.Management_MIS = Convert.ToString(reader["Management_MIS"]);
        //                                        getvalue.CompanyType = Convert.ToString(reader["CompanyType"]);
        //                                        getvalue.Invoice_Submission_Date = Convert.ToInt32(reader["Invoice_Submission_Date"]);
        //                                        getvalue.Collection_Date = Convert.ToInt32(reader["Collection_Date"]);
        //                                        getvalue.PE_User_ID = Convert.ToString(reader["PE_User_ID"]);
        //                                        getvalue.PE_Name = Convert.ToString(reader["PE_Name"]);
        //                                        getvalue.PE_Email_Id = Convert.ToString(reader["PE_Email_Id"]);
        //                                        getvalue.RM_User_ID = Convert.ToString(reader["RM_User_ID"]);
        //                                        getvalue.RM_Name = Convert.ToString(reader["RM_Name"]);
        //                                        getvalue.RM_Email_Id = Convert.ToString(reader["RM_Email_Id"]);
        //                                        getvalue.Client_SPOC_Name = Convert.ToString(reader["Client_SPOC_Name"]);
        //                                        getvalue.Client_SPOC_Email_Id = Convert.ToString(reader["Client_SPOC_Email_Id"]);
        //                                        getvalue.Client_SPOC_Mobile_No = Convert.ToString(reader["Client_SPOC_Mobile_No"]);
        //                                        getvalue.Client_Escalation_Manager_Name = Convert.ToString(reader["Client_Escalation_Manager_Name"]);
        //                                        getvalue.Client_Escalation_Manager_Email_Id = Convert.ToString(reader["Client_Escalation_Manager_Email_Id"]);
        //                                        getvalue.Client_Escalation_Manager_Mobile_No = Convert.ToString(reader["Client_Escalation_Manager_Mobile_No"]);
        //                                        getvalue.Portal_Payslip_Format = Convert.ToString(reader["Portal_Payslip_Format"]);
        //                                        getvalue.IsNewJoinee = Convert.ToBoolean(reader["IsNewJoinee"]);
        //                                        getvalue.ReimbPaymentId = string.IsNullOrEmpty(Convert.ToString(reader["ReimbPaymentId"])) ? 0 : Convert.ToInt32(reader["ReimbPaymentId"]);
        //                                        getvalue.PayrollWithDecimalId = string.IsNullOrEmpty(Convert.ToString(reader["PayrollWithDecimalId"])) ? 0 : Convert.ToInt32(reader["PayrollWithDecimalId"]);
        //                                        getvalue.PfCategoryId = string.IsNullOrEmpty(Convert.ToString(reader["PfCategoryId"])) ? 0 : Convert.ToInt32(reader["PfCategoryId"]);
        //                                        getvalue.IsSignature = Convert.ToBoolean(reader["IsSignature"]);
        //                                        getvalue.ServiceFeeWithDecimalId = string.IsNullOrEmpty(Convert.ToString(reader["ServiceFeeWithDecimalId"])) ? 0 : Convert.ToInt32(reader["ServiceFeeWithDecimalId"]);
        //                                        getvalue.Qdemy_charges = Convert.ToDecimal(reader["Qdemy_charges"]);
        //                                        getvalue.IsCurrencyConversion = Convert.ToBoolean(reader["Is_Conversion"]);
        //                                        getvalue.TechSubscriptionCharges = Convert.ToDecimal(reader["TechSubscriptionCharges"]);
        //                                        getvalue.Tech_Subscription_Charges_Criteria_Type = Convert.ToBoolean(reader["Tech_Subscription_Charges_Criteria_Type"]);
        //                                        getvalue.BankAdviceId = string.IsNullOrEmpty(Convert.ToString(reader["BankAdviceId"])) ? 0 : Convert.ToInt32(reader["BankAdviceId"]);
        //                                        getvalue.DigitalPlatformConsent = string.IsNullOrEmpty(Convert.ToString(reader["DigitalPlatformConsent"])) ? 0 : Convert.ToInt32(reader["DigitalPlatformConsent"]);
        //                                        getvalue.DGPSF = string.IsNullOrEmpty(Convert.ToString(reader["DGPSF"])) ? 0 : Convert.ToInt32(reader["DGPSF"]);
        //                                        getvalue.Vertical_Id = string.IsNullOrEmpty(Convert.ToString(reader["Vertical_Id"])) ? 0 : Convert.ToInt32(reader["Vertical_Id"]);
        //                                        getvalue.ServiceChargeClubbing = string.IsNullOrEmpty(Convert.ToString(reader["ServiceChargeClubbing"])) ? 0 : Convert.ToInt32(reader["ServiceChargeClubbing"]);
        //                                        getvalue.IsOneTouchInvoicing = Convert.ToInt32(reader["IsOneTouchInvoicing"]);
        //                                        getvalue.IsInvoicePoBased = Convert.ToInt32(reader["IsInvoicePoBased"]);
        //                                        getvalue.Sector = Convert.ToString(reader["Sector"]);
        //                                        getvalue.IS_ESI_split = Convert.ToInt32(reader["IS_ESI_split"]);
        //                                        getvalue.Is40BillingModel = Convert.ToBoolean(reader["Is40BillingModel"]);
        //                                        getvalue.BillingCompanyId = string.IsNullOrEmpty(Convert.ToString(reader["BillingCompanyId"])) ? 0 : Convert.ToInt32(reader["BillingCompanyId"]);

        //                                        #endregion
        //                                        break;
        //                                    case 2:
        //                                        Com.ContactPerson item = new Com.ContactPerson();
        //                                        item.Company_Contact_Id = Convert.ToInt32(reader["Company_Contact_Id"]);
        //                                        item.Company_Id = Convert.ToInt32(reader["Company_Id"]);
        //                                        item.Contact_Name = Convert.ToString(reader["Contact_Name"]);
        //                                        item.Designation_Name = Convert.ToString(reader["Designation_Name"]);
        //                                        item.Department_Name = Convert.ToString(reader["Department_Name"]);
        //                                        item.Email_Id = Convert.ToString(reader["Email_Id"]);
        //                                        item.Phone_Number = Convert.ToString(reader["Phone_Number"]);
        //                                        lstContactPerson.Add(item);
        //                                        break;
        //                                    case 3:
        //                                        Com.CompanyServiceCharge itemser = new Com.CompanyServiceCharge();
        //                                        itemser.Company_Id = Convert.ToInt32(reader["Company_Id"]);
        //                                        itemser.Company_Service_Charge_Id = Convert.ToInt32(reader["Company_Service_Charge_Id"]);
        //                                        itemser.Company_Service_Charge_Master_Id = Convert.ToInt32(reader["Company_Service_Charge_Master_Id"]);
        //                                        itemser.Company_Service_Charge_Type_Id = Convert.ToInt32(reader["Company_Service_Charge_Type_Id"]);
        //                                        itemser.Fixed_Month_Days = Convert.ToInt32(reader["Fixed_Month_Days"]);
        //                                        itemser.Criteria = Convert.ToString(reader["Criteria"]);
        //                                        itemser.Criteria_Id = Convert.ToString(reader["Criteria_Id"]);
        //                                        itemser.Service_Charge_Amount = Convert.ToDouble(reader["Service_Charge_Amount"]);
        //                                        itemser.Service_Charge_Fixed = Convert.ToDouble(reader["Service_Charge_Fixed"]);
        //                                        itemser.Service_Charge_Percentage = Convert.ToDecimal(reader["Service_Charge_Percentage"]);
        //                                        itemser.Static_Paycode_Id = Convert.ToInt32(reader["Pay_Code_Id"]);
        //                                        itemser.Static_Paycode_Text = Convert.ToString(reader["Static_Paycode_Text"]);
        //                                        lstCompanyServiceCharge.Add(itemser);
        //                                        break;
        //                                    case 4:
        //                                        Com.CompanyServiceCharge1 itemser1 = new Com.CompanyServiceCharge1();
        //                                        itemser1.Company_Id = Convert.ToInt32(reader["Company_Id"]);
        //                                        itemser1.Service_Charge_Id = Convert.ToInt32(reader["Service_Charge_Id"]);
        //                                        itemser1.Company_Service_Charge_Master_Id = Convert.ToInt32(reader["Service_Charge_Master_Id"]);
        //                                        itemser1.Company_Service_Charge_Type_Id = Convert.ToInt32(reader["Service_Charge_Type_Id"]);
        //                                        itemser1.Service_Charge_Slab_Item_Id = Convert.ToInt32(reader["Service_Charge_Slab_Item_Id"]);
        //                                        itemser1.Service_Charge_Slab_Inner_Item_Id = Convert.ToInt32(reader["Service_Charge_Slab_Inner_Item_Id"]);
        //                                        itemser1.Cost_Center_Mapping_Id = Convert.ToInt32(reader["Map_Name_Id"]);
        //                                        itemser1.Map_Name = Convert.ToString(reader["Map_Name"]);
        //                                        itemser1.Service_Charge_Name = Convert.ToString(reader["Service_Charge_Name"]);
        //                                        //itemser1.PayCode = Convert.ToString(reader["PayCode"]);
        //                                        itemser1.PayCode_Id = Convert.ToInt32(reader["PayCode_Id"]); // method for getting pay codes based on company - Added By Vijay - 15Nov2016
        //                                        itemser1.PayCode_Code = Convert.ToString(reader["PayCode_Code"]); // method for getting pay codes based on company - Added By Vijay - 15Nov2016
        //                                        itemser1.MaxAmount = Convert.ToDecimal(reader["MaxAmount"]);
        //                                        itemser1.Type = Convert.ToInt32(reader["Types"]);
        //                                        itemser1.Type_Name = Convert.ToString(reader["Type_Names"]);
        //                                        itemser1.Invoicing_Type = Convert.ToBoolean(reader["Invoicing_Type"]);

        //                                        itemser1.Slab_Id = Convert.ToString(reader["Slab_Id"]);
        //                                        // itemser1.Slab_Name = Convert.ToString(reader["Slab_Name"]);
        //                                        itemser1.Value = Convert.ToString(reader["Value"]);
        //                                        itemser1.Effective_Date = Convert.ToString(reader["Effective_Date"]);
        //                                        itemser1.IsBillToRate = Convert.ToInt32(reader["IsBillToRate"]);
        //                                        itemser1.IsCTC = Convert.ToInt32(reader["IsCTC"]);
        //                                        itemser1.IsHeadCount = Convert.ToInt32(reader["IsHeadCount"]);
        //                                        itemser1.IsAttendanceProrated = Convert.ToInt32(reader["IsAttendanceProrated"]);
        //                                        itemser1.IsAttendanceProrated_Text = Convert.ToString(reader["IsAttendanceProrated_Text"]);
        //                                        itemser1.IsCriteriaApplicable = Convert.ToInt32(reader["IsCriteriaApplicable"]);
        //                                        itemser1.IsCriteriaApplicable_Text = Convert.ToString(reader["IsCriteriaApplicable_Text"]);
        //                                        itemser1.Criteria = Convert.ToString(reader["Criteria"]);
        //                                        itemser1.IsReplacementClauseApplicable = Convert.ToInt32(reader["IsReplacementClauseApplicable"]);
        //                                        itemser1.IsReplacementClauseApplicable_Text = Convert.ToString(reader["IsReplacementClauseApplicable_Text"]);
        //                                        itemser1.Replacement = Convert.ToDecimal(reader["Replacement"]);
        //                                        itemser1.IsSourcingWaitingPeriod_Id = Convert.ToInt32(reader["IsSourcingWaitingPeriod"]);
        //                                        itemser1.IsSourcingWaitingPeriod_Text = Convert.ToString(reader["IsSourcingWaitingPeriod_Text"]);
        //                                        itemser1.SourcingValue = Convert.ToDecimal(reader["SourcingValue"]);
        //                                        itemser1.TATDays = Convert.ToInt32(reader["TATDays"]);
        //                                        itemser1.IsMapNameRequired = Convert.ToInt32(reader["IsMapNameRequired"]);
        //                                        itemser1.IsMapnamerequire_Text = Convert.ToString(reader["IsMapnamerequire_Text"]);
        //                                        itemser1.Category_Id = Convert.ToInt32(reader["Category_Id"]);
        //                                        itemser1.Category_Name = Convert.ToString(reader["Category_Name"]);
        //                                        itemser1.Invoice_Map_Name_Id = Convert.ToInt32(reader["Invoice_Map_Name_Id"]);
        //                                        itemser1.Invoice_Map_Name = Convert.ToString(reader["Invoice_Map_Name"]);
        //                                        itemser1.Compliance_Fee = Convert.ToDecimal(reader["Compliance_Fee"]);
        //                                        itemser1.RandStad_Fee = Convert.ToDecimal(reader["RandStad_Fee"]);
        //                                        itemser1.UnitType_Id = Convert.ToInt32(reader["UnitType_Id"]);
        //                                        itemser1.Unit_Type = Convert.ToString(reader["Unit_Type"]);
        //                                        itemser1.Discount_Type_Id = Convert.ToInt32(reader["Discount_Type_Id"]);
        //                                        itemser1.Discount_Type = Convert.ToString(reader["Discount_Type"]);
        //                                        itemser1.Discount_Amount = Convert.ToDecimal(reader["Discount_Amount"]);
        //                                        itemser1.Type_Id = Convert.ToInt32(reader["Type_Id"]);
        //                                        itemser1.Types = Convert.ToString(reader["TYPES"]);
        //                                        itemser1.Pay_Code_Id = Convert.ToInt32(reader["Pay_Code_Id"]);
        //                                        itemser1.Pay_Code = Convert.ToString(reader["Pay_Code"]);
        //                                        itemser1.From = Convert.ToInt32(reader["From"]);
        //                                        itemser1.To = Convert.ToInt32(reader["To"]);
        //                                        itemser1.Slab_Calculation_Type_Id = Convert.ToInt32(reader["Slab_Calculation_Type_Id"]);
        //                                        itemser1.Slab_Calculation_Type = Convert.ToString(reader["Slab_Calculation_Type"]);
        //                                        itemser1.Cap_Value = Convert.ToDecimal(reader["Cap_Value"]);
        //                                        itemser1.Upfront_Charge = Convert.ToDecimal(reader["Upfront_Charge"]);
        //                                        itemser1.Upfront_PayCode = Convert.ToString(reader["Upfront_PayCode"]);
        //                                        itemser1.Upfront_Type_Id = Convert.ToInt32(reader["Upfront_Type_Id"]);
        //                                        itemser1.Upfront_Type = Convert.ToString(reader["Upfront_Type"]);
        //                                        itemser1.Insurance_Amount = Convert.ToDecimal(reader["Insurance_Amount"]);
        //                                        itemser1.MarginalPayCodeId = Convert.ToInt32(reader["MarginalPayCodeId"]);
        //                                        itemser1.MarginalPayCode = Convert.ToString(reader["MarginalPayCode"]);
        //                                        itemser1.QDemyFee = Convert.ToDecimal(reader["QDemyFee"]);
        //                                        itemser1.InEdgeFee = Convert.ToDecimal(reader["InEdgeFee"]);
        //                                        itemser1.BTP_Training = Convert.ToDecimal(reader["BTP_Training"]);
        //                                        itemser1.Govt_Grants_Subsidy = Convert.ToDecimal(reader["Govt_Grants_Subsidy"]);
        //                                        itemser1.Assessment_And_Certificatioin_Cost = Convert.ToDecimal(reader["Assessment_And_Certificatioin_Cost"]);
        //                                        itemser1.Registration_Fee = Convert.ToDecimal(reader["Registration_Fee"]);
        //                                        itemser1.IsNewjoineeProrate = Convert.ToInt32(reader["IsNewjoineeProrate"]);
        //                                        itemser1.IsNewjoineeProrate_Text = Convert.ToString(reader["IsNewjoineeProrate_Text"]);
        //                                        itemser1.IsFAndFProrate = Convert.ToInt32(reader["IsFAndFProrate"]);
        //                                        itemser1.IsFAndFProrate_Text = Convert.ToString(reader["IsFAndFProrate_Text"]);
        //                                        itemser1.IsFAndFArrearProrate = Convert.ToInt32(reader["IsFAndFArrearProrate"]);
        //                                        itemser1.IsFAndFArrearProrate_Text = Convert.ToString(reader["IsFAndFArrearProrate_Text"]);
        //                                        itemser1.IsNewJoineeArrearProrate = Convert.ToInt32(reader["IsNewJoineeArrearProrate"]);
        //                                        itemser1.IsNewJoineeArrearProrate_Text = Convert.ToString(reader["IsNewJoineeArrearProrate_Text"]);
        //                                        itemser1.QDemyFee_Type_Id = Convert.ToInt32(reader["QDemyFee_Type_Id"]);
        //                                        itemser1.QDemyFee_Type = Convert.ToString(reader["QDemyFee_Type"]);
        //                                        itemser1.InEdgeFee_Type_Id = Convert.ToInt32(reader["InEdgeFee_Type_Id"]);
        //                                        itemser1.InEdgeFee_Type = Convert.ToString(reader["InEdgeFee_Type"]);
        //                                        lstCompanyServiceChargeDetail.Add(itemser1);
        //                                        break;


        //                                    case 5:
        //                                        Com.CompanyAddress itemadd = new Com.CompanyAddress();
        //                                        itemadd.Company_Address_Detail_Id = Convert.ToInt32(reader["Company_Address_Detail_Id"]);
        //                                        itemadd.Location = Convert.ToInt32(reader["Location"]);
        //                                        //Shailendra V2
        //                                        itemadd.Cost_Center_Mapping_Id = Convert.ToInt32(reader["Cost_Center_Mapping_Id"]);
        //                                        itemadd.Map_Name = Convert.ToString(reader["Map_Name"]);
        //                                        //Shailendra V2
        //                                        itemadd.Location_Name = Convert.ToString(reader["Location_Name"]);
        //                                        itemadd.Invoice_Location = Convert.ToInt32(reader["Invoice_Location"]);
        //                                        itemadd.Invoice_Location_Name = Convert.ToString(reader["Invoice_Location_Name"]);
        //                                        itemadd.Address = Convert.ToString(reader["Address"]);
        //                                        itemadd.City_Id = Convert.ToInt32(reader["City_Id"]);
        //                                        itemadd.City_Name = Convert.ToString(reader["City_name"]);
        //                                        itemadd.Pin_Code = Convert.ToString(reader["Pin_Code"]);
        //                                        itemadd.Cost_Code = Convert.ToString(reader["Cost_Code"]);
        //                                        itemadd.Circle_Code = Convert.ToString(reader["Circle_Code"]);
        //                                        itemadd.Certificate_Number = Convert.ToString(reader["Certificate_Number"]);
        //                                        itemadd.Fax_Number = Convert.ToString(reader["Fax_Number"]);
        //                                        itemadd.Website_Name = Convert.ToString(reader["Website_Name"]);
        //                                        itemadd.Phone_Number = Convert.ToString(reader["Phone_Number"]);
        //                                        itemadd.TAN_Number = Convert.ToString(reader["TAN_Number"]);
        //                                        itemadd.PF_Code = Convert.ToString(reader["PF_Code"]);
        //                                        itemadd.PT_Code = Convert.ToString(reader["PT_Code"]);
        //                                        itemadd.PAN_Number = Convert.ToString(reader["PAN_Number"]);
        //                                        itemadd.Service_Tax_Number = Convert.ToString(reader["Service_Tax_Number"]);
        //                                        itemadd.ESI_Code = Convert.ToString(reader["ESI_Code"]);
        //                                        itemadd.Billing_Client_Name = Convert.ToString(reader["Billing_Client_Name"]);
        //                                        itemadd.Billing_Client_Address1 = Convert.ToString(reader["Billing_Client_Address1"]);
        //                                        itemadd.Billing_Client_Address2 = Convert.ToString(reader["Billing_Client_Address2"]);
        //                                        itemadd.Shipment_Client_Name = Convert.ToString(reader["Shipment_Client_Name"]);
        //                                        itemadd.Shipment_Client_Address1 = Convert.ToString(reader["Shipment_Client_Address1"]);
        //                                        itemadd.Shipment_Client_Address2 = Convert.ToString(reader["Shipment_Client_Address2"]);
        //                                        itemadd.State_Id = Convert.ToInt32(reader["State_Id"]);
        //                                        itemadd.State_Name = Convert.ToString(reader["state_name"]);
        //                                        itemadd.Email_Id = Convert.ToString(reader["Email_Id"]);
        //                                        lstCompanyAddress.Add(itemadd);
        //                                        break;
        //                                    case 6:
        //                                        Com.QrsServiceCharge qrs = new Com.QrsServiceCharge();
        //                                        qrs.QRS_Service_Charge_Id = Convert.ToInt32(reader["QRS_Service_Charge_Id"]);
        //                                        qrs.Company_Id = Convert.ToInt32(reader["Company_Id"]);
        //                                        qrs.Cost_Center_Mapping_Id = Convert.ToInt32(reader["Cost_Center_Mapping_Id"]);
        //                                        qrs.Map_Name = Convert.ToString(reader["Map_Name"]);
        //                                        qrs.QRS_Service_Charge_Type_Id = Convert.ToInt32(reader["QRS_Service_Charge_Type_Id"]);
        //                                        qrs.QRS_Service_Charge_Type = Convert.ToString(reader["QRS_Service_Charge_Type"]);
        //                                        qrs.QRS_Service_Charge_Type_Value_Id = Convert.ToInt32(reader["QRS_Service_Charge_Type_Value_Id"]);
        //                                        qrs.QRS_Service_Charge_Type_Value = Convert.ToString(reader["QRS_Service_Charge_Type_Value"]);
        //                                        qrs.QRS_Service_Charge_Type_Value1_Id = Convert.ToInt32(reader["QRS_Service_Charge_Type_Value1_Id"]);
        //                                        qrs.QRS_Service_Charge_Type_Value1 = Convert.ToString(reader["QRS_Service_Charge_Type_Value1"]);
        //                                        qrs.QRS_Service_Charge_Category_Id = Convert.ToInt32(reader["QRS_Service_Charge_Category_Id"]);
        //                                        qrs.QRS_Service_Charge_Category = Convert.ToString(reader["QRS_Service_Charge_Category"]);
        //                                        qrs.From = Convert.ToInt32(reader["From"]);
        //                                        qrs.To = Convert.ToInt32(reader["To"]);
        //                                        qrs.Value = Convert.ToString(reader["Value"]);
        //                                        qrs.Cap_Value = Convert.ToDecimal(reader["Cap_Value"]);
        //                                        qrs.Effective_Date = Convert.ToString(reader["Effective_Date"]);
        //                                        lstQrsServiceCharge.Add(qrs);
        //                                        break;

        //                                }

        //                            }
        //                        }
        //                        i++;
        //                        tableexists = reader.NextResult();
        //                    }
        //                }
        //            }

        //            getvalue.objContactPerson = new Com.ContactPerson();
        //            getvalue.objContactPerson.lstContactPerson = lstContactPerson;

        //            getvalue.lstCompanyAddress = new List<Com.CompanyAddress>();
        //            getvalue.lstCompanyAddress = lstCompanyAddress;
                    
        //          return getvalue;
        //        }
        //        catch(Exception ex)
        //        {

        //        }

        //    }
           
        //}

        public DataSet InvoiceRuleExport(int companyId, int siteCode)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("Proc_GetAllInvoicingRule_Export", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyId", companyId);
                    command.Parameters.AddWithValue("@Site_id", siteCode);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataSet LeaveMasterExport(int companyId, int siteCode)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("Proc_GetAllLeaveType", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyId", companyId);
                    command.Parameters.AddWithValue("@Site_id", siteCode);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds; 
        }

        public DataSet DepartmentExport(int companyId)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("sp_GetAllDepartmentDetailExportToExcel", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataSet DesignationExport(int companyId)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("sp_GetAllDesignationDetailExportToExcel", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
        public DataSet CostCenterExport(string? CostCenterMapName)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("Proc_GetAllCostCenterMappingDetailsExportToExcel", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CostCenterMapname", CostCenterMapName);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataSet ClientAddressExport(int userId)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("Proc_ManageClientAddress", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", "Export");
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@PageNo", 1);
                    command.Parameters.AddWithValue("@PageSize", 999999);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataSet POInvoiceInitiateExport(int companyId, int payPeriodId)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("Proc_ManageMagnaGstInvoiceInitiate", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Company_Id", companyId);
                    command.Parameters.AddWithValue("@Pay_Period_Id", payPeriodId);
                    command.Parameters.AddWithValue("@Action", "ExportToExcel");
                    

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataSet InvoiceCultureExport(int companyId)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("sp_GetAllInvoiceCultureExportToExcel", connection);
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using var adapter = new SqlDataAdapter(command);
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public async Task<List<object>> QueryMultipleAsync(string sql, Type[] resultTypes, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = new SqlConnection(_connectionString);

            var results = new List<object>();
            try
            {
                using var multi = await connection.QueryMultipleAsync(sql, parameters, commandType: commandType);
                foreach (var t in resultTypes)
                {
                    var result = await multi.ReadAsync(t, buffered: true);
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {

            }


            return results;
        }

        public DataSet EmployeePOSearch(int companyId, string poNumber, string employeeId, string status, string pricingType)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            {
                using var command = new SqlCommand("[dbo].[USP_MAIN_PO_VIEW_EMPLOYEE]", connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@COMPANY_ID", companyId);
                    command.Parameters.AddWithValue("@PONumber", poNumber);
                    command.Parameters.AddWithValue("@ClientEmployeeID", employeeId);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@PricingType", pricingType);
                    command.CommandTimeout = 1500;

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public async Task<string> ExecuteGstInvoiceAsync(GstInvoiceCreateRequest request)
        {
            try
            {
                using var conn = Connection;

                var parameters = new DynamicParameters(request);
                parameters.Add("@Invoice_Id", request.Invoice_Id, DbType.Int32, ParameterDirection.InputOutput);

                conn.Open();

                var data = await conn.QueryAsync(
                    "Proc_ManageGstInvoice",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return JsonConvert.SerializeObject(new
                {
                    InvoiceId = parameters.Get<int?>("@Invoice_Id"),
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public DataSet GetDataSetsSecondaryAsync(int companyCode, int pay_period_id, int lot, int inputType)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_secondaryString);
            {
                using var command = new SqlCommand("InputAutomation_Custom_Report", connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Company_Id", companyCode);
                    command.Parameters.AddWithValue("@Pay_Period_Id", pay_period_id);
                    command.Parameters.AddWithValue("@InputLotNumber", lot);
                    command.Parameters.AddWithValue("@InputType", inputType);
                    command.CommandTimeout = 1500;
                    //if (param != null)
                    //{
                    //    foreach (var prop in param.GetType().GetProperties())
                    //    {
                    //        command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(param) ?? DBNull.Value);
                    //    }
                    //}

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }
        public DataSet GetDataSetsSecondaryAsync(int companyCode, int pay_period_id)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_secondaryString);
            {
                using var command = new SqlCommand("[sp_PayregisteruploadexporttoExcel]", connection);
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Company_Id", companyCode);
                    command.Parameters.AddWithValue("@Pay_Period_Id", pay_period_id);

                    command.CommandTimeout = 1500;
                    //if (param != null)
                    //{
                    //    foreach (var prop in param.GetType().GetProperties())
                    //    {
                    //        command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(param) ?? DBNull.Value);
                    //    }
                    //}

                    using var adapter = new SqlDataAdapter(command);
                    {
                        //   await Task.Run(() => adapter.Fill(ds));
                        adapter.Fill(ds);
                    }
                }
            }
            return ds;
        }


        public async Task<IEnumerable<T>> GetItemsSecondaryAsync<T>(string storeProcedureName, object param)
        {
            try
            {
                using (var dbConnection = ConnectionSecondary)
                {
                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync<T>(storeProcedureName, param, null, commandTimeout: 1500, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                throw new Exception("Database operation failed: " + ex.Message);
            }
        }

        public async Task<string> GetItemsSecondaryAsync(string storeProcedureName, object param)
        {
            try
            {
                using (var dbConnection = ConnectionSecondary)
                {

                    dbConnection.Open();
                    var result = await dbConnection.QueryAsync(storeProcedureName, param, null, commandTimeout: 1000, CommandType.StoredProcedure);
                    var obj = JsonConvert.SerializeObject(result);
                    return obj;
                }
            }
            catch (SqlException ex)
            {
                // Log exception details (You can use a logging library like Serilog or NLog)
                //Console.WriteLine($"SQL Exception: {ex.Message}");
                //throw ex; // Rethrow the exception or return a custom error

                return ex.Message;
                //    }
                //    catch (Exception ex)
                //    {
                //        return ex.Message;
                //        // Handle other exceptions
                //        //Console.WriteLine($"Exception: {ex.Message}");
                //        //throw;
            }
        }




    }
}
