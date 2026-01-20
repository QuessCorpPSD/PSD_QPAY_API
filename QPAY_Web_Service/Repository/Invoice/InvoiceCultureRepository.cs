using Dapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.GlobalMaster;
using QPay.UI.Invoice;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Models.Invoice.InvoiceCulture;

namespace QPay.BAL.Repository.Invoice
{
    public class InvoiceCultureRepository: IInvoiceCultureRepository
    {
        private readonly DbRepository _dbRepository;

        public InvoiceCultureRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<ServiceChargeMastereDD>> GetAllServiceChargeMaster()
        {
            var parameters = new DynamicParameters();

            var res = await this._dbRepository.GetItemsAsync("GetAllService_Charge_Master", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<ServiceChargeMastereDD>>(res) ?? new List<ServiceChargeMastereDD>();
            }

            return new List<ServiceChargeMastereDD>();
        }
        public async Task<List<InvoiceTypeforCultureDD>> GetAllInvoiceType()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "InvoiceCulture");

            var res = await this._dbRepository.GetItemsAsync("GetAllInvoiceTypeForInvoiceStructure", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InvoiceTypeforCultureDD>>(res) ?? new List<InvoiceTypeforCultureDD>();
            }

            return new List<InvoiceTypeforCultureDD>();
        }

        public async Task<List<GenDD>> GetAllInvoiceCategories()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetInvoiceCategory");

            var res = await this._dbRepository.GetItemsAsync("USP_CommonDropDowns", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<GenDD>>(res) ?? new List<GenDD>();
            }

            return new List<GenDD>();
        }

        public async Task<DataSet> GetMapNameByService(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Company_Id"] = companyId,
                ["@Service_Charge_Master_Id"] = 0,
                ["@Service_Charge_Type_Id"] = 0,
                ["@Service_Charge_Slab_Item_Id"] = 0,
                ["@Service_Charge_Slab_Inner_Item_Id"] = 0,
            };

            return this._dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllMapNameDetailByServiceCharge", parameters);

        }

        public async Task<DataSet> GetAllPayCodeFromCompany(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@CompanyId"] = companyId,
            };
            return this._dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllPayCodeFromCompanyPayCodeMapping", parameters);
        }

        public async Task<DataSet> GetAllPayCodeFromCompanyOI(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@CompanyId"] = companyId,
            };
            return this._dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetAllPayCodeFromCompanyOtherincomePayCode", parameters);
        }

        public async Task<List<InvoiceStructure>> GetAllInvoiceCulture(int companyId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@CompanyCode"] = companyId,
            };
                
            var res = await this._dbRepository.GetItemsAsync("sp_GetAllInvoiceStructure", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<InvoiceStructure>>(res) ?? new List<InvoiceStructure>();
            }

            return new List<InvoiceStructure>();
        }


        public async Task<DataSet> Create(string xml, int createdBy, string mode, string invoiceType)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = xml,
                ["@mode"] = mode,
                ["@CreatedBy"] = createdBy,
                ["@InvoiceType"] = invoiceType
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Sp_CreateUpdateInvoiceCultureAndStructureForMultipleMapName", parameters);
        }

        public async Task<string> PostInvoiceCulture(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@XML_File", xmlString);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_InvoiceStructure_Upload", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public DataSet InvoiceCultureExport(int companyId)
        {
            DataSet ds = this._dbRepository.InvoiceCultureExport(companyId);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                throw new Exception("No data found for the given Parameters.");
            }

        }
    }
}
