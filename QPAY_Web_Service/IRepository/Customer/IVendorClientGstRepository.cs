using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface IVendorClientGstRepository
    {
        Task<List<VendorClientGSTGrid>> GetAllVendorClientGSTDetails(int userId);
        Task<string> PostAddVendorClientGST(VendorClientGSTRequest Request);
        Task<string> PostDeleteVendorClientGST(int VendorClientGSTId, int UserId);
        Task<VendorClientGSTResponse> PostVendorClientGSTUpload(string xmlString, string flag, string userId);
        DataSet VendorClientGSTExport(int userId);
    }
}
