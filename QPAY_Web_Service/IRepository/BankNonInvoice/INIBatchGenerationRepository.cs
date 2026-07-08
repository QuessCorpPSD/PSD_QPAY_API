using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.BankNonInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.BankNonInvoice
{
    public interface INIBatchGenerationRepository
    {
        #region BatchTypeLoad start
        List<CommonDropDown> GetBatchTypeList(int UserId);
        DataSet GetTemplate(string Flag, int UserId);

        #endregion BatchTypeLoad end

        #region BatchGenerate start
        DataSet GetSalaryreleaseProcessdata(string BatchType, int EntityId, int BatchCreationType, int Status, int UserId);

        DataSet GetSalaryreleaseProcessExport(string BatchType, int EntityId, int BatchCreationType, int Status, int UserId);

        Task<List<BulkUploadErrorMessage>> BatchGenerate(NIBatchGenerate payload);

        Task<List<BulkUploadErrorMessage>> Rejectgroup(string BatchType, int Salary_Process_Initiate_detail_Id, int UserId);
        List<EntityMasterNI> EntityListbg(int UserId);

        List<CommonGenModel> BatchCreationTypelist(int UserId);

        Task<List<SatausErrorMessage>> UploadCollectionStatus(IFormFile file, [FromForm] string BatchType, [FromForm] int UserId);
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
