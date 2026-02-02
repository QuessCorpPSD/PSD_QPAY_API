using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QPay.BAL.IRepository.Billing;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.DAL.Repository;
using QPay.UI.Common;
using QPay.UI.GlobalMaster;
using QPay.UI.Models;
using QPay.UI.Models.Customer;
using QPay.UI.Utilities;
using QPay.UI_Domain.Models.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QPay.UI.Billing.GenericUpload;
using static QPay.UI.Customer.Company;
using static QPay.UI_Domain.Models.PurchaseOrder.PoRequest;

namespace QPay.BAL.Repository.Billing
{
    public class GenericUploadRepository : IGenericUploadRepository
    {

        private readonly DbRepository _dbRepository;
        private readonly IConfiguration _configuration;

        public GenericUploadRepository(DbRepository dbRepository, IConfiguration configuration)
        {
            this._dbRepository = dbRepository;
            this._configuration = configuration;
        }
       
        public async Task<DataSet> masters(int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@Action"] = "BankInvoiceGenericUploadTypes",
                ["@CreatedBy"] = userId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("USP_CommonDropDowns", parameters);
        }

        public async Task<DataTable> DownloadTemplate(string UploadType)
        {
            DataTable dt = GetExcelColumnNames(UploadType);
            return dt;
        }

        public async Task<GenericUploadResponse> FileUpload(IFormFile file, [FromForm] string uploadType, [FromForm] int createdBy)
        {
            GenericUploadResponse poDetails = new GenericUploadResponse();
            if (file != null && file.Length != 0)
            {
                var uploadsFolder = Path.Combine(_configuration["ClaimDocPath"].ToString(), "GenericUpload");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var datePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                
                var filePath = Path.Combine(uploadsFolder, originalFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                if (uploadType == "Cancel Invoice Mapping")
                {
                    string strXml = WriteXmlFromExcel.ReadFileFromExcel(filePath, "CancelInvoiceMappingTemplate");
                    if (strXml.Contains("Columns Name are not matching") == true)
                    {
                        poDetails.response = "Columns Name are not matching please upload valid template";
                        return poDetails;
                    }
                }

                DataSet ds = new DataSet("DocumentElement");
                ds = ExcelToDataSet(filePath);
                //Convert dt to XML
                if (ds.Tables.Count == 0)
                {
                    poDetails.response = "Excel sheet is empty or not formatted correctly.";
                    return poDetails;
                }
                DataSet dscolumns = new DataSet();
                foreach (DataTable dt in ds.Tables)
                {
                    DataTable newTable = dt.Clone();

                    if (dt.Rows.Count > 0)
                        newTable.ImportRow(dt.Rows[0]);

                    dscolumns.Tables.Add(newTable);
                }

                DataTable dtToSerilize = new DataTable();
                dtToSerilize = ds.Tables[0];

                // Convert DataTable to XML
                using var xmlWriter = new StringWriter();
                ds.WriteXml(xmlWriter, XmlWriteMode.IgnoreSchema);
                string xmlInput = xmlWriter.ToString();

                string storeProcedure = @"Proc_Upload_PartialHoldEmployeeSalary";
                var parameters = new DynamicParameters();

                parameters.Add("@xmlInput", xmlInput);
                parameters.Add("@UploadType", uploadType);
                parameters.Add("@CreatedBy", createdBy);
                var res = await this._dbRepository.GetItemsAsync(storeProcedure, parameters);

                if (!string.IsNullOrWhiteSpace(res))
                {
                    try
                    {
                        var resultList = JsonConvert.DeserializeObject<List<GenericUploadResponseModel>>(res);
                        var message = resultList?.FirstOrDefault()?.Error_Message ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(message) &&
                            message.Contains("Record(s) Inserted Successfully!", StringComparison.OrdinalIgnoreCase))
                        {
                            poDetails.response = message;
                        }
                        else
                        {
                            poDetails.response = "Failed to import.";
                            poDetails.errors = res
                                ?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList() ?? new List<string> { "Unknown error." };
                        }
                    }
                    catch
                    {
                        poDetails.response = "Error while processing response.";
                    }
                }
                else
                {
                    poDetails.response = "Failed";
                }

            }
            else
            {
                poDetails.response = "File not found";
            }
            return poDetails;
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

        public DataTable GetExcelColumnNames(string UploadType)
        {
            DataTable dt = new DataTable();
            try
            {
                if (UploadType == "PartialHoldSalary")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("HoldAmount");
                    dt.Columns.Add("SalaryType");
                    dt.Columns.Add("HoldReason");
                }
                else if (UploadType == "AccountNumberRequest")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                }
                else if (UploadType == "CancelInvoiceMapping")
                {
                    dt.Columns.Add("Invoice_No");
                    dt.Columns.Add("Employee_code");
                    dt.Columns.Add("New_Invoice_No");
                }
                else if (UploadType == "PartialHoldSalaryRelease")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("PartialReleaseAmount");
                    dt.Columns.Add("SalaryType");
                }
                else if (UploadType == "BonusRelease")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                }
                else if (UploadType == "BankInvoiceManualUpload")
                {
                    dt.Columns.Add("Company_Code");
                    dt.Columns.Add("Invoice_No");
                    dt.Columns.Add("Employee_Code");
                    dt.Columns.Add("Pay_Period");
                    dt.Columns.Add("Bank_Name");
                    dt.Columns.Add("Account_No");
                    dt.Columns.Add("IFSC_Code");
                    dt.Columns.Add("Net_Pay");
                    dt.Columns.Add("CTC");
                    dt.Columns.Add("Data_From");
                    dt.Columns.Add("Input_no");
                }
                else if (UploadType == "ManualBatchCreation")
                {
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("SalaryReleaseDate");
                }
                else if (UploadType == "BadDebtUpdate")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("BadDebtAmount");
                    dt.Columns.Add("Remarks");
                    dt.Columns.Add("ApprovedBy");
                }
                else if (UploadType == "BeneficiaryUpdate")
                {
                    dt.Columns.Add("EmployeeCode");
                }
                else if (UploadType == "AccrualsUpload")
                {
                    dt.Columns.Add("CompanyCode");
                    dt.Columns.Add("Location");
                    dt.Columns.Add("PayPeriod");
                    dt.Columns.Add("HeadCount");
                    dt.Columns.Add("CTC");
                    dt.Columns.Add("AdditionalCTCAmount");
                    dt.Columns.Add("ServiceCharge");
                    dt.Columns.Add("AdditionalServiceCharge");
                    dt.Columns.Add("SourcingFee");
                    dt.Columns.Add("AbsorptionFee");
                    dt.Columns.Add("OnboardingCharge");
                    dt.Columns.Add("InedgeCharge");
                    dt.Columns.Add("UpfrontCharge");
                    dt.Columns.Add("Mode");
                }
                else if (UploadType == "MultipleClientInvoiceMapping")
                {
                    dt.Columns.Add("Invoice_No");
                    dt.Columns.Add("Employee_code");
                    dt.Columns.Add("New_Invoice_No");
                }
                else if (UploadType == "InvoiceAdhoc")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("Particulars");
                    dt.Columns.Add("NoofBeneficiaries");
                    dt.Columns.Add("Amount_Beneficiary");
                }
                else if (UploadType == "CreditNoteAdjustment")
                {
                    dt.Columns.Add("CreditNoteNumber");
                    dt.Columns.Add("AdjustedAmount");
                }
                else if (UploadType.ToLower() == "mapnamechanges")
                {
                    dt.Columns.Add("COMPANY_CODE");
                    dt.Columns.Add("EMPLOYEE_CODE");
                    dt.Columns.Add("PAY_PERIOD");
                    dt.Columns.Add("ACTION");
                }
                else if (UploadType.ToLower() == "otherincomemapnamechanges")
                {
                    dt.Columns.Add("COMPANY_CODE");
                    dt.Columns.Add("EMPLOYEE_CODE");
                    dt.Columns.Add("PAY_PERIOD");
                    dt.Columns.Add("INPUT_NO");
                    dt.Columns.Add("MAP_NAME");
                    dt.Columns.Add("ACTION");
                }
                else if (UploadType.ToLower() == "attendanceleaveupdate")
                {
                    dt.Columns.Add("EMPLOYEE_CODE");
                    dt.Columns.Add("PAY_PERIOD");
                    dt.Columns.Add("LEAVE_OPENING_BALANCE");
                    dt.Columns.Add("LEAVE_CREDIT");
                    dt.Columns.Add("LEAVE_CLOSING_BALANCE");
                    dt.Columns.Add("ACTUAL_LEAVE_CLOSING_BALANCE");
                    dt.Columns.Add("LEAVE_AVAILED");
                }

                else if (UploadType.ToLower() == "invoiceaxisdsa")
                {
                    dt.Columns.Add("Invoice_Number");
                    dt.Columns.Add("Personal_Loans");
                    dt.Columns.Add("Gold_Loan");
                    dt.Columns.Add("Education_Loan");
                    dt.Columns.Add("Auto");
                    dt.Columns.Add("CV_CE");
                    dt.Columns.Add("Two_Wheeler");
                    dt.Columns.Add("Home_Loan");
                    dt.Columns.Add("LAP");
                    dt.Columns.Add("ASHA");
                    dt.Columns.Add("LAS");
                    dt.Columns.Add("SBB");
                    dt.Columns.Add("B2B_B2C_GOLD_LOAN");
                    dt.Columns.Add("MSME");
                    dt.Columns.Add("CLWF");
                    dt.Columns.Add("Tractor_Loan");
                    dt.Columns.Add("RENT");
                    dt.Columns.Add("HOUSEKEEPING");
                    dt.Columns.Add("SECURITY");
                    dt.Columns.Add("ELECTRICITY");
                    dt.Columns.Add("DEPRECIATION");
                }
                else if (UploadType == "UanActiveStatus")
                {
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("UanActiveStatus");
                    dt.Columns.Add("UanActiveStatusRemarks");
                }
                else if (UploadType.ToLower() == "invoicebayer")
                {
                    dt.Columns.Add("Invoice_Number");
                    dt.Columns.Add("PO_Number");
                    dt.Columns.Add("PO_description");
                    dt.Columns.Add("PO_line");
                    dt.Columns.Add("Unit_Quantity");
                    dt.Columns.Add("Unit_Cost");
                    dt.Columns.Add("Total_Cost");
                    dt.Columns.Add("GL");
                    dt.Columns.Add("Cost_Center");
                    dt.Columns.Add("Order_Number");
                    dt.Columns.Add("CA_Name");

                }
                else if (UploadType == "PFECRDataRemoval")
                {
                    dt.Columns.Add("CompanyCode");
                    dt.Columns.Add("PayPeriod");
                    dt.Columns.Add("EmployeeCode");
                }
                else if (UploadType == "PanNumberupload")
                {
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("PanNumberRemarks");
                }
                else if (UploadType == "TiscActivationStatus")
                {
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("TiscActiveStatus");
                    dt.Columns.Add("TiscActiveStatusRemarks");
                }
                else if (UploadType == "EsiSubCode")
                {
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("EsiSubCode");
                    dt.Columns.Add("EsiSubCodeName");
                }
                else if (UploadType == "ESIECRDataRemoval")
                {
                    dt.Columns.Add("CompanyCode");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("PayPeriod");
                }
                else if (UploadType == "AadhaarAuthenticateStatus")
                {
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("AadhaarAuthenticateStatus");
                    dt.Columns.Add("Remarks");
                }
                else if (UploadType == "SubmissionDateUpdate")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("SubmissionDate");
                }
                else if (UploadType == "InvoiceMPVersionOne")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("SerialNo");
                    dt.Columns.Add("Service");
                    dt.Columns.Add("SAC");
                    dt.Columns.Add("Units");
                    dt.Columns.Add("Rate");
                    dt.Columns.Add("Amount");
                    dt.Columns.Add("TaxableValue");
                    dt.Columns.Add("CGSTRate");
                    dt.Columns.Add("CGSTAmount");
                    dt.Columns.Add("SGSTRate");
                    dt.Columns.Add("SGSTAmount");
                    dt.Columns.Add("UTGSTRate");
                    dt.Columns.Add("UTGSTAmount");
                    dt.Columns.Add("IGSTRate");
                    dt.Columns.Add("IGSTAmount");
                    dt.Columns.Add("Total");
                }
                else if (UploadType == "InvoiceMPVersionTwo")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("SerialNo");
                    dt.Columns.Add("SAC");
                    dt.Columns.Add("Service");
                    dt.Columns.Add("NOP");
                    dt.Columns.Add("Rate");
                    dt.Columns.Add("PerDay");
                    dt.Columns.Add("Present");
                    dt.Columns.Add("Wage");
                    dt.Columns.Add("EPF");
                    dt.Columns.Add("ESI");
                    dt.Columns.Add("Bonus");
                    dt.Columns.Add("LWFR");
                    dt.Columns.Add("INCENTIVE");
                    dt.Columns.Add("ServiceCharge");
                    dt.Columns.Add("TaxableAmount");
                    dt.Columns.Add("CGSTRate");
                    dt.Columns.Add("CGSTAmount");
                    dt.Columns.Add("SGSTRate");
                    dt.Columns.Add("SGSTAmount");
                    dt.Columns.Add("UTGSTRate");
                    dt.Columns.Add("UTGSTAmount");
                    dt.Columns.Add("IGSTRate");
                    dt.Columns.Add("IGSTAmount");
                    dt.Columns.Add("Total");
                }
                else if (UploadType == "InvoiceMPVersionThree")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("SerialNo");
                    dt.Columns.Add("Designation");
                    dt.Columns.Add("Quantity");
                    dt.Columns.Add("Attendance");
                    dt.Columns.Add("Rate");
                    dt.Columns.Add("GrossAmount");
                    dt.Columns.Add("Bonus");
                    dt.Columns.Add("LWFR");
                    dt.Columns.Add("INCENTIVE");
                    dt.Columns.Add("PF");
                    dt.Columns.Add("ESIC");
                    dt.Columns.Add("ServiceCharge");
                    dt.Columns.Add("TaxableCTC");
                }
                else if (UploadType == "InvoiceMPSalaryDetail")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("Salary");
                    dt.Columns.Add("Bonus");
                    dt.Columns.Add("ESI");
                    dt.Columns.Add("PF");
                    dt.Columns.Add("ServiceCharge");
                    dt.Columns.Add("LWFR");
                    dt.Columns.Add("INCENTIVE");
                }
                else if (UploadType == "PoCultureUpload")
                {
                    dt.Columns.Add("CompanyCode");
                    dt.Columns.Add("MapName");
                    dt.Columns.Add("PurchaseRequestNo");
                }
                //InvoiceVoltas
                else if (UploadType == "InvoiceVoltas")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("CallsClosed");
                    dt.Columns.Add("ChargesPerCall");
                }
                //InvoiceJohnDeere
                else if (UploadType == "InvoiceJohnDeere")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("Discount_1");
                    dt.Columns.Add("Discount_2");
                }
                else if (UploadType == "QITSDepartment")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("Dept_Code");
                    dt.Columns.Add("Dept_Name");
                }
                else if (UploadType == "RejoineeUpload")
                {
                    dt.Columns.Add("OldCompanyCode");
                    dt.Columns.Add("NewCompanyCode");
                    dt.Columns.Add("NewDOJ");
                    dt.Columns.Add("OldEmpID");
                    dt.Columns.Add("Rejoinee");
                    dt.Columns.Add("JoiningPayPeriod");
                    dt.Columns.Add("GroupName");
                    dt.Columns.Add("Department");
                    dt.Columns.Add("Designation");
                    dt.Columns.Add("PayCategory");
                    dt.Columns.Add("CostCentre");
                    dt.Columns.Add("Mapname");
                    dt.Columns.Add("NewOmsid");
                    dt.Columns.Add("EnityLocation");
                }
                else if (UploadType == "UBRRemarkUpdate")
                {
                    dt.Columns.Add("FinalStatus");
                    dt.Columns.Add("ReasonForInvoicePending");
                    dt.Columns.Add("ExpectedClosureDate");
                    dt.Columns.Add("AxpertEmployeeID");
                    dt.Columns.Add("PayPeriod");
                }
                else if (UploadType == "PFEDLICharges")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("PfEdliCharges");
                }
                else if (UploadType == "WBSUpdation")
                {
                    dt.Columns.Add("EmployeeCode");
                }
                else if (UploadType == "PayRegisterUploadDeletion")
                {
                    dt.Columns.Add("Company_Code");
                    dt.Columns.Add("Pay_Period");
                    dt.Columns.Add("Employee_Code");
                }
                else if (UploadType == "DBTHoldSalary")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("HoldAmount");
                    dt.Columns.Add("SalaryType");
                }
                else if (UploadType == "DBTHoldSalaryRelease")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("DBTReleaseAmount");
                    dt.Columns.Add("SalaryType");
                }
                else if (UploadType == "BlockCNAmountHold")
                {
                    dt.Columns.Add("CompanyCode");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("CreditNoteNo");
                    dt.Columns.Add("Amount");
                    dt.Columns.Add("HoldStatus");
                }
                else if (UploadType == "PartialHoldReversal")
                {
                    dt.Columns.Add("InvoiceNumber");
                    dt.Columns.Add("EmployeeCode");
                    dt.Columns.Add("ReversalAmount");
                    dt.Columns.Add("SalaryType");
                    dt.Columns.Add("ReversalReason");
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return dt;
        }


    }
}
