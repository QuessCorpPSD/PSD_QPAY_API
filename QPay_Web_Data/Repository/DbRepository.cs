using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.UI.Common;
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
            _connectionReconString = configuration.GetConnectionString("ReConDBConnection") ?? "";
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
                    var result = await dbConnection.QueryAsync<T>(storeProcedureName, param,null,commandTimeout: 1500, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                throw new Exception("Database operation failed: " + ex.Message);
            }
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


        //public async Task<IEnumerable<T>> GerListOfITemItemAsync(T model, string tableNamewithFilter)
        //{
        //    try
        //    {
        //        using (var dbConnection = Connection)
        //        {
        //            dbConnection.Open();
        //            var result = await dbConnection.QueryMultipleAsync<IEnumerable<T>>(tableNamewithFilter, model, null, null, CommandType.StoredProcedure);
        //            return result;
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        // Log exception details
        //        Console.WriteLine($"SQL Exception: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle other exceptions
        //        Console.WriteLine($"Exception: {ex.Message}");
        //        throw;
        //    }
        //}

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

    }
}
