using QPay.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Common
{
    public interface IAccesstypeRepository
    {
        Task<List<AccessTypeUI>> GetAllAccessType();
    }
}
