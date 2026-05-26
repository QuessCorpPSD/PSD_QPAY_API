using Microsoft.AspNetCore.Http;
using QPay.UI.Models.BankNonInvoice;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.IBankNonInvoice
{
    public interface IBankAdviceSplitCultureRepository
    {
        Task<DataSet> GetVendorname(string filter, int Company_id);
        Task<DataSet> GetSearchEditdata(int Company_id, int Vendor_id, int Bank_Culture_Id, string Mode);
        Task<BankAdviceSplitCultureUploadResponse>
BankSplitCultureupload(
    IFormFile file,
    int CreatedBy);
        Task<BankCultureResponse>
 CreateBankCulture(
     CreateBankCultureRequest request);

        Task<DataSet> Getgroupname(int Company_id, int Client_id);
    }

}
