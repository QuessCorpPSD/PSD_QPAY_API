using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.GlobalMaster;
using System.Data;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface ISiteMasterRepository
    {
        Task<DataSet> Search(int? companyId, int? groupId);
        Task<DataSet> GetQuessLegalEntity();

       // Task<DataSet> Create(EntityRequest request);

        Task<DataSet> ExporttoExcel(int? companyId, int? groupId);

        Task<List<PortalPayslipFormatUI>> GetPortalPayslipFormat();
        Task<SiteMasterResponse> CreateUpdateSiteMaster(CreateUpdateSitemasterRequest request);
        Task<SiteMasterResponse> UploadSiteMaster(IFormFile file, [FromForm] string User);

        //Task<DataSet> GetCriteria(int? CriteriaTypeId);
        //Task<List<CategoryUI>> GetCategory();
    }
}
