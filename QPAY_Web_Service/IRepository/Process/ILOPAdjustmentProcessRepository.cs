using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QPay.UI.Models.Process.AttendanceProcess;
using static QPay.UI.Models.Process.Process;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;

namespace QPay.BAL.IRepository.Process
{
    public interface ILOPAdjustmentProcessRepository
    {
        Task<DataSet> SearchDetails(SearchLOPRequest searchRequest);
        Task<DataSet> ExporttoExcel(ExporttoExcelxml exporttoExcelRequest);
        Task<DataSet> DeleteLOPAdjustment(string LOP_Adjustment_Id,string CreatedBy);
        Task<ProcessResponse> ImportLOPAdjustment(IFormFile file, [FromForm] string User);
        Task<PoResponse> BulkPOCreate(IFormFile file, [FromForm] string flag,
         [FromForm] string CreatedBy);
    }
}
