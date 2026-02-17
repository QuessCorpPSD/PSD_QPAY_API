using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.SalaryReleaseInvoice
{
    public interface IBatchGenerationRepository
    {
        #region BatchGenerate start
        DataSet GetApproveInvoices(string BatchType, int BatchCreationType, int EntityId, int UserId);

        DataSet GetApproveInvoicesExport(string BatchType, int BatchCreationType, int EntityId, int UserId);

        Task<List<BulkUploadErrorMessage>> BatchGenerate(BatchCreate payload);

        Task<List<BulkUploadErrorMessage>> RejectBankAdvice(RejectBankAdvice payload);
        List<EntityMaster> EntityListbg(int UserId);
       
        List<CommonGenModel> BatchCreationTypelist(int UserId);

        #endregion BatchGenerate end

        #region Salaryreleaseprocess start
        List<BatchList> GetSRPBatchList(string BatchType, int UserId);

        DataSet GetSRPBatchData(string BatchType, string BatchId, int UserId);

        byte[] BatchIntitiate(string BatchType, string BatchId, int UserId);

        #endregion Salaryreleaseprocess end

        #region Download Batch start
        List<BatchList> GetBatchList(string BatchType, string BatchDate, int UserId);
        #endregion Download Batch end
    }
}
