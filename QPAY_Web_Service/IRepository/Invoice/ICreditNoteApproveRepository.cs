using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Invoice
{
    public interface ICreditNoteApproveRepository
    {
        Task<List<CreditNote>> GetCreditNoteSearch(CreditNoteSearchApprove creditNoteSearchApprove);
        Task<string> UploadCreditNote(string xmlString, string userId);
    }
}
