using Azure.Core;
using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Invoice;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Invoice.Invoice;

namespace QPay.BAL.Repository.Invoice
{
    public class GSTInvoiceRepository : IGSTInvoiceRepository
    {
        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _config;

        public GSTInvoiceRepository(DbRepository dbRepository, IConfiguration config)
        {
            this._dbRepository = dbRepository;
            this._config = config;
        }

        public async Task<List<GstInvoiceGrid>> GetGSTInvoice(int userId)
        {

            string storeProcedure = "[dbo].[Proc_ManageGstInvoice_newUI]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "Get");
            parameter.Add("@UserId", userId);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<GstInvoiceGrid>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<GstInvoiceGrid>>(res);
                return list?.ToList() ?? new List<GstInvoiceGrid>();
            }
            catch (JsonException ex)
            {
                return new List<GstInvoiceGrid>();
            }


        }
        public DataSet GetInvoiceData(int invoiceId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = "GetInvoiceHtml",
                ["@Company_Id"] = 0,
                ["@InvoiceId"] = invoiceId,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetInvoiceDetails", parameters, 1500);
        }

        public async Task<string> PostCancelReject(string xmlString, string userId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Cancelled");
            parameters.Add("@Status", "Cancelled");
            parameters.Add("@XmlData", xmlString);
            parameters.Add("@UserId", userId);
            parameters.Add("@CreatedBy", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice_newUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> Create(GstInvoiceCreateRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", request.Action);
            parameters.Add("@Created_Mode", request.Created_Mode);
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Invoice_Id", request.Invoice_Id, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@Invoice_Number", request.Invoice_Number);
            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@Cost_Center_Mapping_Id", request.Cost_Center_Mapping_Id);
            parameters.Add("@City_Id", request.City_Id);
            parameters.Add("@Financial_Year_Id", request.Financial_Year_Id);
            parameters.Add("@Pay_Period_Id", request.Pay_Period_Id);
            parameters.Add("@Invoice_Type_Id", request.Invoice_Type_Id);
            parameters.Add("@Invoice_Date", request.Invoice_Date);
            parameters.Add("@Invoice_Due_Date", request.Invoice_Due_Date);
            parameters.Add("@Particulars", request.Particulars);
            parameters.Add("@Amount", request.Amount);
            parameters.Add("@StateId", request.StateId);
            parameters.Add("@InvoicingStateId", request.InvoicingStateId);
            parameters.Add("@CGST_Percentage", request.CGST_Percentage);
            parameters.Add("@SGST_Percentage", request.SGST_Percentage);
            parameters.Add("@UTGST_Percentage", request.UTGST_Percentage);
            parameters.Add("@IGST_Percentage", request.IGST_Percentage);
            parameters.Add("@Client_PO", request.Client_PO);
            parameters.Add("@Purchase_Order_Id", request.Purchase_Order_Id);
            parameters.Add("@Input_Date", request.Input_Date);
            parameters.Add("@Output_Date", request.Output_Date);
            parameters.Add("@Service_Charge", request.Service_Charge);
            parameters.Add("@Service_Charge_Amount", request.Service_Charge_Amount);
            parameters.Add("@Sourcing_Fee", request.Sourcing_Fee);
            parameters.Add("@Sourcing_Fee_Amount", request.Sourcing_Fee_Amount);
            parameters.Add("@No_Of_Employees", request.No_Of_Employees);
            parameters.Add("@Absorption_Fee", request.Absorption_Fee);
            parameters.Add("@Absorption_Amt", request.Absorption_Amt);
            parameters.Add("@CTC_Amt_Adjusted", request.CTC_Amt_Adjusted);
            parameters.Add("@CTC_Amt_NorP", request.CTC_Amt_NorP);
            parameters.Add("@CTC_Adj_Note", request.CTC_Adj_Note);
            parameters.Add("@Net_Amt_Adjusted", request.Net_Amt_Adjusted);
            parameters.Add("@Net_Amt_NorP", request.Net_Amt_NorP);
            parameters.Add("@Net_Adj_Note", request.Net_Adj_Note);
            parameters.Add("@Invoice_Culture_Id", request.Invoice_Culture_Id);
            parameters.Add("@Invoice_Culture_RefNo", request.Invoice_Culture_RefNo);
            parameters.Add("@Input_No", request.Input_No);
            parameters.Add("@Employee_ESI", request.Employee_ESI);
            parameters.Add("@Employer_ESI", request.Employer_ESI);
            parameters.Add("@Employee_PF", request.Employee_PF);
            parameters.Add("@Employer_PF", request.Employer_PF);
            parameters.Add("@Mobile_Recovery_Amount", request.Mobile_Recovery_Amount);
            parameters.Add("@Personal_Loan_Amount", request.Personal_Loan_Amount);
            parameters.Add("@Other_Deduction_Amount", request.Other_Deduction_Amount);
            parameters.Add("@WO_Number", request.WO_Number);
            parameters.Add("@Pl_Id_No", request.Pl_Id_No);
            parameters.Add("@Employee_Name", request.Employee_Name);
            parameters.Add("@Markup", request.Markup);
            parameters.Add("@Gri_Msp", request.Gri_Msp);
            parameters.Add("@DO_Number", request.DO_Number);
            parameters.Add("@Remarks", request.Remarks);
            parameters.Add("@Status", request.Status);
            parameters.Add("@IsActive", request.IsActive);
            parameters.Add("@WO_Date", request.WO_Date);
            parameters.Add("@InvoiceNotes", request.InvoiceNotes);
            parameters.Add("@CreatedBy", request.CreatedBy);
            parameters.Add("@CreatedOn", request.CreatedOn);
            parameters.Add("@ModifiedBy", request.ModifiedBy);
            parameters.Add("@ModifiedOn", request.ModifiedOn);
            parameters.Add("@Discrepancy_By", request.Discrepancy_By);
            parameters.Add("@Discrepancy_Reason", request.Discrepancy_Reason);
            parameters.Add("@Onboarding_Charge", request.Onboarding_Charge);
            parameters.Add("@Group_Detail_Id", request.Group_Detail_Id);
            parameters.Add("@TaxableAmount1", request.TaxableAmount1);
            parameters.Add("@TaxableAmount1_Note", request.TaxableAmount1_Note);
            parameters.Add("@TaxableAmount2", request.TaxableAmount2);
            parameters.Add("@TaxableAmount2_Note", request.TaxableAmount2_Note);
            parameters.Add("@TaxableAmount3", request.TaxableAmount3);
            parameters.Add("@TaxableAmount3_Note", request.TaxableAmount3_Note);
            parameters.Add("@NonTaxableAmount1", request.NonTaxableAmount1);
            parameters.Add("@NonTaxableAmount1_Note", request.NonTaxableAmount1_Note);
            parameters.Add("@NonTaxableAmount2", request.NonTaxableAmount2);
            parameters.Add("@NonTaxableAmount2_Note", request.NonTaxableAmount2_Note);
            parameters.Add("@NonTaxableAmount3", request.NonTaxableAmount3);
            parameters.Add("@NonTaxableAmount3_Note", request.NonTaxableAmount3_Note);
            parameters.Add("@Billable_Type_Id", request.Billable_Type_Id);
            parameters.Add("@ProvisionalInvoiceNumber", request.ProvisionalInvoiceNumber);
            parameters.Add("@Compliance_Fee", request.Compliance_Fee);
            parameters.Add("@Compliance_Fee_Amount", request.Compliance_Fee_Amount);
            parameters.Add("@Ctc_Deduction_Type_Id", request.Ctc_Deduction_Type_Id);
            parameters.Add("@Net_Deduction_Type_Id", request.Net_Deduction_Type_Id);
            parameters.Add("@GratuityInterest", request.Gratuityinterest);
            parameters.Add("@InsuranceAmount", request.InsuranceAmount);
            parameters.Add("@NewInvoiceNumber", request.NewInvoiceNumber);
            parameters.Add("@BGVBL", request.BGVBL);
            parameters.Add("@ASTFEE", request.ASTFEE);
            parameters.Add("@DISCT1", request.DISCT1);
            parameters.Add("@DISCT2", request.DISCT2);
            parameters.Add("@IDCARD", request.IDCARD);
            parameters.Add("@EMAIL", request.EMAIL);
            parameters.Add("@REGFEE", request.REGFEE);
            parameters.Add("@TRNFEE", request.TRNFEE);
            parameters.Add("@GGDBT", request.GGDBT);
            parameters.Add("@PPEKIT", request.PPEKIT);
            parameters.Add("@VMSFEE", request.VMSFEE);
            parameters.Add("@CALCRG", request.CALCRG);
            parameters.Add("@CALRT", request.CALRT);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "Failed";
        }

        public async Task<string> Edit(GstInvoiceEditRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", request.Action);
            parameters.Add("@UserId", request.UserId);
            parameters.Add("@Invoice_Id", request.Invoice_Id, DbType.Int32, ParameterDirection.InputOutput);
            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "Failed";
        }

        //public async Task<GstInvoiceCreateResponse> Create(GstInvoiceCreateRequest request)
        //{
        //    GstInvoiceCreateResponse invoiceDetails = new GstInvoiceCreateResponse();

        //    string result = await this._dbRepository.ExecuteGstInvoiceAsync(request);

        //    if (!string.IsNullOrWhiteSpace(result))
        //    {
        //        if (result.Contains("InvoiceId"))
        //        {
        //            // Success
        //            invoiceDetails.response = "Success";

        //            // Extract InvoiceId + Data from the result JSON
        //            var parsed = JsonConvert.DeserializeObject<GstInvoiceCreateResponse>(result);

        //            invoiceDetails.InvoiceId = parsed.InvoiceId;
        //            invoiceDetails.Data = parsed.Data;
        //        }
        //        else
        //        {
        //            // Failure
        //            invoiceDetails.response = "Failed";
        //        }
        //    }
        //    else
        //    {
        //        invoiceDetails.response = "Failed";
        //    }

        //    return invoiceDetails;

        //}

        public async Task<List<UI.Models.Invoice.InvoiceTypeUI>> GetGSTInvoiceType()
        {
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetInvoiceType");

        var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<UI.Models.Invoice.InvoiceTypeUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<UI.Models.Invoice.InvoiceTypeUI>>(res);
                return list?.ToList() ?? new List<UI.Models.Invoice.InvoiceTypeUI>();
            }
            catch (JsonException ex)
            {
                return new List<UI.Models.Invoice.InvoiceTypeUI>();
            }
        }

        public async Task<List<BillingTypeUI>> GetGSTBillableType()
        {
            string storeProcedure = "[dbo].[SP_GET_Billable_Type]" ?? "";
            var parameter = new DynamicParameters();

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<BillingTypeUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<BillingTypeUI>>(res);
                return list?.ToList() ?? new List<BillingTypeUI>();
            }
            catch (JsonException ex)
            {
                return new List<BillingTypeUI>();
            }
        }

        public async Task<List<CtcDeductionUI>> GetGSTCtcDeductionType()
        {
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetCtcDeductionType");

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<CtcDeductionUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<CtcDeductionUI>>(res);
                return list?.ToList() ?? new List<CtcDeductionUI>();
            }
            catch (JsonException ex)
            {
                return new List<CtcDeductionUI>();
            }
        }

        public async Task<List<NewDeductionUI>> GetGSTNetDeductionType()
        {
            string storeProcedure = "[dbo].[USP_CommonDropDowns]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetNetDeductionType");

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<NewDeductionUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<NewDeductionUI>>(res);
                return list?.ToList() ?? new List<NewDeductionUI>();
            }
            catch (JsonException ex)
            {
                return new List<NewDeductionUI>();
            }
        }
        public async Task<List<GetGstRateUI>> GetGstRates(GetGstRateRequest request)
        {
            string storeProcedure = "[dbo].[Proc_ManageGstInvoice_newUI]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetGstRates");
            parameter.Add("@StateId", request.StateId);
              parameter.Add("@Company_Id", request.Company_Id);
            parameter.Add("@Cost_Center_Mapping_Id",request.Cost_Center_Mapping_Id);
            parameter.Add("@Group_Detail_Id",request.Group_Detail_id);
            parameter.Add("@Invoice_Date",request.Invoice_Date);
            parameter.Add("@Invoice_Id", request.Invoice_Id);
            parameter.Add("@UserId", request.UserId);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<GetGstRateUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<GetGstRateUI>>(res);
                return list?.ToList() ?? new List<GetGstRateUI>();
            }
            catch (JsonException ex)
            {
                return new List<GetGstRateUI>();
            }
        }
        public async Task<string> GetParticulars(SendRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", "GetParticulars");
            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@UserId", request.UserId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice_newUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<string> GetInvoiceStatus(InvoiceStatusUI request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Action", request.Action);
            parameters.Add("@Invoice_Id", request.Invoice_Id);
            parameters.Add("@UserId", request.UserId);
            var res = await this._dbRepository.GetItemsAsync("Proc_ManageInvoiceStatus", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";
        }

        public async Task<List<PayPeriodUI>> GetPayPeriod(PayPeriodRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Company_Id", request.Company_Id);
            parameters.Add("@Financial_Year_Id", request.Financial_Year_Id);
            var res = await this._dbRepository.GetItemsAsync("Proc_ManagePayPeriod", parameters);
            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<PayPeriodUI>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<PayPeriodUI>>(res);
                return list?.ToList() ?? new List<PayPeriodUI>();
            }
            catch (JsonException ex)
            {
                return new List<PayPeriodUI>();
            }
        }

        public async Task<string> Reject(string xmlString, string userId,string status)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Action", "Rejected");
            parameters.Add("@Status", status);
            parameters.Add("@XmlData", xmlString);
            parameters.Add("@UserId", userId);

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice_newUI", parameters);

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }
        public async Task<List<InvoiceCancelGrid>> GetAllInvoiceCancelDetails(int companyId, int payPeriodId)
        {


            string storeProcedure = "[dbo].[Proc_ManageEInvoice_NewUI]" ?? "";
            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetInvoiceCancelDetails");
            parameter.Add("@Company_Id", companyId);
            parameter.Add("@Pay_Period_Id", payPeriodId);

            var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

            if (string.IsNullOrWhiteSpace(res))
            {
                return new List<InvoiceCancelGrid>();
            }

            try
            {
                var list = JsonConvert.DeserializeObject<List<InvoiceCancelGrid>>(res);
                return list?.ToList() ?? new List<InvoiceCancelGrid>();
            }
            catch (JsonException ex)
            {
                return new List<InvoiceCancelGrid>();
            }
        }
        public async Task<InvoiceCancelResponse> BulkApproveInvoice(InvoiceCancelApprovalRequest request)
        {
            var response = new InvoiceCancelResponse();

            try
            {
                if (request?.invoice_Id == null || !request.invoice_Id.Any())
                {
                    response.Status = "FAILED";
                    response.Message = "No invoices selected";
                    return response;
                }

                string storeProcedure = "[dbo].[Proc_ManageEInvoice_NewUI]";

                var parameter = new DynamicParameters();
                parameter.Add("@Action", "GetIrnCancellationData");
                parameter.Add("@InvoiceIds", string.Join(",", request.invoice_Id));
                //parameter.Add("@Company_Id", request.CompanyId);
                //parameter.Add("@Pay_Period_Id", request.PayPeriodId);
                //parameter.Add("@QzoneUserId", request.userId);
                parameter.Add("@Remarks", request.remarks);
                parameter.Add("@Status", "Approved");



                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameter);

                if (string.IsNullOrWhiteSpace(res))
                {
                    response.Status = "FAILED";
                    response.Message = "No response from database";
                    return response;
                }

                var invoiceResults = JsonConvert.DeserializeObject<List<InvoiceCancelResult>>(res);

                if (invoiceResults == null || !invoiceResults.Any())
                {
                    response.Status = "FAILED";
                    response.Message = "No invoices processed";
                    return response;
                }

                response.InvoiceResults = invoiceResults;

                // ✅ Only include backend-approved Invoice IDs for credit note
                foreach (var result in invoiceResults)
                {
                    if (result.Status?.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        response.CreditnoteInvoices.InvoiceIds.Add(result.Invoice_Id);
                    }
                }

                // Failed invoices
                var failedInvoices = invoiceResults
                    .Where(x => x.Status?.Equals("FAILED", StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();

                if (failedInvoices.Any() && response.CreditnoteInvoices.InvoiceIds.Any())
                {
                    response.Status = "PARTIAL_SUCCESS";
                    response.Message = "Some invoices failed: " +
                        string.Join(" | ", failedInvoices.Select(x => $"Invoice {x.Invoice_No}: {x.Error_Message}"));
                }
                else if (failedInvoices.Any())
                {
                    response.Status = "FAILED";
                    response.Message = string.Join(" | ", failedInvoices.Select(x => $"Invoice {x.Invoice_No}: {x.Error_Message}"));
                }
                else
                {
                    response.Status = "SUCCESS";
                    response.Message = "Invoices approved successfully";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Status = "FAILED";
                response.Message = "Error while approving invoices: " + ex.Message;
                return response;
            }
        }
        public async Task<string> BulkRejectInvoice(InvoiceCancelApprovalRequest request)
        {
            var response = new InvoiceCancelResponse();

            string storeprocedure = "[dbo].[Proc_ManageEInvoice_NewUI]";

            var parameter = new DynamicParameters();
            parameter.Add("@Action", "GetIrnCancellationData");
            parameter.Add("@InvoiceIds", string.Join(",", request.invoice_Id));
            parameter.Add("@Status", "Rejected");
            parameter.Add("@Remarks", request.remarks);
            //parameter.Add("@Company_Id", request.CompanyId);
            //parameter.Add("@Pay_Period_Id", request.PayPeriodId);
            //parameter.Add("@QzoneUserId", request.userId);

            return await _dbRepository.GetItemsAsync(storeprocedure, parameter);
        }

        Task<string> IGSTInvoiceRepository.PostCancelReject(string xmlString, string userId)
        {
            throw new NotImplementedException();
        }

        //Task<string> IGSTInvoiceRepository.Create(GstInvoiceCreateRequest request)
        //{
        //    throw new NotImplementedException();
        //}  
        public async Task<string> SaveBatchResponse(
     int statusCode,
     string responseMessage,
     string response,
     string responseXml,
     string invoiceIds,
     string mode,
     string userId)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Action"] = mode,
                ["@StatusCode"] = statusCode,
                ["@ResponseMessage"] = responseMessage,
                ["@Response"] = response,
                ["@XmlData"] = responseXml,
                ["@InvoiceIds"] = invoiceIds,
                ["@QzoneUserId"] = userId
            };

            return await _dbRepository.GetItemsAsync(
                "Proc_ManageEInvoice_NewUI",
                parameters
            );
        }

        public async Task<EInvoice> GetEInvoiceData(string invoiceIds, string UserId, string Action)
        {
            {
                var dbResults = _dbRepository.GetEInvoiceData(invoiceIds, UserId, Action);
                return dbResults;
            }
        }


        public string GetFilename(int invoice_Id)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@InvoiceId", invoice_Id);

            var res = this._dbRepository.GetItemsAsync("Proc_GetInvoiceCancelDoc", parameters).Result;

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            return "No data found";

        }

        public async Task<InvoiceDetail> GetInvoiceDetailByInvoiceId(int invoiceId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Invoice_Id", invoiceId);
            string storedProcedure = "SP_IRN_GeneratedStatus_InvoiceDetail";

            var res = await _dbRepository.GetItemsAsync(storedProcedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {
                var resultList = JsonConvert.DeserializeObject<List<InvoiceDetail>>(res);

                return resultList.FirstOrDefault() ?? new InvoiceDetail();
            }
            else
            {
                return new InvoiceDetail();
            }

        }

        public async Task<ClientPeriodUI> CompanyPayPeriod(int payperiod)
        {
            var parameters = new DynamicParameters();
            string storeProcedure = "SP_GetCompanyCodeAndPayPeriod";
            parameters.Add("@PayPeriod", payperiod);
            var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

            if (res != null)
            {
                var company = JsonConvert.DeserializeObject<List<ClientPeriodUI>>(res);
                return company.FirstOrDefault() ?? new ClientPeriodUI { Company_Code = "", Pay_Period = "" };
            }
            else
            {
                return new ClientPeriodUI { Company_Code = "", Pay_Period = "" };
            }
        }

        public async Task<InvoiceNumberLotUI> IRNStatusGenerationUpdate(string Invoice_Number)
        {
            string procedure = "SP_PayRegister_Invoice";
            var parameter = new DynamicParameters();
            parameter.Add("@Flag", "IRNUpdate");
            parameter.Add("@InvoiceNumber", Invoice_Number);
            var res = await this._dbRepository.GetItemsAsync(procedure, parameter);
            if (!string.IsNullOrWhiteSpace(res))
            {
                var resultList = JsonConvert.DeserializeObject<List<InvoiceNumberLotUI>>(res);

                return resultList.FirstOrDefault() ?? new InvoiceNumberLotUI();
            }
            else
            {
                return new InvoiceNumberLotUI();
            }
        }

        public async Task<List<AttributeUI>> GetAllAttribute(AttributeUI attributeUI)
            {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", attributeUI.id);
            parameter.Add("@AttributeName", attributeUI.AttributeName);
            parameter.Add("@Isactive", attributeUI.IsActive);
            parameter.Add("@CreatedBy", attributeUI.CreatedBy);
            parameter.Add("@ActionType", attributeUI.ActionType);
            parameter.Add("@CompanyId", attributeUI.CompanyId);

            var res = await _dbRepository.GetItemsAsync("SP_tbl_Attributes_AddUpdate", parameter);

            if (res != null)
            {
                var attribute = JsonConvert.DeserializeObject<List<AttributeUI>>(res);
                return attribute;
            }
            else
            {
                return new List<AttributeUI>();
            }
        }

        public async Task<InvoiceResponse> UploadAttributes(IFormFile file, [FromForm] string CompanyId,
          [FromForm] string payperiodId, [FromForm] string CreatedBy)
        {
            InvoiceResponse invoiceDetails = new InvoiceResponse();

            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_config["ClaimDocPath"].ToString(), "Invoice", "Attributes");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var newFileName = $"Attributes_{CompanyId}_{payperiodId}_{datePrefix}{extension}";

                var filePath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    invoiceDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return invoiceDetails;
                }

                DataTable dtToSerialize = ds.Tables[0];

                if (!dtToSerialize.Columns.Contains("Company_Id"))
                    dtToSerialize.Columns.Add("Company_Id", typeof(string));

                if (!dtToSerialize.Columns.Contains("PayPeriod_Id"))
                    dtToSerialize.Columns.Add("PayPeriod_Id", typeof(int));

                // Add extra columns that SQL expects
                if (!dtToSerialize.Columns.Contains("Narration"))
                    dtToSerialize.Columns.Add("Narration", typeof(string));

                if (!dtToSerialize.Columns.Contains("PO_Number"))
                    dtToSerialize.Columns.Add("PO_Number", typeof(string));

                if (!dtToSerialize.Columns.Contains("GL_Code"))
                    dtToSerialize.Columns.Add("GL_Code", typeof(string));

                if (!dtToSerialize.Columns.Contains("Cost_Center_Name"))
                    dtToSerialize.Columns.Add("Cost_Center_Name", typeof(string));

                if (!dtToSerialize.Columns.Contains("Client_SPOC_Name"))
                    dtToSerialize.Columns.Add("Client_SPOC_Name", typeof(string));

                if (!dtToSerialize.Columns.Contains("Work_Order_Number"))
                    dtToSerialize.Columns.Add("Work_Order_Number", typeof(string));

                foreach (DataRow row in dtToSerialize.Rows)
                {
                    row["Company_Id"] = CompanyId;   // or actual PayPeriod from UI
                    row["PayPeriod_Id"] = payperiodId;
                }

                foreach (DataRow row in dtToSerialize.Rows)
                {
                    foreach (DataColumn col in dtToSerialize.Columns)
                    {
                        if (row.IsNull(col))
                            row[col] = string.Empty; // replace DBNull with empty string
                    }
                }


                // Convert to XML
                using var xmlWriter = new StringWriter();
                dtToSerialize.TableName = "Table";  // Required for SQL XQuery
                DataSet xmlDS = new DataSet("NewDataSet");
                xmlDS.Tables.Add(dtToSerialize.Copy());

                xmlDS.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();
                string storeProcedure = "Proc_Upload_GSTInvoice_Attributes";
                var parameters = new DynamicParameters();

                parameters.Add("@XML_File", xmlInput);
                parameters.Add("@CreatedBy", CreatedBy);

                var res = await _dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<ResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Result ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Row(s) Uploaded Successfully.", StringComparison.OrdinalIgnoreCase))
                        {
                            invoiceDetails.response = message;
                        }
                        else
                        {
                            invoiceDetails.response = "Failed to import.";
                            invoiceDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        invoiceDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    invoiceDetails.response = "Failed";
                }
            }
            else
            {
                invoiceDetails.response = "File not found";
            }
            return invoiceDetails;
        }


        public class ResponseModel
        {
            public string Result { get; set; }
            public string Error_Message { get; set; }
        }


        public static DataSet ExcelToDataSet(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var dataSet = new DataSet();

            foreach (var worksheet in workbook.Worksheets)
            {
                var dataTable = new DataTable(worksheet.Name);
                bool firstRow = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            string columnName = cell.IsEmpty() ? $"Column{cell.Address.ColumnNumber}" : cell.GetValue<string>();
                            dataTable.Columns.Add(columnName);
                        }
                        firstRow = false;
                    }
                    else
                    {
                        var values = row.Cells(1, dataTable.Columns.Count)
                                        .Select(cell => cell.IsEmpty() ? string.Empty : cell.GetValue<string>())
                                        .ToArray();

                        dataTable.Rows.Add(values);
                    }
                }

                dataSet.Tables.Add(dataTable);
            }

            return dataSet;
        }

        public async Task<DataSet> GetConsolidateInvoiceSummary(int companyId, int payperiodid)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Company_id"] = companyId,
                ["@Pay_Period_id"] = payperiodid,
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SpInvoiceDetails_Report_Companywise", parameters, 1500);
        }
        public async Task<DataSet> GetEInvoiceErrorHover(int invoiceId)
        {
            var parameters = new Dictionary<string, object?>
            {


                ["@Invoice_Id"] = invoiceId,
                ["@Company_Id"] = 0,
                ["@Pay_Period_Id"] = "0",
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Get_EInvoice_Error_Invoicewise", parameters, 1500);

        }
        public async Task<DataSet> GetEInvoiceError(int invoiceId)
        {
            var parameters = new Dictionary<string, object?>
            {


                ["@Invoice_Id"] = invoiceId,
                ["@Company_Id"] = 0,
                ["@Pay_Period_Id"] = "0",
            };

            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("SP_Get_EInvoice_Error_Invoicewise", parameters, 1500);

        }

    }
}
