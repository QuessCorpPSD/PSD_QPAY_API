using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository.CreditNoteMatrix;
using QPay.DAL.Repository;
using QPay.UI.CreditNoteMatrix;
using QPay.UI.Models.SalaryReleaseInvoice;
using System.Data;
using System.Text;

namespace QPay.BAL.Repository.CreditNoteMatrix
{
    public class CreditNoteMatrixRepository : ICreditNoteMatrixRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public CreditNoteMatrixRepository(
            DbRepository dbRepository,
            IConfiguration configuration)
        {
            _dbRepository = dbRepository;
            _configuration = configuration;
        }

        public DataSet Search(string Action, string XmlFile, int? CreatedBy)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = Action,
                ["@XmlFile"] = XmlFile,
                ["@CreatedBy"] = CreatedBy
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "PROC_Manage_CreditNote_Matrix",
                parameters);
        }

        public async Task<List<ErrorMessage>> Create(CreditNoteMatrixRequest request)
        {
            const string storedProcedure = "[dbo].[PROC_Manage_CreditNote_Matrix]";

            var parameter = new DynamicParameters();

            var xmlBuilder = new StringBuilder();

            xmlBuilder.Append("<ROOT>");

            foreach (var item in request.requestdata)
            {
                xmlBuilder.Append("<ROW>");

                xmlBuilder.AppendFormat("<CRNCategory>{0}</CRNCategory>", item.CRNCategory);

                xmlBuilder.AppendFormat("<PPT>{0}</PPT>", item.PPT == true ? 1 : 0);
                xmlBuilder.AppendFormat("<PPTUserId>{0}</PPTUserId>", item.PPTUserId);
                xmlBuilder.AppendFormat("<PPTMailId>{0}</PPTMailId>", item.PPTMailId);

                xmlBuilder.AppendFormat("<ZM>{0}</ZM>", item.ZM == true ? 1 : 0);
                xmlBuilder.AppendFormat("<ZMUserId>{0}</ZMUserId>", item.ZMUserId);
                xmlBuilder.AppendFormat("<ZMMailId>{0}</ZMMailId>", item.ZMMailId);

                xmlBuilder.AppendFormat("<BillingHead>{0}</BillingHead>", item.BillingHead == true ? 1 : 0);
                xmlBuilder.AppendFormat("<BillingHeadUserId>{0}</BillingHeadUserId>", item.BillingHeadUserId);
                xmlBuilder.AppendFormat("<BillingHeadMailId>{0}</BillingHeadMailId>", item.BillingHeadMailId);

                xmlBuilder.AppendFormat("<BF>{0}</BF>", item.BF == true ? 1 : 0);
                xmlBuilder.AppendFormat("<BFUserId>{0}</BFUserId>", item.BFUserId);
                xmlBuilder.AppendFormat("<BFMailId>{0}</BFMailId>", item.BFMailId);

                xmlBuilder.AppendFormat("<COO>{0}</COO>", item.COO == true ? 1 : 0);
                xmlBuilder.AppendFormat("<COOUserId>{0}</COOUserId>", item.COOUserId);
                xmlBuilder.AppendFormat("<COOMailId>{0}</COOMailId>", item.COOMailId);

                xmlBuilder.AppendFormat("<CEO>{0}</CEO>", item.CEO == true ? 1 : 0);
                xmlBuilder.AppendFormat("<CEOUserId>{0}</CEOUserId>", item.CEOUserId);
                xmlBuilder.AppendFormat("<CEOMailId>{0}</CEOMailId>", item.CEOMailId);

                xmlBuilder.AppendFormat("<WCFO>{0}</WCFO>", item.WCFO == true ? 1 : 0);
                xmlBuilder.AppendFormat("<WCFOUserId>{0}</WCFOUserId>", item.WCFOUserId);
                xmlBuilder.AppendFormat("<WCFOMailId>{0}</WCFOMailId>", item.WCFOMailId);

                xmlBuilder.AppendFormat("<President>{0}</President>", item.President == true ? 1 : 0);
                xmlBuilder.AppendFormat("<PresidentUserId>{0}</PresidentUserId>", item.PresidentUserId);
                xmlBuilder.AppendFormat("<PresidentMailId>{0}</PresidentMailId>", item.PresidentMailId);

                xmlBuilder.Append("</ROW>");
            }

            xmlBuilder.Append("</ROOT>");

            string resultXml = xmlBuilder.ToString();

            parameter.Add("@Action", "CREATE");
            parameter.Add("@XmlFile", resultXml);
            parameter.Add("@CreatedBy", request.CreatedBy);

            var res = await _dbRepository.GetItemsAsync(
                storedProcedure,
                parameter);

            return JsonConvert.DeserializeObject<List<ErrorMessage>>(res);
        }

        public async Task<List<ErrorMessage>> Update(CreditNoteMatrixRequest request)
        {
            const string storedProcedure = "[dbo].[PROC_Manage_CreditNote_Matrix]";

            var parameter = new DynamicParameters();

            var xmlBuilder = new StringBuilder();

            xmlBuilder.Append("<ROOT>");

            foreach (var item in request.requestdata)
            {
                xmlBuilder.Append("<ROW>");

                xmlBuilder.AppendFormat("<SNo>{0}</SNo>", item.SNo);
                xmlBuilder.AppendFormat("<CRNCategory>{0}</CRNCategory>", item.CRNCategory);

                xmlBuilder.AppendFormat("<PPT>{0}</PPT>", item.PPT == true ? 1 : 0);
                xmlBuilder.AppendFormat("<PPTUserId>{0}</PPTUserId>", item.PPTUserId);
                xmlBuilder.AppendFormat("<PPTMailId>{0}</PPTMailId>", item.PPTMailId);

                xmlBuilder.AppendFormat("<ZM>{0}</ZM>", item.ZM == true ? 1 : 0);
                xmlBuilder.AppendFormat("<ZMUserId>{0}</ZMUserId>", item.ZMUserId);
                xmlBuilder.AppendFormat("<ZMMailId>{0}</ZMMailId>", item.ZMMailId);

                xmlBuilder.AppendFormat("<BillingHead>{0}</BillingHead>", item.BillingHead == true ? 1 : 0);
                xmlBuilder.AppendFormat("<BillingHeadUserId>{0}</BillingHeadUserId>", item.BillingHeadUserId);
                xmlBuilder.AppendFormat("<BillingHeadMailId>{0}</BillingHeadMailId>", item.BillingHeadMailId);

                xmlBuilder.AppendFormat("<BF>{0}</BF>", item.BF == true ? 1 : 0);
                xmlBuilder.AppendFormat("<BFUserId>{0}</BFUserId>", item.BFUserId);
                xmlBuilder.AppendFormat("<BFMailId>{0}</BFMailId>", item.BFMailId);

                xmlBuilder.AppendFormat("<COO>{0}</COO>", item.COO == true ? 1 : 0);
                xmlBuilder.AppendFormat("<COOUserId>{0}</COOUserId>", item.COOUserId);
                xmlBuilder.AppendFormat("<COOMailId>{0}</COOMailId>", item.COOMailId);

                xmlBuilder.AppendFormat("<CEO>{0}</CEO>", item.CEO == true ? 1 : 0);
                xmlBuilder.AppendFormat("<CEOUserId>{0}</CEOUserId>", item.CEOUserId);
                xmlBuilder.AppendFormat("<CEOMailId>{0}</CEOMailId>", item.CEOMailId);

                xmlBuilder.AppendFormat("<WCFO>{0}</WCFO>", item.WCFO == true ? 1 : 0);
                xmlBuilder.AppendFormat("<WCFOUserId>{0}</WCFOUserId>", item.WCFOUserId);
                xmlBuilder.AppendFormat("<WCFOMailId>{0}</WCFOMailId>", item.WCFOMailId);

                xmlBuilder.AppendFormat("<President>{0}</President>", item.President == true ? 1 : 0);
                xmlBuilder.AppendFormat("<PresidentUserId>{0}</PresidentUserId>", item.PresidentUserId);
                xmlBuilder.AppendFormat("<PresidentMailId>{0}</PresidentMailId>", item.PresidentMailId);

                xmlBuilder.Append("</ROW>");
            }

            xmlBuilder.Append("</ROOT>");

            string resultXml = xmlBuilder.ToString();

            parameter.Add("@Action", "UPDATE");
            parameter.Add("@XmlFile", resultXml);
            parameter.Add("@CreatedBy", request.CreatedBy);

            var res = await _dbRepository.GetItemsAsync(
                storedProcedure,
                parameter);

            return JsonConvert.DeserializeObject<List<ErrorMessage>>(res);
        }

        public async Task<List<ErrorMessage>> Delete(CreditNoteMatrixRequest request)
        {
            const string storedProcedure = "[dbo].[PROC_Manage_CreditNote_Matrix]";

            var parameter = new DynamicParameters();

            var xmlBuilder = new StringBuilder();

            xmlBuilder.Append("<ROOT>");

            foreach (var item in request.requestdata)
            {
                xmlBuilder.Append("<ROW>");
                xmlBuilder.AppendFormat("<SNo>{0}</SNo>", item.SNo);
                xmlBuilder.Append("</ROW>");
            }

            xmlBuilder.Append("</ROOT>");

            string resultXml = xmlBuilder.ToString();

            parameter.Add("@Action", "DELETE");
            parameter.Add("@XmlFile", resultXml);
            parameter.Add("@CreatedBy", request.CreatedBy);

            var res = await _dbRepository.GetItemsAsync(
                storedProcedure,
                parameter);

            return JsonConvert.DeserializeObject<List<ErrorMessage>>(res);
        }

        public DataSet ExportToExcel()
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "EXPORTTOEXCEL",
                ["@XmlFile"] = DBNull.Value,
                ["@CreatedBy"] = DBNull.Value
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
                "PROC_Manage_CreditNote_Matrix",
                parameters);
        }

        public List<CommonDropDown1> GetCommonDropDownList(string Flag, string UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", Flag);
            parameters.Add("@Createdby", UserId);

            var res = this._dbRepository.GetItemsAsync("sp_Get_salary_Related_Template", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<CommonDropDown1>>(res) ?? new List<CommonDropDown1>();
            }

            return new List<CommonDropDown1>();
        }

        public List<CommonDropDown> GetCommonDropDownList(string Flag, int UserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", Flag);
            parameters.Add("@CreatedBy", UserId);

            var res = this._dbRepository.GetItemsAsync("Proc_manage_batchtemplate", parameters).Result;
            if (res != "")
            {
                return JsonConvert.DeserializeObject<List<CommonDropDown>>(res) ?? new List<CommonDropDown>();
            }

            return new List<CommonDropDown>();
        }
    }
}