using Microsoft.AspNetCore.Mvc;
using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface IClientAddressRespository
    {
        Task<List<ClientAddress>> GetAllClientAddressDetails(ClientAddressSearch searchparams);
        Task<string> PostAddClientAddress(AddressRequest addressRequest);
        Task<string> PostDeleteClientAddress(int ClientAddressId, int UserId);
        Task<ClientAddressResponse> PostClientAddressUpload(string xmlString, string flag, string userId);
        DataSet ClientAddressExport(int userId);
    }
}
