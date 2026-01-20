using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.GlobalMaster.PFClass;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IPFRepository
    {
        Task<List<PFPayCodesUI>> PFPayCodes();
        Task<List<PFCapTypeUI>> PFCapType();
        Task<DataSet> PFSearch(string CapType);
        Task<DataSet> PFExporttoExcel(string CapType);
        Task<PFResponse> CreateUpdatePF(PFRequest request);
        Task<PFResponse> DeletePF(PFDeleteRequest request);
    }
}
