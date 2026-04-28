using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.IRepository.IAccountReceivable
{
    public interface IUnpaidinvoice
    {
        Task<DataSet> GetEntity(string flag);
        Task<DataSet> UnpaidInvoiceExportToExcel(CommonExport1 payload);
    }
}
