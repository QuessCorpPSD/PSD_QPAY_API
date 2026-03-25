using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Common.Invoices;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Invoice
{
    public class SEZWOPRepositoryService : ISEZWOPRepositoryService
    {
        private readonly DbRepository _dbRepository;

        public SEZWOPRepositoryService(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<List<SEZWOPRepository>> SearchAsync(int companyId, int payPeriodId, string invoiceNumbers, int year)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@company_Id", companyId);
            parameters.Add("@payPeriod_Id", payPeriodId);
            parameters.Add("@InvoiceNumber", invoiceNumbers);
            parameters.Add("@Year", year);
            parameters.Add("@Action", "Search");

            var res = await this._dbRepository.GetItemsAsync("sp_ManageSEZWOPRepository", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<SEZWOPRepository>>(res) ?? new List<SEZWOPRepository>();
            }

            return new List<SEZWOPRepository>();
        }

        public async Task<SEZWOPRepository> UploadfileAsync(string cancelledInvoiceRepositoryDetails, int userId, string action)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", cancelledInvoiceRepositoryDetails);
            parameters.Add("@Action", action);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("sp_ManageSEZWOPRepository", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<SEZWOPRepository>(res) ?? new SEZWOPRepository();
            }

            return new SEZWOPRepository();
        }

        public async Task<DataSet> ExportToExcelAsync(int? companyId, string statusId, string invoiceNumbers, int? year)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@PayPeriodId"] = statusId,
                ["@InvoiceNumber"] = invoiceNumbers,
                ["@Year"] = year,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetSEZWOPRepositoryExportToExcel", parameters, 1500);


        }

        //public async Task<List<DocumentTypeMaster>> GetDocumentTypeMasterAsync()
        //{
        //    var result = new List<DocumentTypeMaster>();
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            await connection.OpenAsync();
        //            using (var cmd = new SqlCommand("select * from Tbl_Document_Repository_Master where isactive=1", connection))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await reader.ReadAsync())
        //                    {
        //                        var item = new DocumentTypeMaster
        //                        {
        //                            Document_Type_Id = Convert.ToInt32(reader["Document_Type_Id"]),
        //                            Document_Type = Convert.ToString(reader["Document_Type"])
        //                        };
        //                        result.Add(item);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return result;

    }
}

