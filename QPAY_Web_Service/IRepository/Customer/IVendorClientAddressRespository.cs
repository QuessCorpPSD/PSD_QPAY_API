using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface IVendorClientAddressRespository
    {
        Task<List<VendorClientAddress>> GetAllVendorClientAddressDetails(VendorClientAddressSearch vendorsearchparams);
        Task<string> PostAddVendorClientAddress(VendorAddressRequest addressRequest);
        Task<string> PostDeleteVendorClientAddress(int ClientAddressId, int UserId);
        Task<VendorClientAddressResponse> PostVendorClientAddressUpload(string xmlString, string flag, string userId);
        DataSet VendorClientAddressExport(int userId);
    }
}
