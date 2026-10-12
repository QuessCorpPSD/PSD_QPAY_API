using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.ARKnockOff
{
    public interface IARKnockOffRepository
    {
        Task<string> SaveARDetails(string xml);
        Task<DataSet> GetARInvoiceDetails();
        Task<DataSet> GetIgnoreSubjectLine();
        DataSet ARReportExport(string FromDate);
        Task<DataSet> GetAllIgnoreSubjectLine();
        Task<string> CreateUpdateDelete_IgnoreSubjectLine(string xml, string mode, int userId);
    }
}
