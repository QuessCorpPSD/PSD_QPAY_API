using QPay.UI.Models.GlobalMaster;
using System.Data;
using static QPay.UI.Models.GlobalMaster.GlobalMasters;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface IGstRepository
    {
        Task<DataSet> SearchDetails(string UserId);
        Task<DataSet> GetGSTtype();

        Task<DataSet> ExporttoExcel(string UserId);
        Task<GstMastersResponse> Create(GstRequest createRequest);
        Task<GlobalMastersResponse> Edit(GstRequest createRequest);
        Task<GlobalMastersResponse> Delete(int GstMasterId,int UserId);
    }
}
