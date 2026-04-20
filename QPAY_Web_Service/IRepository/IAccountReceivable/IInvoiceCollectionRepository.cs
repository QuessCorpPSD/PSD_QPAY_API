using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface IInvoiceCollectionRepository
    {
        Task<DataSet> GetMapName(int companyId);
        Task<DataSet> GetModeOfCollections(string action);
        Task<DataSet> SearchEditInvoiceCollection(int companyId, int payPeriodId, int invoiceCollectionId, string mode);
        Task<DataSet> ValidateInvoiceCollection(string collection, string collectiondetail, int createdby, string mode);
        Task<DataSet> CreateInvoiceCollection(string collection, string collectiondetail, int createdby, string mode);
        Task<DataSet> GetTDSPercentage(int? companyId);
        Task<DataSet> GetOnAccount(int? companyId);
        Task<DataSet> GetCollectionInvoiceNo(int? companyId, int payPeriodId);
        Task<DataSet> InvoiceCollectionBulkUpload(IFormFile file, string fileType, string user);
        Task<DataSet> ExportInvoiceCollectionToExcel(int? companyId, int? payPeriodId);
        Task<DataSet> GetReceivableAmount(int PayPeriodId, string InvoiceNumber, decimal TdsPercentage);
        Task<DataSet> GetInvoiceCollectionData(int CompanyId, int PayPeriodId, int MapNameId, int RefId);
        Task<DataSet> GetCompanyNameByCode(string companyCode);
        Task<DataSet> GetOnAccountReference(string referenceNumber);
    }
}
