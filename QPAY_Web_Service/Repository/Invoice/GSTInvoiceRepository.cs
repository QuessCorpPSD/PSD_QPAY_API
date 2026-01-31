using Azure;
using Azure.Core;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Invoice;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.Invoice;
using QPay.UI.Models;
using QPay.UI.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var res = await this._dbRepository.GetItemsAsync("Proc_ManageGstInvoice", parameters);

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
                parameter.Add("@Company_Id", request.CompanyId);
                parameter.Add("@Pay_Period_Id", request.PayPeriodId);
                parameter.Add("@QzoneUserId", request.userId);

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


        Task<List<GstInvoiceGrid>> IGSTInvoiceRepository.GetGSTInvoice(int userId)
        {
            throw new NotImplementedException();
        }

        DataSet IGSTInvoiceRepository.GetInvoiceData(int invoiceId)
        {
            throw new NotImplementedException();
        }

        Task<string> IGSTInvoiceRepository.PostCancelReject(string xmlString, string userId)
        {
            throw new NotImplementedException();
        }

        //Task<string> IGSTInvoiceRepository.Create(GstInvoiceCreateRequest request)
        //{
        //    throw new NotImplementedException();
        //}  


        public async Task<List<EInvoiceCancel>> GetEInvoiceData(
    string invoiceIds,
    string userId,
    string action)
        {
            var result = new EInvoiceCancel
            {
                docs = new List<Docs>()
            };

            string storedProcedure = "[dbo].[Proc_ManageEInvoice_NewUI]";

            var parameters = new DynamicParameters();
            parameters.Add("@Action", action ?? string.Empty, DbType.String, ParameterDirection.Input);
            parameters.Add("@QzoneUserId", userId ?? string.Empty, DbType.String, ParameterDirection.Input);
            parameters.Add("@InvoiceIds", invoiceIds ?? string.Empty, DbType.String, ParameterDirection.Input);

            try
            {
                // Call the repository and get a wrapped response
                var res = await _dbRepository.GetItemsAsync(storedProcedure, parameters);

                if (string.IsNullOrWhiteSpace(res))
                    return null;

                try
                {
                    var response = JsonConvert.DeserializeObject<List<EInvoiceCancel>>(res);
                    //return response.ToList();

                    foreach (var item in response)
                    {

                        var header = item;

                    result.client_id = Convert.ToString(header.client_id);
                    result.client_hash = Convert.ToString(header.client_hash);
                    result.pan = Convert.ToString(header.pan);
                    result.ip_addr = Convert.ToString(header.ip_addr);
                    result.file_type = Convert.ToString(header.file_type);

                    // ---------------------------
                    // Group by Invoice / Doc_No
                    // ---------------------------
                    foreach (var grp in item.GroupBy(r => r.Doc_No))
                    {
                        var first = grp.First();
                        var docs = new Docs();

                        #region Version
                        docs.Version = Convert.ToString(first.Version);
                        #endregion

                        #region Transaction Details
                        docs.TranDtls = new TranDtls
                        {
                            Tran_TaxSch = Convert.ToString(first.Tran_TaxSch),
                            Tran_SupTyp = Convert.ToString(first.Tran_SupTyp),
                            Tran_RegRev = Convert.ToString(first.Tran_RegRev),
                            Tran_Typ = Convert.ToString(first.Tran_Typ),
                            Tran_Ecmgstin = Convert.ToString(first.Tran_Ecmgstin),
                            Tran_IgstOnIntra = Convert.ToString(first.Tran_IgstOnIntra)
                        };
                        #endregion

                        #region Document Details
                        docs.DocDtls = new DocDtls
                        {
                            Doc_Typ = Convert.ToString(first.Doc_Typ),
                            Doc_No = Convert.ToString(first.Doc_No),
                            Doc_Dt = Convert.ToString(first.Doc_Dt),
                            Doc_FY = Convert.ToString(first.Doc_FY)
                        };
                        #endregion

                        #region Seller Details
                        docs.SellerDtls = new SellerDtls
                        {
                            Seller_Gstin = Convert.ToString(first.Seller_Gstin),
                            Seller_LglNm = Convert.ToString(first.Seller_LglNm),
                            Seller_TrdNm = Convert.ToString(first.Seller_TrdNm),
                            Seller_Addr1 = Convert.ToString(first.Seller_Addr1),
                            Seller_Addr2 = Convert.ToString(first.Seller_Addr2),
                            Seller_Loc = Convert.ToString(first.Seller_Loc),
                            Seller_Pin = Convert.ToInt32(first.Seller_Pin),
                            Seller_Stcd = Convert.ToInt32(first.Seller_Stcd),
                            Seller_Ph = Convert.ToInt64(first.Seller_Ph),
                            Seller_Em = Convert.ToString(first.Seller_Em)
                        };
                        #endregion

                        #region Buyer Details
                        docs.BuyerDtls = new BuyerDtls
                        {
                            Buyer_GSTIN = Convert.ToString(first.Buyer_GSTIN),
                            Buyer_LglNm = Convert.ToString(first.Buyer_LglNm),
                            Buyer_TrdNm = Convert.ToString(first.Buyer_TrdNm),
                            Buyer_POS = Convert.ToString(first.Buyer_POS),
                            Buyer_Addr1 = Convert.ToString(first.Buyer_Addr1),
                            Buyer_Addr2 = Convert.ToString(first.Buyer_Addr2),
                            Buyer_Loc = Convert.ToString(first.Buyer_Loc),
                            Buyer_Pin = Convert.ToInt32(first.Buyer_Pin),
                            Buyer_Stcd = Convert.ToInt32(first.Buyer_Stcd),
                            Buyer_Ph = Convert.ToInt64(first.Buyer_Ph),
                            Buyer_Em = Convert.ToString(first.Buyer_Em)
                        };
                        #endregion

                        #region Dispatch + Shipping
                        docs.DispDtls = new DispDtls
                        {
                            Dispatch_Fr_Nm = Convert.ToString(first.Dispatch_Fr_Nm),
                            Dispatch_Fr_Addr1 = Convert.ToString(first.Dispatch_Fr_Addr1),
                            Dispatch_Fr_Addr2 = Convert.ToString(first.Dispatch_Fr_Addr2),
                            Dispatch_Fr_Loc = Convert.ToString(first.Dispatch_Fr_Loc),
                            Dispatch_Fr_Pin = Convert.ToInt32(first.Dispatch_Fr_Pin),
                            Dispatch_Fr_Stcd = Convert.ToInt32(first.Dispatch_Fr_Stcd),
                            Dispatch_Fr_Ph = Convert.ToInt64(first.Dispatch_Fr_Ph),
                            Dispatch_Fr_Em = Convert.ToString(first.Dispatch_Fr_Em)
                        };

                        docs.ShipDtls = new ShipDtls
                        {
                            Ship_To_Gstin = Convert.ToString(first.Ship_To_Gstin),
                            Ship_To_LglNm = Convert.ToString(first.Ship_To_LglNm),
                            Ship_To_TrdNm = Convert.ToString(first.Ship_To_TrdNm),
                            Ship_To_Addr1 = Convert.ToString(first.Ship_To_Addr1),
                            Ship_To_Addr2 = Convert.ToString(first.Ship_To_Addr2),
                            Ship_To_Loc = Convert.ToString(first.Ship_To_Loc),
                            Ship_To_Pin = Convert.ToInt32(first.Ship_To_Pin),
                            Ship_To_Stcd = Convert.ToInt32(first.Ship_To_Stcd),
                            Ship_To_Ph = Convert.ToInt64(first.Ship_To_Ph),
                            Ship_To_Em = Convert.ToString(first.Ship_To_Em)
                        };
                        #endregion

                        #region Item Details
                        docs.ItemList = new List<ItemList>();

                        foreach (var row in grp)
                        {
                            var item = new ItemList
                            {
                                Item_SlNo = Convert.ToInt32(row.Item_SlNo),
                                Item_PrdDesc = Convert.ToString(row.Item_PrdDesc),
                                Item_IsServc = Convert.ToString(row.Item_IsServc),
                                Item_HsnCd = Convert.ToString(row.Item_HsnCd),
                                Item_Qty = Convert.ToInt32(row.Item_Qty),
                                Item_Unit = Convert.ToString(row.Item_Unit),
                                Item_TotItemVal = Convert.ToString(row.Item_TotItemVal),
                                Ref11 = Convert.ToString(row.Ref11),
                                Ref12 = Convert.ToString(row.Ref12),
                                Ref13 = Convert.ToString(row.Ref13),
                                Ref14 = Convert.ToString(row.Ref14),
                                Ref15 = Convert.ToString(row.Ref15),
                                AttribDtls = new List<AttribDtls>(),
                                BchDtls = new List<BchDtls>()
                            };

                            item.AttribDtls.Add(new AttribDtls
                            {
                                Attrib_SlNo = Convert.ToInt32(row.Attrib_SlNo),
                                Attrib_Nm = Convert.ToString(row.Attrib_Nm),
                                Attrib_Val = Convert.ToString(row.Attrib_Val)
                            });

                            item.BchDtls.Add(new BchDtls
                            {
                                Bch_SlNo = Convert.ToInt32(row.Bch_SlNo),
                                Bch_Nm = Convert.ToString(row.Bch_Nm),
                                Bch_ExpDt = Convert.ToString(row.Bch_ExpDt),
                                Bch_WrDt = Convert.ToString(row.Bch_WrDt)
                            });

                            docs.ItemList.Add(item);
                        }
                        #endregion

                        #region Value / Pay / Ref / EWB / CustomRefs
                        docs.ValDtls = new ValDtls
                        {
                            Doc_TotInvVal = Convert.ToString(first.Doc_TotInvVal)
                        };

                        docs.PayDtls = new PayDtls
                        {
                            Payee_Nm = Convert.ToString(first.Payee_Nm)
                        };

                        docs.RefDtls = new RefDtls
                        {
                            Ref_InvRmk = Convert.ToString(first.Ref_InvRmk)
                        };

                        docs.EwbDtls = new EwbDtls
                        {
                            Ewb_TransID = Convert.ToString(first.Ewb_TransID)
                        };

                        docs.CustomRefs = new CustomRefs
                        {
                            Ref01 = Convert.ToString(first.Ref01)
                        };
                        #endregion

                        result.docs.Add(docs);
                    }
                }


            
            catch (JsonException ex)
            {
                // Optional: log the error for debugging
                Console.Error.WriteLine($"JSON Deserialization error: {ex.Message}");
                return null;
            }
        }
            return result;
        }


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

    }
}
