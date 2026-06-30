using System.Data;
using QPay.UI.Models.SalaryReleaseInvoice;

namespace QPay.BAL.IRepository.SalaryReleaseInvoice
{
    public interface IinvoiceBatchConsolidationRepository
    {
        Task<DataSet> GetBusinessUnitName();

        Task<DataSet> InvoiceBatchConsolidationExport(InvoiceBatchExport payload);

        Task<DataSet> SearchHTHBankTransferStatus(HTHBankTransferStatusDto request);

        Task<DataSet> ExportToExcelHTHBankTransferStatus(HTHBankTransferStatusDto request);
    }
}