using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Customer;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Billing.GenericUpload;
using static QPay.UI.Customer.Company;

namespace QPay.BAL.IRepository.Billing
{
    public interface IGenericUploadRepository
    {
        Task<DataSet> masters(int userId);
        Task<DataSet> GetGenericTemplate(string uploadType);
        Task<InvoiceResponse> PostGenericUpload(string xmlString, string userId, string uploadType);
    }
}
