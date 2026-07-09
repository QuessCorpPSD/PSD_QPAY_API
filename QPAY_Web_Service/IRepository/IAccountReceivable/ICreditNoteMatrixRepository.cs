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

        Task<List<UI.CreditNoteMatrix.ErrorMessage>> Create(CreditNoteMatrixRequest request);

        Task<List<UI.CreditNoteMatrix.ErrorMessage>> Update(CreditNoteMatrixRequest request);

        Task<List<UI.CreditNoteMatrix.ErrorMessage>> Delete(CreditNoteMatrixRequest request);

        DataSet ExportToExcel();
      
      List<CommonDropDown> GetCommonDropDownList(string Flag, int UserId);
    }
}
