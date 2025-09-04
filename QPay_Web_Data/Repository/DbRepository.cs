using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.UI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.DAL.Repository
{
    public class DbRepository
    {
        //DbRepository<T> 
        private readonly string _connectionString;
        private readonly string _connectionReconString;

        public DbRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            _connectionReconString = configuration.GetConnectionString("ReConDBConnection")??"";
        }
        private IDbConnection Connection => new SqlConnection(_connectionString);
        private IDbConnection ConnectionRecon => new SqlConnection(_connectionReconString);

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
        public DataSet GetDataSetsAsync(int companyCode, int pay_period_id)
        {
            var ds = new DataSet();
            using var connection = new SqlConnection(_connectionString);
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

    }
}
