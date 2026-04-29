using Microsoft.AspNetCore.Http;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.SalaryReleaseInvoice
{
    public interface IReIssueApprove
    {
        // INTERFACE

        Task<DataSet> SearchReIssueApprove(
            int CompanyId,
            int PayPeriodId,
            string ReIssueTypes,
            string FromDate,
            string ToDate,
            int param,
            string Status);
        Task<DataSet> GetDropdown(string flag);
        Task<ReIssueApproveUploadResponse> ReissueProcessApproveBulkUpload(IFormFile file, string User);
        Task<DataSet> ExportToExcel(ReIssueApproveExportRequest payload);
        Task<ReIssueApproveRejectResponse>
       CreateReIssueApproveReject(
           ReIssueApproveRejectRequest request
       );
    }
}

