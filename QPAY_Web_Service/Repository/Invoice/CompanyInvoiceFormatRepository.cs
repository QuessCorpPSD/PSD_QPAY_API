using Dapper;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Models.Customer;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QPay.BAL.Repository.Invoice
{
    public class CompanyInvoiceFormatRepository: ICompanyInvoiceFormatRepository
    {
        private readonly DbRepository _dbRepository;

        public CompanyInvoiceFormatRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<List<CompanyInvoiceFormat>> GetAllCompanyInvoiceFormat(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Get");
            parameters.Add("@UserId", userId);
            parameters.Add("@PageNo", 1);
            parameters.Add("@PageSize", 999999);


            var res = await this._dbRepository.GetItemsAsync("Proc_ManageCompanyInvoiceFormat", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<CompanyInvoiceFormat>>(res) ?? new List<CompanyInvoiceFormat>();
            }

            return new List<CompanyInvoiceFormat>();
        }

        public async Task<List<InvoiceTypeModel>> GetAllInvoiceType()
        {
            var parameters = new DynamicParameters();
        
            var res = await this._dbRepository.GetItemsAsync("Proc_GetallInvoiceType", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InvoiceTypeModel>>(res) ?? new List<InvoiceTypeModel>();
            }

            return new List<InvoiceTypeModel>();
        }

        public async Task<List<InvoiceFormat>> GetAllInvoiceFormat()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("Proc_GetAllInvoiceFormat", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InvoiceFormat>>(res) ?? new List<InvoiceFormat>();
            }

            return new List<InvoiceFormat>();
        }
        public async Task<string> Create(InvoiceFormatAdd request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", request.mode);
            parameters.Add("@UserId", request.userId);
            parameters.Add("@Id", request.Id);
            parameters.Add("@CompanyId", request.CompanyId);
            parameters.Add("@GroupDetailId", request.GroupDetailId);
            parameters.Add("@InvoiceType_Id", request.InvoiceType_Id);
            parameters.Add("@InvoiceFormatId", request.InvoiceFormatId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageCompanyInvoiceFormat", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
    }
}
