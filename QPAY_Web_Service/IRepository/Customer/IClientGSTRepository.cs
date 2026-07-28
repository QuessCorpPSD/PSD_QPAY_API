using QPay.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.IRepository.Customer
{
    public interface IClientGSTRepository
    {
        Task<List<ClientGSTGrid>> GetAllClientGSTDetails(ClientGSTSearch searchparams);
        Task<string> PostAddClientGST(ClientGSTRequest Request);
        Task<string> PostDeleteClientGST(int ClientGSTId, int UserId);
        Task<ClientGSTResponse> PostClientGSTUpload(string xmlString, string flag, string userId);
        DataSet ClientGSTExport(int userId);
    }
}
