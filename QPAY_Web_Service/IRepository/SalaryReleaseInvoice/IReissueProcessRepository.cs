using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.SalaryReleaseInvoice
{
    using Microsoft.AspNetCore.Http;
    using QPay.UI.GlobalMaster;
    using QPay.UI.Models.SalaryReleaseInvoice;
    using System.Data;
    using System.Threading.Tasks;

    public interface IReissueProcessRepository
    {
        Task<ReissueProcessReportResponse> ImportReissueProcess(
         IFormFile file,
         string createdBy
     );

        DataSet ReissueProcessReportExportToExcel(
            CommonExport payload
        );

        DataSet ReissueProcessSearch(
            string fromdate,
            string todate,
            string status
        );
    }
}
