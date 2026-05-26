using System.Data;
using QPay.UI.DebitNote;

namespace QPay.BAL.IRepository.DebitNote
{
    public interface IDebitNoteRepository
    {
        Task<DataSet> Search(
    string ClientName,
    string EmpCode,
    string FromDate,
    string ToDate
);

        Task<DataSet> DebitNoteExportToExcel(
            DebitNoteExport payload
        );
    }
}