using Microsoft.AspNetCore.Http;
using QPay.UI.Models.AccountReceivableMod;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface IAPARAdjustmentRepository
    {

        Task<DataSet> SearchAPARAdjustmentUpdate(int CompanyId, string fromdate, string todate);
        Task<DataSet> APARAdjustmentEmployeeSearch(string APARAdjustmentNo);
        Task<DataSet> APARAdjustmentExportToExcel(APARAdjustmentExport payload);
        Task<APARAdjustmentUploadResponse> UploadAPARAdjustmentCancel(IFormFile file, string User);

        Task<string> EditAPARAdjustment(APARAdjustmentEditRequest request);
        Task<APARAdjustmentUploadResponse> UploadAPARAdjustment(IFormFile file, string User);


    }
}
