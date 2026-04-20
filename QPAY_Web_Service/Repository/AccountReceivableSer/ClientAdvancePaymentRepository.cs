using ClosedXML.Excel;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.DAL.Repository;
using System.Data;
using System.Text;
using static QPay.UI_Domain.Models.AccountReceivable.ClientAdvancePayment;
    
public class ClientAdvancePaymentRepository : IClientAdvancePaymentRepository
{
    private readonly DbRepository _dbRepository;
    private readonly IConfiguration _configuration;

    public ClientAdvancePaymentRepository(DbRepository dbRepository, IConfiguration configuration)
    {
        this._dbRepository = dbRepository;
        this._configuration = configuration;
    }

    public async Task<DataSet> Search(int? CompanyId, string FromDate, string ToDate)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@Company_Id"] = CompanyId,
            ["@From_Date"] = FromDate,
            ["@To_Date"] = ToDate
        };

        return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "USP_Bank_Invoice_GetAllClientAdvancePaymentDetails", parameters, 1500);
    }

    public async Task<DataSet> ExportToExcel(CommonExport payload)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@Company_Id"] = Convert.ToInt32(payload.companyId),
            ["@From_Date"] = payload.fromDate,
            ["@To_Date"] = payload.toDate
        };

        return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "USP_Bank_Invoice_GetAllClientAdvancePaymentDetailsExportToExcel",
            parameters,
            1500
        );
    }
  
    
    public async Task<DataSet> GetModeOfCollections(string Action)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@Action"] = Action
        };

        return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "USP_CommonDropDowns", parameters, 1500);
    }

    public async Task<DataSet> GetOnAccountNumbers(string Description, string Action)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@Description"] = Description,
            ["@Action"] = Action
        };

        return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "USP_CommonDropDowns", parameters, 1500);
    }

    public async Task<DataSet> GetOnAccountTypes(string Action)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@Action"] = Action
        };

        return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "USP_CommonDropDowns", parameters, 1500);
    }

    public async Task<DataSet> GetBankNameForOnAccount()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@BankName"] = null,
            ["@Bank_Id"] = null
        };

        return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "sp_GetBankDetailsOnAccountScreen", parameters, 1500);
    }

    public async Task<DataSet> GetGroupNameByCompanyID(int? CompanyId)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@Company_Id"] = CompanyId
        };

        return _dbRepository.ExecuteStoredProcedureToDataSetAsync(
            "sp_GetGroupNameByCompanyID", parameters, 1500);
    }
    private string BuildClientAdvancePaymentXml(ClientAdvancePaymentRequest request)
    {
        var sb = new StringBuilder();
        sb.Append("<ClientAdvancepaymentModelResponse>");

        foreach (var item in request.clientadvancepayment)
        {
            sb.Append("<ClientAdvancePayment>");
            sb.AppendFormat("<Client_Advance_Payment_Id>{0}</Client_Advance_Payment_Id>", item.Client_Advance_Payment_Id);
            //sb.AppendFormat("<Reference_Id>{0}</Reference_Id>", item.Reference_Id ?? "");
            sb.AppendFormat("<Company_Id>{0}</Company_Id>", item.Company_Id);
            sb.AppendFormat("<UTRChequeNumber>{0}</UTRChequeNumber>", item.UTRChequeNumber);
            sb.AppendFormat("<Cheque_Date>{0}</Cheque_Date>", item.Cheque_Date?.ToString("dd/MM/yyyy"));
            sb.AppendFormat("<Credit_Date>{0}</Credit_Date>", item.Credit_Date?.ToString("dd/MM/yyyy"));
            sb.AppendFormat("<Bank_Id>{0}</Bank_Id>", item.Bank_Id);
            sb.AppendFormat("<Amount>{0}</Amount>", item.Amount);
            sb.AppendFormat("<Remarks>{0}</Remarks>", item.Remarks);
            sb.AppendFormat("<Posting_Date>{0}</Posting_Date>", item.Posting_Date?.ToString("dd/MM/yyyy"));
            sb.AppendFormat("<Client_Id>{0}</Client_Id>", item.Client_Id);
            sb.AppendFormat("<OnAccountTypeValue>{0}</OnAccountTypeValue>", item.OnAccountTypeValue);
            sb.AppendFormat("<ModeOfCollectionsValue>{0}</ModeOfCollectionsValue>", item.ModeOfCollectionsValue);
            sb.AppendFormat("<OnAccountNumbersValue>{0}</OnAccountNumbersValue>", item.OnAccountNumbersValue);
            sb.AppendFormat("<Group_Detail_Id>{0}</Group_Detail_Id>", item.Group_Detail_Id);
            sb.Append("</ClientAdvancePayment>");
        }

        sb.Append("</ClientAdvancepaymentModelResponse>");
        return sb.ToString();
    }
    public async Task<ClientAdvancePaymentResponse> SaveUpdateDeleteClientAdvancePayment(ClientAdvancePaymentRequest request)
    {
        ClientAdvancePaymentResponse response = new ClientAdvancePaymentResponse();

        if (request == null || request.clientadvancepayment == null || !request.clientadvancepayment.Any())
        {
            response.response = "Invalid request.";
            return response;
        }

        var xmlInput = BuildClientAdvancePaymentXml(request);

        string storeProcedure = "USP_Bank_Invoice_CreateUpdateClientAdvancePayment";

        var parameters = new DynamicParameters();
        parameters.Add("@xmlInput", xmlInput);
        parameters.Add("@CreatedBy", request.Created_By);
        parameters.Add("@Mode", request.Mode);

        var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

        if (!string.IsNullOrWhiteSpace(res))
        {
            try
            {
                if (res.Contains("Successfully") || res.Contains("successfully"))
                {
                    response.response = res;
                }
                else
                {
                    response.response = "Failed to " + request.Mode;
                    response.errors = res
                        ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList() ?? new List<string> { "Unknown error." };
                }
            }
            catch
            {
                response.response = "Error while processing response.";
            }
        }
        else
        {
            response.response = "Failed";
        }

        return response;
    }

    public async Task<ClientAdvancePaymentResponse> UploadClientAdvancePayment(IFormFile file, string User)
    {
        ClientAdvancePaymentResponse response = new ClientAdvancePaymentResponse();

        try
        {
            if (file == null || file.Length == 0)
            {
                response.response = "File not found";
                return response;
            }

            var DirName = Path.Combine(_configuration["ClaimDocPath"].ToString(), "ClientAdvancePayment");

            if (!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(DirName, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
             
            // Read Excel
            DataTable dt = ReadExcelToDataTable(filePath);

            if (dt.Rows.Count == 0)
            {
                response.response = "Excel sheet is empty.";
                return response;
            }

            // IMPORTANT: XML format must match Stored Procedure
            dt.TableName = "Table";
            DataSet ds = new DataSet("NewDataSet");
            ds.Tables.Add(dt);

            string xmlInput = "";
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                xmlInput = sw.ToString();
            }

            string storeProcedure = "SP_Client_Advance_Payment_Upload";

            var parameters = new DynamicParameters();
            parameters.Add("@xmlInput", xmlInput);
            parameters.Add("@CreatedBy", User);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (!string.IsNullOrWhiteSpace(res))
            {
                if (res.ToLower().Contains("success"))
                {
                    response.response = res;
                }
                else
                {
                    response.response = "Failed to import.";
                    response.errors = res
                        ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList() ?? new List<string> { "Unknown error." };
                }
            }
            else
            {
                response.response = "Failed";
            }
        }
        catch (Exception ex)
        {
            response.response = ex.Message;
        }

        return response;
    }
    private DataTable ReadExcelToDataTable(string filePath)
    {
        DataTable dt = new DataTable();

        using (var workbook = new XLWorkbook(filePath))
        {
            var worksheet = workbook.Worksheet(1);
            bool firstRow = true;

            foreach (var row in worksheet.RowsUsed())
            {
                if (firstRow)
                {
                    foreach (var cell in row.Cells())
                        dt.Columns.Add(cell.Value.ToString().Trim());

                    firstRow = false;
                }
                else
                {
                    dt.Rows.Add();
                    int i = 0;
                    foreach (var cell in row.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString().Trim();
                        i++;
                    }
                }
            }
        }

        return dt;
    }
    public async Task<ClientAdvancePaymentResponse> TransferClientAdvancePayment(ClientAdvancePaymentRequest request)
    {
        ClientAdvancePaymentResponse response = new ClientAdvancePaymentResponse();

        if (request == null || request.clientadvancepayment == null || !request.clientadvancepayment.Any())
        {
            response.response = "Invalid request.";
            return response;
        }

        var xmlInput = BuildClientAdvancePaymentXml(request);

        string storeProcedure = "USP_ClientAdvancePaymentAmountTransfer";

        var parameters = new DynamicParameters();
        parameters.Add("@xmlInput", xmlInput);
        parameters.Add("@CreatedBy", request.Created_By);

        var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

        if (!string.IsNullOrWhiteSpace(res))
        {
            try
            {
                response.response = res;

                /*
                if (res.Contains("Successful") || res.Contains("Successfully"))
                {
                    response.response = res;
                }
                else
                {
                    response.response = "Failed to Transfer";
                    response.errors = res
                        ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList() ?? new List<string> { "Unknown error." };
                }*/
            }
            catch
            {
                response.response = "Error while processing response.";
            }
        }
        else
        {
            response.response = "Failed to Transfer";
        }

        return response;
    }
}