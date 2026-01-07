using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.GlobalMaster.ESIClass;
using static QPay.UI.Models.GlobalMaster.PTClass;


namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IPTRepository
    {
        Task<List<PTTypeUI>> PTType();
        Task<List<PTCategoryUI>> PTCategory();
        Task<List<PTCircleUI>> PTCircle(int StateId);
        Task<DataSet> PTSearch(PTSearchRequest request);
        Task<DataSet> PTExporttoExcel(PTSearchRequest request);
        Task<PTResponse> CreateUpdateDeletePT(PTRequest request);
    }
}
