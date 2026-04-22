using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using QPay.BAL.IRepository.IAccountReceivable;
using QPay.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.AccountReceivableRepository
{
    public class InvoiceCollection : IInvoiceCollectionRepository
    {
        private readonly DbRepository _dbRepository;

        public InvoiceCollection(DbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public async Task<DataSet> GetMapName(int companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetMapNamesWithAllOption",
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetModeOfCollections(string action)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = action
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_CommonDropDowns", parameters, 1500);
        }
        public async Task<DataSet> SearchEditInvoiceCollection(int companyId, int payPeriodId, int invoiceCollectionId, string mode)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@Pay_Period_id"] = payPeriodId,
                ["@Invoice_Collection_Id"] = invoiceCollectionId,
                ["@Mode"] = mode
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "BankInvoiceCollectionEditSearch",
                parameters,
                1500
            );
        }
        public async Task<DataSet> ValidateInvoiceCollection(string collection, string collectiondetail, int createdby, string mode)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@invoiceCollection"] = collection ?? (object)DBNull.Value,
                ["@invoiceCollection_Detail"] = collectiondetail ?? (object)DBNull.Value,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "ValidateInvoiceCollection",
                parameters,
                1500
            );
        }
        public async Task<DataSet> CreateInvoiceCollection(string collection, string collectiondetail, int createdby, string mode)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@invoiceCollection"] = collection,
                ["@invoiceCollection_Detail"] = collectiondetail,
                ["@Createdby"] = createdby,
                ["@mode"] = mode
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "SaveInvoiceCollection",
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetTDSPercentage(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_ID"] = companyId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetAllTDSPercentageByCompanyId",
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetOnAccount(int? companyId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_ID"] = companyId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "GetClientadvancepaymentRefno",
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetCollectionInvoiceNo(int? companyId, int payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_ID"] = companyId,
                ["@Pay_Period_id"] = payPeriodId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "GetInvocecollectionInvoiceno",
                parameters,
                1500
            );
        }
        
        public async Task<DataSet> InvoiceCollectionBulkUpload(IFormFile file, string fileType, string user)
        {
            string spName = "";

            if (fileType == "Insert")
                spName = "Proc_Upload_InvoiceCollection";
            else if (fileType == "Disbursal")
                spName = "Proc_Upload_InvoiceDisbursal";
            else
                spName = "Proc_Upload_InvoiceCollectionDelete";

            // Save file
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(file.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Read Excel → DataTable
            DataTable dt = new DataTable();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                            dt.Columns.Add(cell.Value.ToString().Trim());

                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;

                        foreach (var cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString().Trim();
                            i++;
                        }
                    }
                }
            }

            // Convert to XML
            dt.TableName = "Table";
            DataSet dsXML = new DataSet("NewDataSet");
            dsXML.Tables.Add(dt);

            string xmlInput = "";
            using (var sw = new StringWriter())
            {
                dsXML.WriteXml(sw);
                xmlInput = sw.ToString();
            }

            var parameters = new Dictionary<string, object?>
            {
                ["@xml"] = xmlInput,
                ["@Createdby"] = user
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                spName,
                parameters,
                1500
            );
        }
        public async Task<DataSet> ExportInvoiceCollectionToExcel(int? companyId, int? payPeriodId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = companyId,
                ["@Pay_Period_id"] = payPeriodId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "BankInvoiceCollectionEditSearchExportToExcel",
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetReceivableAmount(int PayPeriodId, string InvoiceNumber, decimal TdsPercentage)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@PayPeriodId"] = PayPeriodId,      
                ["@InvoiceNumber"] = InvoiceNumber, 
                ["@TdsPercentage"] = TdsPercentage  
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_Bank_Invoice_GetReceivableAmount", 
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetInvoiceCollectionData(int CompanyId, int PayPeriodId, int MapNameId, int RefId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@CompanyId"] = CompanyId,
                ["@PayPeriodId"] = PayPeriodId,
                ["@MapNameId"] = MapNameId,
                ["@RefId"] = RefId
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "sp_GetInvoiceCollectionData", 
                parameters,
                1500
            );
        }
        public async Task<DataSet> GetCompanyNameByCode(string companyCode)
        {
            try
            {
                var parameters = new Dictionary<string, object?>
                {
                    ["@CompanyCode"] = companyCode   
                };

                return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                    "sp_GetClientNamesByClientCode", 
                    parameters,
                    1500
                );
            }
            catch (Exception ex)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("Error");

                dt.Columns.Add("ErrorMessage");
                dt.Rows.Add(ex.Message);

                ds.Tables.Add(dt);

                return ds;
            }
        }
        public async Task<DataSet> GetOnAccountReference(string referenceNumber)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@RefNo"] = referenceNumber
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "USP_Bank_Invoice_GetOnAccountReferenceAmount",
                parameters,
                1500
            );
        }
    }
}
