using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.BankNonInvoice.EmployeeSalaryRelease;

namespace QPay.BAL.IRepository.BankNonInvoice
{
    public interface IBankNEFTcultureNonInvoice
    {
        Task<DataSet> Getbankname(int? Company_id, string mode);
        Task<DataSet> GetSearchdata(int Company_id, int Bank_Culture_Id, string Mode);
        Task<List<BankCultureMessage>> NeftCultureSave(BankCulturesave payload);

        Task<DataSet> Getpayperiod();

        Task<DataSet> ExportToExcel(string payperiod);
    }
}
