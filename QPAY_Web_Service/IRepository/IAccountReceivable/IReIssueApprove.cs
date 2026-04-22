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
    public interface IReIssueApprove
    {
        Task<DataSet> SearchReIssueApprove(int CompanyId,int PayPeriodId,string ReIssueTypes,int? PaytypeId,string vPayperiods,string Status);
        Task<DataSet> GetDropdown(string flag);
        Task<ReIssueApproveUploadResponse> ReissueProcessApproveBulkUpload(IFormFile file, string User);
        Task<DataSet> ExportToExcel(ReIssueApproveExportRequest payload);
        Task<ReIssueApproveRejectResponse> CreateReIssueApproveReject(ReIssueApproveRejectRequest request);
    }
}
