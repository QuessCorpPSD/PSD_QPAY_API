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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.ComponentModel.DataAnnotations;

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

            var res = await this._dbRepository.GetItemsAsync("sp_ManageSEZWOPRepository_NewUI", parameters);
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

        public async Task<List<SEZCertificate>> SearchSEZCertificate(int companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", companyId);
            parameters.Add("@Action", "Search");

            var res = await this._dbRepository.GetItemsAsync("sp_ManageSEZCertificate_Upload", parameters);
            if (res != null)
            {
                return JsonConvert.DeserializeObject<List<SEZCertificate>>(res) ?? new List<SEZCertificate>() { new SEZCertificate() };
            }
            else
            {
                return new List<SEZCertificate>() { new SEZCertificate() };
            }
        }
        public string GetUploadedCertificate(int Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            parameters.Add("@Action", "GetFilePath");

            var res = this._dbRepository.GetItemsAsync("sp_ManageSEZCertificate_Upload", parameters).Result;

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }


        public async Task<string> SaveUploadData(string companyId, string userId, string validFrom, string validTo, string remarks, string AckNo, string OriginalFileName
            , string FileName, string FilePath, string Action)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            parameters.Add("@CreatedBy", userId);
            parameters.Add("@UploadRemarks", remarks);
            parameters.Add("@AckNo", AckNo);
            parameters.Add("@ValidFrom", validFrom);
            parameters.Add("@ValidTo", validTo);
            parameters.Add("@OriginalFilename", OriginalFileName);
            parameters.Add("@FileName", FileName);
            parameters.Add("@FilePath", FilePath);
            parameters.Add("@Action", "SaveFilepath");

            var res = await this._dbRepository.GetItemsAsync("Proc_SEZCertificate_Upload", parameters);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }
    }
}
