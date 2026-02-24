using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.SalaryReleaseInvoice
{
    public interface ISalaryReleasePendingApprovalRepository
    {
        DataSet BankAdviceList(string BatchType, string CollectionStatus, string UserId);

        DataSet BankAdviceListExport(string BatchType, string CollectionStatus, string UserId);

        Task<List<BankadviceApprovalMessage>> BankAdviceApprove(ApproveBankAdvice payload);
    }
}
