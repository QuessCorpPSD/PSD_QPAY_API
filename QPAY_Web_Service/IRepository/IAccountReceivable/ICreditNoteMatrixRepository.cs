using QPay.UI.CreditNoteMatrix;
using QPay.UI.Models.SalaryReleaseInvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.CreditNoteMatrix
{
    public interface ICreditNoteMatrixRepository
    {
        DataSet Search(string Action, string XmlFile, int? CreatedBy);

        Task<List<ErrorMessage>> Create(CreditNoteMatrixRequest request);

        Task<List<ErrorMessage>> Update(CreditNoteMatrixRequest request);

        Task<List<ErrorMessage>> Delete(CreditNoteMatrixRequest request);

        DataSet ExportToExcel();
      
      List<CommonDropDown> GetCommonDropDownList(string Flag, int UserId);
    }
}
