using Microsoft.AspNetCore.Http;
using QPay.UI.Models.AccountReceivableMod;
using System.Data;
using System.Threading.Tasks;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;

namespace QPay.BAL.IRepository.AccountReceivable
{
    public interface IForecastRepository
    {
        Task<DataSet> Search(int? CompanyId, string PayPeriod, string Mode);
        Task<DataSet> ExportToExcel(ForecastExport payload);
        Task<DataSet> GetSBU();
        Task<DataSet> GetRegion();

        Task<DataSet> GetInvoiceNumber(int? CompanyId, int? PayPeriodId);
        Task<ForecastResponse> SaveUpdateDeleteForecast(ForecastRequest request);
        Task<ForecastResponse> UploadForecast(IFormFile file, string User);
    }
}