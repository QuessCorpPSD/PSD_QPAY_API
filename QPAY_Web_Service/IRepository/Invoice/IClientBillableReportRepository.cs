using Microsoft.AspNetCore.Mvc;
using QPay.DAL.Repository;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.Invoice;

namespace QPay.BAL.IRepository.Invoice
{
    public interface IClientBillableReportRepository
    {
        Task<DataSet> Search(int? entityId, string? startDate, string? endDate);
    }
}
