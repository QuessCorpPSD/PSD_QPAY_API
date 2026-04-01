using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Invoice;
using QPay.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Invoice
{
    public class SezRepository : ISezRepository
    {
        private readonly DbRepository _dbRepository;
        //private readonly ILogger _logger;
        public SezRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
          //  this._logger = logger;
        }
        public async Task<List<SEZWOPRepositoryUI>> Search(int companyId, int payPeriodId, string InvoiceNumbers, int Year)
        {
           // this._logger.LogInformation("Search Request");
            List<SEZWOPRepositoryUI> sEZWOPRepositories = new List<SEZWOPRepositoryUI>(); 
            try
            {

                var parameter = new DynamicParameters();
                parameter.Add("@company_Id", companyId);
                parameter.Add("@payPeriod_Id", payPeriodId);
                parameter.Add("@InvoiceNumber", InvoiceNumbers);
                parameter.Add("@Year", Year);
                parameter.Add("@Action", "Search");
                string storeProcedure = "sp_ManageSEZWOPRepository_NewUI";
               // this._logger.LogInformation("Db search started..");
                var res=await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
               // this._logger.LogInformation("Db search completed..");
                if (res.Any())
                {
                    sEZWOPRepositories = JsonConvert.DeserializeObject<List<SEZWOPRepositoryUI>>(res)?? new List<SEZWOPRepositoryUI>();
                }

            }
            catch (Exception Ex)
            {
               // this._logger.LogInformation("search exception : " + Ex.StackTrace);
            }
            return sEZWOPRepositories;
        }
        public async Task<SEZWOPRepositoryUI> Uploadfile(string CancelledInvoiceRepositoryDetails, int UserId, string Action)
        {
            SEZWOPRepositoryUI objUploadEmployeeDocument = new SEZWOPRepositoryUI();
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@xmlInput", CancelledInvoiceRepositoryDetails);
                parameter.Add("@Action", UserId);
                parameter.Add("@CreatedBy", Action);
               
                string storeProcedure = "sp_ManageSEZWOPRepository_NewUI";
               // this._logger.LogInformation("Db search started..");
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameter);
              //  this._logger.LogInformation("Db search completed..");
                if (res.Any())
                {
                  var  sEZWOPRepositories = JsonConvert.DeserializeObject<List<SEZWOPRepositoryUI>>(res).FirstOrDefault();

                    objUploadEmployeeDocument.Error_Message = sEZWOPRepositories.Error_Message;
                }
                
            }
            catch (Exception ex)
            {
                objUploadEmployeeDocument.Error_Message = ex.StackTrace;
            }
            return objUploadEmployeeDocument;
        }

        public FileResponse ExportToExcel(int? companyId, int payPeriodId, string InvoiceNumbers, int? Year)
        {
            FileResponse fileResponse = new FileResponse();
            DataTable ds = new DataTable();
            try
            {
                var parameter = new DynamicParameters();
                parameter.Add("@company_Id", companyId);
                parameter.Add("@payPeriod_Id", payPeriodId);
                parameter.Add("@InvoiceNumber", InvoiceNumbers);
                parameter.Add("@Year", Year);
                string storeProcedure = "sp_GetSEZWOPRepositoryExportToExcel_NewUI";
                var res =  this._dbRepository.GetItemsAsync(storeProcedure, parameter).Result;
                //this._logger.LogInformation("Db search completed..");
                if (res.Any())
                {
                    ds = JsonConvert.DeserializeObject<DataTable>(res) ?? new DataTable();
                    if (ds.Rows.Count > 0)
                    {
                        using var workbook = new XLWorkbook();
                        {
                            var ws = workbook.AddWorksheet(ds, "Sez Repository");
                            using (MemoryStream stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                stream.Seek(0, SeekOrigin.Begin);
                                var bytes = Convert.ToBase64String(stream.ToArray());
                                
                                fileResponse.FileName = "Sez";
                                fileResponse.File = bytes;

                            }

                        }
                    }
                    else
                    {
                        fileResponse.FileName = "Other Income.xlsx";
                        fileResponse.File = "N";
                    }
                }

            }
            catch (Exception ex)
            {
               // throw ex;
            }
            return fileResponse;
          
        }
    }
}
