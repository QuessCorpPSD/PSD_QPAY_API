using ClosedXML.Excel;
using System.Data;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace QPay.UI.Common
{
    public static class Common
    {
        
        /// <summary>
        /// Encodes the string value sent to it
        /// </summary>
        /// <param name="stringValue">String value to be encoded</param>
        /// <returns>Encoded string</returns>
        public static string EncodeData(string stringValue)
        {
            string encodedData = string.Empty;

            var utf8Text = System.Text.Encoding.UTF8.GetBytes(stringValue);
            encodedData = System.Convert.ToBase64String(utf8Text);

            return encodedData;
        }

        /// <summary>
        /// Decodes the string value sent to it
        /// </summary>
        /// <param name="stringValue">String value to be decoded</param>
        /// <returns>Decoded string</returns>
        public static string DecodeData(string stringValue)
        {
            string encodedData = string.Empty;

            var utf8Text = System.Convert.FromBase64String(stringValue);
            encodedData = System.Text.Encoding.UTF8.GetString(utf8Text);

            return encodedData;
        }

        private static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        ///// <summary>
        ///// Creates the logger object to log the messages in the application
        ///// </summary>
        ///// <returns>Logger object</returns>
        //private static FileLogger LogDetails()
        //{
        //    string logFilePath = ConfigurationManager.AppSettings["LogFilePath"] != null ? ConfigurationManager.AppSettings["LogFilePath"].ToString() : string.Empty;
        //    string logFileName = ConfigurationManager.AppSettings["LogFileName"] != null ? ConfigurationManager.AppSettings["LogFileName"].ToString() : string.Empty;
        //    FileLogger logger = null;

        //    if (!(string.IsNullOrEmpty(logFileName) || string.IsNullOrEmpty(logFilePath)))
        //    {
        //        FileLogger.SetFileRest(logFilePath, logFileName);
        //        logger = FileLogger.GetLogger();
        //    }

        //    return logger;
        //}

        ///// <summary>
        ///// Logs information based on a string of messages
        ///// </summary>
        ///// <param name="messages">String of messages</param>
        //public static void LogInformation(params string[] messages)
        //{
        //    FileLogger logger = LogDetails();

        //    logger.info(messages);
        //}

        ///// <summary>
        ///// Logs information based on a string of messages and exception raised
        ///// </summary>
        ///// <param name="ex">Exception raised</param>
        ///// <param name="messages">String of messages</param>
        //public static void LogInformation(Exception ex, params string[] messages)
        //{
        //    FileLogger logger = LogDetails();

        //    logger.info(ex, messages);
        //}

        ///// <summary>
        ///// Logs warning information based on a string of messages
        ///// </summary>
        ///// <param name="messages">String of messages</param>
        //public static void LogWarning(params string[] messages)
        //{
        //    FileLogger logger = LogDetails();

        //    logger.warning(messages);
        //}

        ///// <summary>
        ///// Logs warning information based on a string of messages and exception raised
        ///// </summary>
        ///// <param name="ex">Exception raised</param>
        ///// <param name="messages">String of messages</param>
        //public static void LogWarning(Exception ex, params string[] messages)
        //{
        //    FileLogger logger = LogDetails();

        //    logger.warning(ex, messages);
        //}

        ///// <summary>
        ///// Logs error information based on a string of messages
        ///// </summary>
        ///// <param name="messages">String of messages</param>
        //public static void LogError(params string[] messages)
        //{
        //    FileLogger logger = LogDetails();

        //    logger.error(messages);
        //}

        ///// <summary>
        ///// Logs error information based on a string of messsages and exception raised
        ///// </summary>
        ///// <param name="ex">Exception raised</param>
        ///// <param name="messages">String of messages</param>
        //public static void LogError(Exception ex, params string[] messages)
        //{
        //    FileLogger logger = LogDetails();

        //    logger.error(ex, messages);
        //}

        public static List<string> FinancialYear(int yearlenght)
        {
            List<string> Years = new List<string>();
            DateTime startYear = DateTime.Now.AddYears(-yearlenght);
            while (startYear.Year <= DateTime.Now.AddYears(yearlenght).Year)
            {
                Years.Add(startYear.Year + "-" + ((startYear.AddYears(1).Year).ToString()).Substring(2, 2));
                startYear = startYear.AddYears(1);
            }
            return Years;
        }

        public static List<string> Category()
        {
            return new List<string>() { "Male", "Female", "Senior Citizen", "Undefined", "Not_Available", "Retainer", "Not_Available_R", "TRIBAL", "New Tax Regime", "TRAINEE", "Not_Available_NT", "SUB-CONTRACTOR", "TECH ADVISER", "COMMISSION AGENT" };
        }

       

        //Santanu [17.11.2015]
        public static List<EnumModel> GetEnumList(Enum tEnum, int index)
        {
            List<EnumModel> list = new List<EnumModel>();
            foreach (var value in Enum.GetValues(tEnum.GetType()))
            {
                var name = value.ToString();
                var number = Enum.Parse(tEnum.GetType(), name);
                list.Add(new EnumModel { Value = index.ToString(), Name = name.Replace('_', ' ') });
                index++;
            }
            return list;
        }

        public static List<EnumModel> GetEnumList(Enum tEnum, int index,int selected)
        {
            List<EnumModel> list = new List<EnumModel>();
            foreach (var value in Enum.GetValues(tEnum.GetType()))
            {
                var name = value.ToString();
                var number = Enum.Parse(tEnum.GetType(), name);
                list.Add(new EnumModel { Value = index.ToString(), Name = name.Replace('_', ' ') });
                index++;
            }
            return list;
        }

        #region 19/09/2016 Jagannath changes

        public static string GetStoreProcedureName(string type)
        {
            string StoreProcedureName = string.Empty;
            switch (type.ToUpper())
            {
                case "PAYCODE":
                    StoreProcedureName = "sp_GetAllPaycodee_ExportToExcel";
                    break;

                case "CHILDRENEXCEMPTIONCRITERIA":
                    StoreProcedureName = "sp_GetChildrenEducationAllowanceDetails_ExportToExcel";
                    break;

                case "MINIMUMWAGES":
                    StoreProcedureName = "sp_GetMinimumWagesDetails_ExportToExcel";
                    break;

                case "ESIBLOACK":
                    StoreProcedureName = "sp_GetEsiblockDetailsData_ExportToExcel";
                    break;

                case "ESISLAB":
                    StoreProcedureName = "sp_GetAllESISlab_ExportToExcel";
                    break;

                case "LWSSLAB":
                    StoreProcedureName = "sp_GetAllLWFSlabDetail_ExportToExcel";
                    break;

                case "INVOICE":
                    StoreProcedureName = "Proc_SearchInvoiceDetails_ExportToExcel";
                    break;

                case "PURCHASE REQUEST":
                    StoreProcedureName = "Proc_GetAllPurchaseRequest_ExportToExcel";
                    break;

                case "CLIENT PO MAPPING":
                    StoreProcedureName = "Proc_GetAllClientPOMapping_ExportToExcel";
                    break;

                case "PO TOP UP":
                    StoreProcedureName = "Proc_GetAllPOTopupDetailsExportToExcel";
                    break;

                case "PO BALANCE TRANSFER":
                    StoreProcedureName = "Proc_GetAllPOBalanceTransferExportToExcel";
                    break;

                case "MAGNALEAVEOPENINGBALANCE":
                    StoreProcedureName = "Proc_MagnaLeaveOpeningBalanceExportToExcel";
                    break;

                case "PARAMETER":
                    StoreProcedureName = "spSearchCategoryDetailsForParameter_ExportToExcel";
                    break;

                case "INVOICE CANCELLATION REQUEST":
                    StoreProcedureName = "Proc_GetInvoiceCancellationRequestExport2Excel";
                    break;

                case "INVOICE COLLECTION REPORT":
                    StoreProcedureName = "Sp_GetAllInvoiceCollectionReport_Export2Excel";
                    break;

                case "HOLIDAYMASTER":
                    StoreProcedureName = "spSearchHolidayMaster_ExportToExcel";
                    break;

                case "SITE MASTER":
                    StoreProcedureName = "Proc_GetGroupMasterDetailsExportToExcel";
                    break;

                case "GRATUITYEXCEMPTIONCRITERIA":
                    StoreProcedureName = "sp_GetGratuityBlockDetails_ExportToExcel";
                    break;

                case "MEDICALEXCEMPTIONCRITERIA":
                    StoreProcedureName = "sp_GetMedicalBlockDetails_ExportToExcel";
                    break;

                case "CONVEYANCEEXCEMPTIONCRITERIA":
                    StoreProcedureName = "sp_GetConveyanceCriteriaDetails_ExportToExcel";
                    break;

                case "PERK":
                    StoreProcedureName = "sp_GetPerkDetails_ExportToExcel";
                    break;

                case "COMPANY":
                    StoreProcedureName = "Company";
                    break;

                case "PAYSTRUCTURE":
                    StoreProcedureName = "PayStructure";
                    break;

                case "LEAVEOPENINGBALENCE":
                    StoreProcedureName = "sp_GetAllLeaveOpeningBalance_ExportToExcel";
                    break;

                case "LEAVETAKEN":
                    StoreProcedureName = "sp_GetAllLeaveTaken_ExportToExcel";
                    break;

                case "IT CALENDER":
                    StoreProcedureName = "Proc_GetAllITCalendarDetails_ExportToExcel";
                    break;

                case "FLEXI RULE":
                    StoreProcedureName = "Proc_Search_FlexiRule_ExportToExcel";
                    break;

                case "HRA CALCULATION":
                    StoreProcedureName = "Proc_GetAllHraDetailsByCIDEID_ExportToExcel";
                    break;

                case "LTA CALCULATION":
                    StoreProcedureName = "Proc_GetAllLTACalculation_ExportToExcel";
                    break;

                case "INCOME OR LOSS HOUSE PROPERTY":
                    StoreProcedureName = "Proc_GetAll_ILHPByCIDandEID_ExportToExcel";
                    break;

                case "GRATUITY":
                    StoreProcedureName = "Proc_GetGratuityDetails_ExportToExcel";
                    break;

                case "PAY REGISTER BUILDER":
                    StoreProcedureName = "Proc_SearchPayRegisterBuilderDetailsExportToExcel";
                    break;

                case "COMPANY PROVIDED BENEFIT":
                    StoreProcedureName = "Proc_SearchCompanyProvidedBenefits_ExportToExcel";
                    break;

                case "LEAVE TYPE":
                    StoreProcedureName = "Proc_GetAllLeaveTypeDetail_ExportToExcel";
                    break;

                case "LOANTYPE":
                    StoreProcedureName = "Proc_GetAllLoan_ExportToExcel";
                    break;

                case "CORPORATE BANK":
                    StoreProcedureName = "Proc_SerarchCorporateBank_ExportToExcel";
                    break;

                case "BAND":
                    StoreProcedureName = "Proc_GetBandDetails_ExportToExcel";
                    break;

                case "STATEDETAILS":
                    StoreProcedureName = "sp_GetAllStateByParam_ExportToExcel";
                    break;

                case "TDSSLAB":
                    StoreProcedureName = "sp_GetAllTDSSlab_ExportToExcel";
                    break;

                case "FORMULA":
                    StoreProcedureName = "sp_GetAllFormula_ExportToExcel";
                    break;

                case "BANK":
                    StoreProcedureName = "sp_GetBankDetails_ExportToExcel";
                    break;

                case "SERVICETAX":
                    StoreProcedureName = "sp_GetServiceTaxDetails_ExportToExcel";
                    break;

                case "ENTITY":
                    StoreProcedureName = "sp_GetAllEntityProfitDetail_ExportToExcel";
                    break;

                case "PROFESSIONALTAX":
                    StoreProcedureName = "sp_GetAllPTDetailsByStID_EffDt_PTType_ExportToExcel";
                    break;

                case "COMPUTATIONRULE":
                    StoreProcedureName = "sp_GetAllComputationRule_ExportToExcel";
                    break;

                case "USERMANAGEMENT":
                    StoreProcedureName = "Proc_GetAllUserDetails_ExportToExcel";
                    break;

                case "LTA":
                    StoreProcedureName = "sp_GetAllLTABlocks_ExportToExcel";
                    break;

                case "PROVIDENTFUND":
                    StoreProcedureName = "sp_GetAllProvidentFund_ExportToExcel";
                    break;

                case "PTBLOCK":
                    StoreProcedureName = "sp_GetAllPTBlockDetailsByEffectiveDate_ExportToExcel";
                    break;

                case "VENDOR":
                    StoreProcedureName = "sp_GetClientDetails_ExportToExcel";
                    break;

                case "INSURANCE MASTER":
                    StoreProcedureName = "Proc_SearchInsuranceMaster_ExportToExcel";
                    break;

                case "INSURANCE EMPLOYEE REPORT":
                    StoreProcedureName = "Proc_SearchInsuranceEmployeeReport_ExportToExcel";
                    break;

                case "DEBIT NOTE LIST":// By Anant on 8-Aug-18 for CreditNoteUpdate  ExportToExcel
                    StoreProcedureName = "sp_SearchDebitNote_ExportToExcel";
                    break;

                case "VERTICAL":
                    StoreProcedureName = "Proc_GetVerticalDetail_ExportToExcel";
                    break;

                case "ROLE MANAGEMENT DETAILS":
                    StoreProcedureName = "Proc_GetAllRoleManagement_ExportToExcel";
                    break;

                case "LOCK PAY PERIOD":
                    // StoreProcedureName = "Proc_GetLockPayPeriod_ExportToExcel";
                    StoreProcedureName = "sp_GetLockPayPeriodNewExporttoexcel";
                    break;

                case "FULL AND FINAL SETTLEMENT":
                    StoreProcedureName = "Proc_GetAllFullFinalSettlement_ExportToExcel";
                    break;

                case "TAX DECLARATION & ACTUAL":
                    StoreProcedureName = "Proc_GetTaxDeclaration_ExportToExcel";
                    break;

                case "PREVIOUS EMPLOYMENT TAX DETAILS":
                    StoreProcedureName = "Proc_GetAllPreviousEmploymentDetails_ExportToExcel";
                    break;

                case "OTHER INCOME":
                    StoreProcedureName = "Proc_GetOtherIncomeData_ExportToExcel";
                    break;

                case "IT ADJUSTMENT":
                    StoreProcedureName = "Proc_GetAllITAdjustment_ExportToExcel";
                    break;

                case "ALLOW REPROCESS":
                    StoreProcedureName = "Proc_GetLockedPayPeriodsForReprocessing_ExportToExcel";
                    break;

                case "BANK CULTURE":
                    StoreProcedureName = "Proc_Batchgeneration";
                    break;

                case "SALARY_PROCESS_INITIATION":
                    StoreProcedureName = "Search_EditSalaryProcessInitiation_Data_ExportToExcel";
                    break;

                case "BANK_NEFTCULTURE":
                    StoreProcedureName = "Search_EditNeftBankculture_ExptToExcel";
                    break;

                case "BANK_NEFTCULTURE_INVOICE":
                    StoreProcedureName = "Search_EditInvoiceNeftBankculture_ExptToExcel";
                    break;

                case "NONINVOIVOICESALARYRELEASE":
                    //StoreProcedureName = "Proc_GetBandDetails_ExportToExcel";
                    StoreProcedureName = "sp_Salary_Release_Process";
                    break;

                case "BANK ADVICE SPLIT CULTURE":
                    StoreProcedureName = "Proc_Batchgeneration";
                    break;

                case "COMPANY PERMISSION":
                    StoreProcedureName = "Sp_GetAllCompanyPermission_Export2Excel";
                    break;

                case "VENDOR CUSTOMER MAPPING":
                    StoreProcedureName = "Sp_GetAllVendorCustomerMapping_Export2Excel";
                    break;
                    

                case "HOLD EMPLOYEE SALARY":
                    StoreProcedureName = "sp_GetAllEmployeeSalaryDetails";
                    break;

                case "INVOICE_HOLD_EMPLOYEE_SALARY":
                    StoreProcedureName = "sp_BankInvoiceHoldEmpSalaryDetails_ExportToExcel";
                    break;

                case "INVOICE_SALARY_RELEASE_REQUEST":
                    StoreProcedureName = "getInvoicenoForSalaryreleaserequest_Export";
                    break;

                case "ALL BANK ADVICE SPLIT CULTURE":
                    StoreProcedureName = "Invoice_SplitCulture_AllExport_To_Excel";
                    break;

                case "INVOICE_ADVICE_SPLIT_CULTURE":
                    StoreProcedureName = "Invoice_SplitCulture_AllExport_To_Excel";
                    break;

                case "UN PAID INVOICE":
                    StoreProcedureName = "Proc_UnPaidInvoiceReport";
                    break;

                case "CLIENT LEDGER":
                    StoreProcedureName = "Sp_ClientLedger_Report";
                    break;

                case "CREDIT NOTE REQUEST":
                    StoreProcedureName = "sp_SearchCreditNoteDetail_ExportToExcel";
                    break;

                case "CREDIT NOTE APPROVE":
                    StoreProcedureName = "sp_BlankSearchCreditNoteDetailExporttoExcel";
                    break;

                case "INVOICE BATCH CREATION":
                    StoreProcedureName = "Sp_rpt_tblBankInvoiceBatchCreation_Export";
                    break;

                case "INVOICE_BANK_ADVICE_SPLIT_CULTURE":
                    StoreProcedureName = "Invoice_SplitCulture_Export_To_Excel";
                    break;

                case "FORECAST":
                    StoreProcedureName = "Search_Forecast_ExptToExcel";
                    break;

                case "INVOIVOICESALARYRELEASE":
                    StoreProcedureName = "sp_Salary_Release_Process_Invoice";
                    break;

                case "DAILY COLLECTION INVOICE":
                    StoreProcedureName = "sp_BankInvoice_Daily_Collection_Report";
                    break;

                case "INVOICE COLLECTION PENDING":
                    StoreProcedureName = "sp_BankInvoice_CollectionPending_Report";
                    break;

                case "DSO":
                    StoreProcedureName = "sp_BankInvoice_DSO_Report";
                    break;

                case "PROJECTION SUMMARY":
                    StoreProcedureName = "Sp_ProjectionSummaryReport";
                    break;

                case "REIMBURSEMENTCALENDER":
                    StoreProcedureName = "sp_GetAllReimbursementCalenderExporttoExcel";
                    break;

                case "PROVISIONAL INVOICE REPORT":
                    StoreProcedureName = "Sp_ProvisionalInvoiceReport";
                    break;

                case "EMPLOYEE_EXPORT":
                    StoreProcedureName = "sp_GetAllEmployeeDetailsExportToExcel";
                    break;

                case "FLEXI_FORMULA_REPORT":
                    StoreProcedureName = "Sp_GetAllFlexiFormulaExportToExcel";
                    break;


                case "MAP_WISE_PAYREGISTER":
                    StoreProcedureName = "sp_PayRegister_MAP_Wise";
                    break;

                case "CREDIT NOTE UPDATE REQUEST":// By Anant on 8-Aug-18 for CreditNoteUpdate  ExportToExcel
                    StoreProcedureName = "sp_SearchCreditNoteUpdateDetail_ExportToExcel";
                    break;

                case "SALARYRELEASEAPPROVEEXPORT":// By Anant on 31-Aug-18 for Salary Release Approve  ExportToExcel
                    StoreProcedureName = "sp_SalaryReleaseApprove_ExportToExcel";
                    break;

                case "SALARYRELPENDINGAPPROVEEXPORT":
                    StoreProcedureName = "sp_SalaryReleasePendingApprove_ExportToExcel";
                    break;

                case "HOLDRELEASEDSALARYREORT":// By Anant on 04-Sep-18 for Hold/Released SalaryReort  ExportToExcel
                    StoreProcedureName = "Proc_HoldReleasedSalaryReort";
                    break;

                case "PROCNETPAYSUMMARY":// By Anant on 10-Oct-18 for NetPay Summary   ExportToExcel
                    StoreProcedureName = "Proc_NetPaySummary";
                    break;

                case "COMPANYGROUP":// By Santosh on 16-Oct-18 for Export CompanyGroup   ExportToExcel
                    StoreProcedureName = "sp_ExportCompanyGroup";
                    break;
                case "PARTIALINVOIVOICESALARYRELEASE":// By Anant on 25-Oct-18 for Partial Salary Release Process   
                    StoreProcedureName = "sp_Partial_Salary_Release_Process_Invoice";
                    break;

                case "CREDITNOTEBALANCE":// By Ajit on 12-Nov-18 for Credit Note Balance Report
                    StoreProcedureName = "sp_CreditNoteBalanceReport";
                    break;

                case "ARREARATTENDANCE":// By Ajit on 12-Nov-18 for Credit Note Balance Report
                    StoreProcedureName = "Sp_getArrearAttendanceDataExporttoexcel";
                    break;

                case "BONUSACCUMATED":// By Vijay on 27-Nov-18 for Bonus Accumated Report
                    StoreProcedureName = "Proc_Bonus_Accumated_Report";
                    break;
                case "NIBONUSACCUMATED":// By Anant on 12-July-19 for NON INVOICE Bonus Accumated Report
                    StoreProcedureName = "Proc_NIBonus_Accumated_Report";
                    break;
                case "BONUSINVOIVOICESALARYRELEASE":// By Anant on 27-Nov-18 for Bonus Salary Release Process   
                    StoreProcedureName = "sp_Bonus_Salary_Release_Process_Invoice";
                    break;

                case "REGULARRELEASEDSALARYREORT":
                    StoreProcedureName = "Proc_RegularReleaseSalaryReport";
                    break;
                case "PARTIALNONINVOIVOICESALARYRELEASE":
                    //StoreProcedureName = "Proc_GetBandDetails_ExportToExcel";
                    StoreProcedureName = "sp_Salary_Release_ProcessNIPartial";
                    break;
                case "BONUSNONINVOIVOICESALARYRELEASE":// By Anant on 12-July-19 for NON INVOICE BONUS Salary Release Process   
                    StoreProcedureName = "sp_Salary_Release_ProcessNIBonus";
                    break;

                case "SUMMARY REPORT":// By Rudra on 23-April-19 for uangeneration report   
                    StoreProcedureName = "Proc_UANGeneration_Report";
                    break;

                case "UANGENERATIONREPORTNOTEPAD":// By Rudra on 23-April-19 for uangeneration note report   
                    StoreProcedureName = "Proc_UANGeneration_Report_Notepad";
                    break;
                case "COMPANY VERTICALS":
                    StoreProcedureName = "sp_GetCompanyVertical_ExportToExcel";
                    break;
                case "MODIFIEDINVOICEREPORT":
                    StoreProcedureName = "proc_GstinvoiceReportAutomation";
					break;
                case "SALARY ADVANCE REQUEST":// By Rudra on 14-April-21 for salary advance report   
                    StoreProcedureName = "Proc_SearchSalarydvanceRequest_exporttoeexcel";
                    break;

                case "SALARY ADVANCE APPROVE":// By Rudra on 18-April-21 for salary advance report   
                    StoreProcedureName = "Proc_SearchSalarydvanceApprove_exporttoeexcel";
                    break;

                case "PAY REGISTER UPLOAD":// By Rudra on 26-May-22 for Pay Register Upload   
                    StoreProcedureName = "sp_PayregisterUploadFormat";
                    break;

                case "PAY REGISTER DETAILS":// By Rudra on 26-May-22 for Pay Register Upload   
                    StoreProcedureName = "sp_PayregisteruploadexporttoExcel";
                    break;

                case "PDC UPLOAD":// By Rudra on 26-May-22 for Pay Register Upload   
                    StoreProcedureName = "Proc_SearchPDCUploadData_export";
                    break;
                case "BATCHCONSOLIDATIONREPORT":// By Praveen on 20-jan-23 for Batch Consolidation Report   
                    StoreProcedureName = "Proc_BatchConsolidationReport";
                    break;
                case "DEDUCTIONSALARYRELEASE":// By Praveen on 05-jun-23 for Invoice Deduction    
                    StoreProcedureName = "sp_Deduction_Salary_Release_Process_Invoice";
                    break;
                case "DEDUCTIONNISALARYRELEASE":// By Praveen on 05-jun-23 for Non Invoice Deduction  
                    StoreProcedureName = "sp_Deduction_Salary_Release_Process_Non_Invoice";
                    break;
                case "DEDUCTION FLASH OUT":// By Rudra on 16-JUNE-23 for DEDUCTION Upload   
                    StoreProcedureName = "Proc_SearchDeductionFlashOut_Export";
                    break;
                case "DBTINVOIVOICESALARYRELEASE":// By PRAVEEN on 26-MAR-24 for DBT Salary Release Process   
                    StoreProcedureName = "sp_DBT_Salary_Release_Process_Invoice";
                    break;

                default:
                    StoreProcedureName = "OneTimeReplacement";
                    break;
            }
            return StoreProcedureName;
        }

        #endregion 19/09/2016 Jagannath changes

        public static bool CreatingZip(List<string> filesToArchive, string zipName)
        {
            try
            {
                FileInfo f = new System.IO.FileInfo(zipName);
                if (f.Exists) f.Delete();

                using (ZipArchive newFile = ZipFile.Open(zipName, ZipArchiveMode.Create))
                {
                    foreach (string file in filesToArchive)
                    {
                        //Adds the file to the archive
                        newFile.CreateEntryFromFile(file, (new FileInfo(file)).FullName, System.IO.Compression.CompressionLevel.Optimal);
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLogException.ErrorLog().LogException("CreatingZip", "Common", ex.Message);
                return false;
            }
            return true;
        }

        //public static void CommonUpload(ref CommonUploadModel commonModel, HttpSessionStateBase session)
        //{
        //    using (StringWriter ms = new StringWriter())
        //    {
        //        string errroMsg = string.Empty;
        //        string strXml = ReadExcelFile.ReadFileFromExcel(commonModel.fileLocation, out errroMsg, commonModel.UploadFileName);
        //        StreamWriter objStreamwriter = null;
        //        StringBuilder objStringBuilder = new StringBuilder();
        //        commonModel.UploadObj.XML_File = strXml;
        //        string Msg = string.Empty;
        //        string path = Path.Combine(commonModel.DirPath, "Error" + ".txt");

        //        if (!Directory.Exists(commonModel.DirPath))
        //        {
        //            Directory.CreateDirectory(commonModel.DirPath);
        //        }

        //        if (strXml == "Columns Name are not matching")
        //        {
        //            Msg = "Please select a valid template";
        //            objStringBuilder.AppendLine(Msg);
        //            objStringBuilder.AppendLine(errroMsg);
        //            objStreamwriter = new StreamWriter(path, false);
        //            objStreamwriter.WriteLine(objStringBuilder);
        //            Console.WriteLine("----------------------------------------------------");
        //            objStreamwriter.Close();
        //            commonModel.RedirectToAction = commonModel.RedirectOnFailure;
        //        }
        //        else
        //        {
        //            commonModel.CreatedBy = Convert.ToInt32(session[Session_Constants.Session_Constants_UserId]);
        //            Type thisType = commonModel.BusinesObj.GetType();
        //            MethodInfo theMethod=thisType.GetMethod(commonModel.ImportMethod);
        //            //commonModel.UploadObj.DBMessage
        //            commonModel.DBMessage = theMethod.Invoke(commonModel.BusinesObj, new object[] { commonModel.UploadObj, commonModel.SpName});
        //            //commonModel.UploadObj.DBMessage = commonModel.BusinesObj.Attendance_FileUpload(commonModel.UploadObj);

        //            for (int i = 0; i < commonModel.DBMessage.Count; i++)
        //            {
        //                Msg += commonModel.DBMessage[i].Error_Message + "|";

        //                if (!(Msg.Contains("Successfully")))
        //                {
        //                    objStringBuilder.Append(commonModel.DBMessage[i].Error_Message);
        //                    objStringBuilder.Append(i == commonModel.DBMessage.Count - 1 ? "\n" : ",");
        //                }
        //                else
        //                {
        //                    commonModel.Messages = Msg;
        //                    commonModel.RedirectToAction = commonModel.RedirectOnSuccess;
        //                }
        //                objStringBuilder.AppendLine();
        //            }
        //            objStreamwriter = new StreamWriter(path, false);
        //            objStreamwriter.Write(objStringBuilder);
        //            Console.WriteLine("----------------------------------------------------");
        //            objStreamwriter.Close();
        //        }
        //    }
        //}



        #region MyRegion

        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);

                // return stringwriter.ToString();
                return RemoveXmlDefinition(stringwriter.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RemoveXmlDefinition(string xml)
        {
            XDocument xdoc = XDocument.Parse(xml);
            xdoc.Declaration = null;

            return xdoc.ToString();
        }


        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
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

        #endregion

        #region GetZipFile Folder --- NKK -----
        /// <summary>
        /// Get ZipFile Path from Web Config File
        /// </summary>
        /// <returns></returns>
        //public static string AddFiletoZipFolder()
        //{
        //    string UserId = HttpContext.Current.Session["Userid"].ToString();
        //    string serverPath = ConfigurationManager.AppSettings["PDFBulkDownLoad"].ToString() + "\\";
        //    string DirectoryName = serverPath + UserId;
        //    string MyFile = serverPath + UserId + ".zip";
        //    if (System.IO.File.Exists(MyFile))
        //        System.IO.File.Delete(MyFile);
        //    ZipFile.CreateFromDirectory(DirectoryName, MyFile);
        //    return MyFile;
        //}
        #endregion
    }
}