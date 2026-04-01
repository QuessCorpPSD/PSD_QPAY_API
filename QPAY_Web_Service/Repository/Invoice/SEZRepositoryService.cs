using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QPay.DAL.Repository;
using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;

namespace QPay.BAL.Repository.Invoice
{
    public class SEZRepositoryService: ISEZRepositoryService
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;
        public SEZRepositoryService(DbRepository dbRepository, IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        public async Task<List<SEZRepository>> Search(int companyId, int payPeriodId, string? InvoiceNumbers, int Year)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@company_Id", companyId);
            parameters.Add("@payPeriod_Id", payPeriodId);
            parameters.Add("@InvoiceNumber", InvoiceNumbers);
            parameters.Add("@Year", Year);
            parameters.Add("@Action", "Search");

            var res = await this._dbRepository.GetItemsAsync("sp_ManageSEZWOPRepository", parameters);
            if (res != null)
            {
                return JsonConvert.DeserializeObject<List<SEZRepository>>(res) ?? new List<SEZRepository>() { new SEZRepository() };
            }
            else
            {
                return new List<SEZRepository>() { new SEZRepository() };
            }
        }

        public string GetSEZFilename(int invoice_Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Invoice_Ids", invoice_Id);
            parameters.Add("@Action", "GetFilePath");

            var res = this._dbRepository.GetItemsAsync("Proc_SEZRepository_UploadDoc", parameters).Result;

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> BulkApproveSEZ(ApproveRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Ids", request.Invoice_Id);
            parameters.Add("@QZoneUserId", request.UserId);
            parameters.Add("@UploadRemarks", request.Remarks);
            parameters.Add("@Action", request.Action);

            var res = this._dbRepository.GetItemsAsync("Proc_SEZRepository_Approve_Reject", parameters).Result;

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
    }
}
