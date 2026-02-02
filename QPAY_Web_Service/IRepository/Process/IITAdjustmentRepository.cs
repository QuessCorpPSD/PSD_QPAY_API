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

namespace QPay.BAL.IRepository.Process
{
    public interface IITAdjustmentRepository
    {
        Task<DataSet> SearchDetails(SearchItRequest searchRequest);
        Task<ProcessResponse> ImportITAdjustment(IFormFile file, [FromForm] string User);
        Task<DataSet> DeleteITAdjustment(string IT_Adjustment_Id, string CreatedBy);
    }
}
