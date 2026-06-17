using QPay.UI.Models.MailApprovalProcess;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.MailApprovalProcess
{
    public interface ICustomerBlockApprovalRepository
    {
        #region CustomerBlockApproval start
        DataSet GetApproveClientList(string UserId);

        Task<List<ErrorMessage>> ClientApproveReject(ClientApprove payload);

        #endregion CustomerBlockApproval end
    }
}
