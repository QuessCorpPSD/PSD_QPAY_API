using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.IRepository.BankNonInvoice
{
    public interface Ibankadvisesplitculturerepository
    {
        Task<DataSet> getvendor(string? filter, int Company_id);
        Task<DataSet> getgroupname(int? Company_id, int client_id);

        Task<DataSet> createbankadvisesplitculture(Bankadvisesplitculture payload);

        Task<DataSet> getsearcheditdata(searcheditdata payload);

        Task<DataSet> getsearcheditdataExport(searcheditdata payload);

        Task<BulkUploadErrormessage> uploadbankadvisesplitculture(IFormFile file, int created_by);

    }
}
