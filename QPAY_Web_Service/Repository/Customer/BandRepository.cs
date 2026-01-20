using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using QPay.UI.Models.Customer;
using System.Text;

namespace QPay.BAL.Repository.Customer
{
    public class BandRepository : IBandRepository
    {
        private readonly DbRepository _dbRepository;

        public BandRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }
        public async Task<List<Band>> GetAllBandDetails(string companyId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Companycode", companyId);

            var res = await this._dbRepository.GetItemsAsync("sp_GetBandDetails", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return JsonConvert.DeserializeObject<List<Band>>(res) ?? new List<Band>();
            }

            return new List<Band>();
        }
        public async Task<BandResponse> SaveUpdateDeleteBand([FromBody] BandRequest request)
        {
            BandResponse bandresponse = new BandResponse();

            if (request == null || request.Bandmaster == null || !request.Bandmaster.Any())
            {
                bandresponse.response = "Invalid request.";
            }

            var xmlInput = BuildBandXml(request);

            string storeProcedure = "sp_CreateUpdateBand";
            var parameters = new DynamicParameters();

            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@CreatedBy", request.Created_By);
            parameters.Add("@mode", request.Mode);

            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);
            string? msg = null;
            if (!string.IsNullOrWhiteSpace(res))
            {
                var arr = JArray.Parse(res);
                msg = arr[0]?["Error_Message"]?.ToString();
            }
            if (!string.IsNullOrWhiteSpace(msg))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(msg) && (msg.Contains("Band Created Successfully", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Band Updated Successfully.", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("Band Deleted Successfully.", StringComparison.OrdinalIgnoreCase)))
                    {
                        bandresponse.response = msg;
                    }
                    else
                    {
                        bandresponse.response = "Failed to " + request.Mode + ".";
                        bandresponse.errors = msg
                            ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToList() ?? new List<string> { "Unknown error." };
                    }
                }
                catch
                {
                    bandresponse.response = "Error while processing response.";
                }
            }
            else
            {
                bandresponse.response = "Failed";
            }

            return bandresponse;
        }

        private string BuildBandXml(BandRequest request)
        {
            var sb = new StringBuilder();
            sb.Append("<BandDetails>");

            foreach (var row in request.Bandmaster)
            {
                sb.Append("<Band>");
                sb.AppendFormat("<Band_Id>{0}</Band_Id>", row.Band_Id);
                sb.AppendFormat("<Band_Code>{0}</Band_Code>", row.Band_Code);
                sb.AppendFormat("<Band_Name>{0}</Band_Name>", row.Band_Name);
                sb.AppendFormat("<Company_Id>{0}</Company_Id>", row.Company_Id);
                sb.AppendFormat("<Serial_No>{0}</Serial_No>", row.Serial_No);
                sb.Append("</Band>");
            }

            sb.Append("</BandDetails>");
            return sb.ToString();
        }
    }
}
