using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.AccountReceivableMod.TDSSlabModels;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface ITDSSlab
    {
        Task<DataSet> GetFinancialYear(int? financialYearId);
        Task<DataSet> Search(int? CompanyId, int? FinancialYearId);
        Task<DataSet> ExportToExcel(CommonExport2 payload);
        Task<ClientAdvancePaymentResponse> UploadTDSSlab(IFormFile file, string createdBy);
        Task<UploadResponse> UploadLTDSSlab(IFormFile file, int userId);
        Task<List<TdsSlabResult>> TdsSlabCreate(string tdsDetails, string action, int userId);
        Task<List<CompanyNameByCodeResult>> GetCompanyNameByCode(string companyCode);
    }
}
    