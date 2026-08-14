using QPay.UI.GlobalMaster;
using QPay.UI.Models.GlobalMaster;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface ICurrencyRepository
    {
        Task<DataSet> GetAllCurrency(string flag);
        Task<DataSet> CurrencyConversion(CurrencyConversionRequest request);
    }
}
