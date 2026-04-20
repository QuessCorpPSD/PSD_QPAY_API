using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.IRepository.AccountReceivable
{
    public interface IClientAdvancePaymentRepository
    {

        Task<DataSet> Search(int? CompanyId, string FromDate, string ToDate);
        Task<DataSet> GetGroupNameByCompanyID(int? CompanyId);
        Task<DataSet> ExportToExcel(CommonExport payload);
        Task<DataSet> GetModeOfCollections(string Action);
        Task<DataSet> GetOnAccountNumbers(string Description, string Action);

        Task<DataSet> GetOnAccountTypes(string Action);
        Task<DataSet> GetBankNameForOnAccount();
        Task<ClientAdvancePaymentResponse> SaveUpdateDeleteClientAdvancePayment(ClientAdvancePaymentRequest request);
        Task<ClientAdvancePaymentResponse> UploadClientAdvancePayment(IFormFile file, string User);
        Task<ClientAdvancePaymentResponse> TransferClientAdvancePayment(ClientAdvancePaymentRequest request);
    }
}
