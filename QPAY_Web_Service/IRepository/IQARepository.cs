using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QPay.UI.Models;

namespace QPay.BAL.IRepository
{
    public interface IQARepository
    {
        List<CustomerSOPQuestion> GetCustomerSOPQuestionAnswer();
    }
}
