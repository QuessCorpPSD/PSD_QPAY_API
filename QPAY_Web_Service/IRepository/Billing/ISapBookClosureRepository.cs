using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QPay.UI.Billing;
using QPay.UI.Customer;
using QPay.UI.Models;
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
    public interface ISapBookClosureRepository
    {
        Task<DataSet> GetMonths();
        Task<DataSet> GetBusinessUnitNames();
        Task<DataSet> Search();
        Task<DataSet> Create(SapBookClosureRequest request);

    }
}
