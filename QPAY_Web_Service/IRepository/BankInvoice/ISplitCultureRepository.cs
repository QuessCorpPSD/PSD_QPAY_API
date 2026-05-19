using Microsoft.AspNetCore.Http;
using QZone.DTo.SplitCulture;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qzone.IRepository.SplitCulture
{
    public interface ISplitCultureRepository
    {
        Task<DataSet> SearchBankAdviceSplitCulture(SplitCultureSearchDto request);
        Task<DataSet> GetInvoiceBankCompanywiseMapname(int companyId);
        Task<SplitCultureResponse> CreateInvoiceBankCulture(BankCultureRequestDto request);
        Task<SplitCultureResponse> UploadBankInvoiceSplit(IFormFile file, int CreatedBy);

    }
}