using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Process.Process;

namespace QPay.BAL.IRepository.Process
{
    public interface IReimbursementCalendarRepository
    {
        Task<DataSet> SearchDetails(SearchReimbursementRequest searchRequest);
    }
}
