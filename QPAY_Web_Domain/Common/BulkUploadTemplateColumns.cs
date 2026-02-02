namespace QPay.UI.Common
{
    public class BulkUploadTemplateColumns
    {
        public static string[] GetBulkUploadIncrementColumns()
        {
            string[] BulkUploadIncrementColumns = { "COMPANYCODE", "EMPLOYEECODE", "BAND", "NEWCTC", "EFFECTIVEDATE", "ENDDATE", "PAYSEQUENCENO", "PAYCODE", "AMOUNT", "ARREARFLAG" };
            return BulkUploadIncrementColumns;
        }

        public static string[] GetBulkUploadLOPAdjustmentColumns()
        {
            string[] BulkUploadLOPAdjustmentColumns = { "COMPCODE", "PAYSEQUENCENO", "EMPCODE", "LOP/LOPR Payseqno", "LOP", "LOPR" };
            return BulkUploadLOPAdjustmentColumns;
        }

        public static string[] GetBulkUploadAttendanceColumns()
        {
            string[] BulkUploadAttendanceColumns = { "COMPCODE", "PAY SEQNO", "EMPID", "LOP DAYS", "ACTION" };
            return BulkUploadAttendanceColumns;
        }

        public static string[] GetBulkUploadTransferAttendanceColumns()
        {
            string[] BulkUploadTransAttendanceColumns = { "COMPCODE", "PAY SEQNO", "EMPID", "LOP DAYS", "ACTION", "MAPNAME", "GROUPNAME" };
            return BulkUploadTransAttendanceColumns;
        }

        public static string[] GetBulkUploadAttendance_MagnaColumns()
        {
            string[] BulkUploadAttendanceColumns = { "COMPCODE", "PAY SEQNO", "EMPID", "Leave_Availed", "ACTION","BATCHID" };
            return BulkUploadAttendanceColumns;
        }

        public static string[] GetBulkUploadOneTimeReplacementColumnsWithoutArrear()
        {
            string[] BulkUploadAttendanceColumns = { "Compcode", "Empcode", "Band", "Payseqno", "Paycode", "Amount", "ModeofEntry", "Type", "Pay_Type" };
            return BulkUploadAttendanceColumns;
        }

        public static string[] GetBulkUploadOneTimeReplacementColumnsWithArrear()
        {
            string[] BulkUploadAttendanceColumns = { "Compcode", "Empcode", "Band", "Payseqno", "Paycode", "Amount", "ModeofEntry", "Type", "ArrearPayseqno", "Pay_Type", "Remarks" };
            return BulkUploadAttendanceColumns;
        }

        public static string[] GetBulkUploadTaxDeclarationColumns()
        {
            string[] BulkUploadTaxDeclarationColumns = { "COMPID", "EMPID", "TAXCODE", "AMOUNT", "TYPE", "FINYEAR", "NO_OF_CHILDREN" };
            return BulkUploadTaxDeclarationColumns;
        }

        public static string[] GetBulkUploadPayTransactionColumns()
        {
            string[] BulkUploadPayTransactionColumns = { "COMP CODE", "PAY SEQUENCE NO", "PAY CODE", "BAND", "EMP ID", "AMOUNT","REMARKS" };
            return BulkUploadPayTransactionColumns;
        }

        public static string[] GetBulkUploadReimbursementColumns()
        {
            //string[] BulkUploadPayTransactionColumns = { "CompanyCode", "EmployeeCode", "FinancialYear", "PayPeriod", "ReimbursementCode", "ClaimAmount" };
            string[] BulkUploadPayTransactionColumns = { "CompanyCode", "EmployeeCode", "FinancialYear", "PayPeriod", "ReimbursementCode", "ClaimAmount", "Action", "ReimbursementDate" };
            return BulkUploadPayTransactionColumns;
        }

        public static string[] GetBulkUploadEmployeeColumns()
        {
            string[] GetBulkUploadEmployeeColumns = { "COMP ID", "NAME ", "FATHER NAME", "GENDER", "DOJ", "DOB", "MARITAL", "DEPARTMENT", "DESIGNATION", "PAY CATEGORY", "PT STATE", "PAN", "BANK NAME", "A/C NO", "WORK LOCATION", "EMAIL", "DATE OF JOIN PAY PERIOD", "IFSC CODE", "HIRING STATUS", "IKYA LOCATION", "MAP NAME", "RECRUITER'S NAME", "MOB. NO", "ESI NUMBER", "ENTITY LOCATION", "CAP/NON CAP", "COST CENTRE", "GROUP NAME" };
            return GetBulkUploadEmployeeColumns;
        }

        public static string[] GetBulkUploadSalaryColumns()
        {
           // string[] GetBulkUploadSalaryColumns = { "COMPCODE", "EMPCODE ", "BAND ", "EFFDATE", "PAYCODE", "AMOUNT", "PAYSEQUENCENO" };
            string[] GetBulkUploadSalaryColumns = { "COMPCODE", "EMPCODE ", "BAND ", "PAYCODE", "AMOUNT", "PAYSEQUENCENO" };
            return GetBulkUploadSalaryColumns;
        }

        public static string[] GetBulkUploadLoanAndAdvanceColumns()
        {
            string[] GetBulkUploadLoanAndAdvanceColumns = { "EMPCode", "LOANAMT", "LOANDATE", "NumberOfInstallments", "LoanPayCodeType", "loantopuplotnumber" };
            return GetBulkUploadLoanAndAdvanceColumns;
        }

        //public static string[] GetBulkUploadHRAColumns()
        //{
        //    string[] BulkUploadHRAColumns = { "COMPCODE", "EMPCODE", "RESIDINGLOCATION", "METRO", "RENT", "FINYEAR" };
        //    return BulkUploadHRAColumns;
        //}
        public static string[] GetBulkUploadHRAColumns()
        {
            string[] BulkUploadHRAColumns = { "COMP CODE", "EMP CODE", "RESIDINGLOCATION", "METRO", "FROM DATE", "TO DATE", "RENT", "STATUS" };
            return BulkUploadHRAColumns;
        }

        public static string[] GetBulkUploadAttendance_1Columns()//For IAM00014
        {
            string[] BulkUploadAttendanceColumns = { "COMPCODE", "PAY  SEQNO", "EMPID", "LOP DAYS", "ACTION", "MONTH_DAYS", "PRESENT_DAYS" };
            return BulkUploadAttendanceColumns;
        }

        public static string[] GetBulkUploadLOtherIncome_With_TDS_ESI_ERESI_Columns()
        {
            string[] BulkUploadLOPAdjustmentColumns = { "Company_Code", "Employee_Code", "Pay_Sequence_No", "Incentive_Paid_Pay_Sequence_No", "Pay_Code", "Input_No", "Amount", "Remarks", "Reason", "ESI", "ERESI", "TDS", "Map_Name", "Other_Deduction" };
            return BulkUploadLOPAdjustmentColumns;
        }

        public static string[] GetBulkUploadLOtherIncome_With_OUT_TDS_ESI_ERESI_Columns()
        {
            string[] BulkUploadLOPAdjustmentColumns = { "Company_Code", "Employee_Code", "Pay_Sequence_No", "Incentive_Paid_Pay_Sequence_No", "Pay_Code", "Input_No", "Amount", "Remarks", "Reason", "Map_Name", "Other_Deduction" };
            return BulkUploadLOPAdjustmentColumns;
        }


        public static string[] GetBulkUploadEmployeeUpload_Columns()
        {

            // string[] BulkUploadEmployeeUploadColumns = { "COMP ID", "NAME", "FATHER NAME", "GENDER", "DOJ", "DOB", "MARITAL", "DEPARTMENT", "DESIGNATION", "PAY CATEGORY", "PT STATE", "PAN", "BANK NAME", "A/C NO", "WORK LOCATION", "EMAIL", "DATE OF JOIN PAY PERIOD", "IFSC CODE", "HIRING STATUS", "IKYA LOCATION", "MAP NAME", "RECRUITER'S NAME", "MOB# NO", "ESI NUMBER", "ENTITY LOCATION", "CAP/NON CAP", "COST CENTRE", "GROUP NAME", "AXPERT_EMPLOYEE_ID", "EMPLOYMENT_TYPE", "OMS_ID", "DMS_ID", "Aadhar_Number", "UAN_Number", "PT_CIRCLE_NAME", "VPF_Applicable", "VPF_Type", "VPF_Amount", "TaxRegime", "NRIC_FIN_NUMBER", "FUND_LEVY", "RACE_CODE", "NATIONAL_CODE", "Branch_code", "IFSC_Code", "LEAVE_SCHEME" };
            string[] BulkUploadEmployeeUploadColumns = { "COMPID", "NAME", "FATHERNAME", "GENDER", "DOJ", "DOB", "MARITAL", "DEPARTMENT", "DESIGNATION", "OLDEMPLOYEECODE", "PAY CATEGORY", "BANK NAME", "A/C NO", "EMAIL", "DATE OF JOIN PAY PERIOD", "SWIFTCODE", "BRANCH", "BRANCHCODE", "BANKCODE", "HIRING STATUS", "MAP NAME", "RECRUITER'S NAME", "MOBNO", "ENTITY LOCATION", "COST CENTRE", "GROUP NAME", "EMPLOYMENT_TYPE", "OMS_ID", "DMS_ID", "NRIC_FIN_NUMBER", "FUND_LEVY", "RACE_CODE", "NATIONAL_CODE", "LEAVE_SCHEME", "RELIGION", "WORK_PASS", "SPR_STATUS", "SPR_APPROVE_DATE", "VISA_NUMBER", "VISA_DURATION_START_DATE", "VISA_DURATION_END_DATE", "RFUND_CODE1", "RFUND_CODE2", "COUNTRY_OF_BIRTH", "PASSPORT_NUMBER", "PASSPORT_EXPIRY_DATE", "ADDRESS", "PIN_CODE" };
            return BulkUploadEmployeeUploadColumns;
        }


        public static string[] GetBulkUploadConsultantEmployeeUpload_Columns()
        {

            string[] BulkUploadEmployeeUploadColumns = { "COMP ID", "NAME", "FATHER NAME", "GENDER", "DOJ", "DOB", "MARITAL", "DEPARTMENT", "DESIGNATION", "PAY CATEGORY", "PT STATE", "PAN", "BANK NAME", "A/C NO", "WORK LOCATION", "EMAIL", "DATE OF JOIN PAY PERIOD", "IFSC CODE", "HIRING STATUS", "IKYA LOCATION", "MAP NAME", "RECRUITER'S NAME", "MOB# NO", "ESI NUMBER", "ENTITY LOCATION", "CAP/NON CAP", "COST CENTRE", "GROUP NAME", "AXPERT_EMPLOYEE_ID", "EMPLOYMENT_TYPE", "OMS_ID", "DMS_ID", "Aadhar_Number", "UAN_Number","Duplicate_Allow_Reason" };
            return BulkUploadEmployeeUploadColumns;
        }

        public static string[] GetBulkUpload_MagnaPayroll()
        {

            string[] BulkUploadMagnaPayrollColumns = { "COMPCODE", "EMPID", "Leave_Opening_Balance", "ACTION" };
            return BulkUploadMagnaPayrollColumns;
        }

        public static string[] GetBulkUpload_SalaryReleasePendingApprove()
        {
            string[] BulkUploadSalaryRelPendingApproveColumns = {"InvoiceNumber","Status","Remarks"};
            return BulkUploadSalaryRelPendingApproveColumns;
        }
            public static string[] GetBulkUpload_AccountNumberRequest()
        {
            string[] BulkUploadAccountNumberRequestColumns = { "InvoiceNumber", "EmployeeCode" };
            return BulkUploadAccountNumberRequestColumns;
        }

        // start by Anant on 17-Oct-18 for PartialHold Salary Release
        public static string[] GetBulkUpload_PartialHoldSalaryRelease()
        {
            string[] BulkUploadAccountNumberRequestColumns = { "InvoiceNumber", "EmployeeCode", "PartialReleaseAmount", "SalaryType" };
            return BulkUploadAccountNumberRequestColumns;
        }
        // End by Anant on 17-Oct-18 for PartialHold Salary Release

        // Start by Anant on 26-Nov-18 for Bonus_Release
        public static string[] GetBulkUpload_BonusRelease()
        {
            string[] BulkUploadAccountNumberRequestColumns = { "InvoiceNumber", "EmployeeCode"};
            return BulkUploadAccountNumberRequestColumns;
        }
        // End by Anant on 26-Nov-18 for Bonus_Release

        public static string[] GetBulkUpload_CancelInvoiceMapping()
        {
            string[] BulkUploadCancelInvoiceMappingColumns = { "Invoice_No", "Employee_code", "New_Invoice_No" };
            return BulkUploadCancelInvoiceMappingColumns;
        }

        public static string[] GetBulkUpload_BankInvoiceManualUpload()
        {
            string[] BulkUploadBankInvoiceManualUploadColumns = { "Company_Code", "Invoice_No", "Employee_Code", "Pay_Period", "Bank_Name", "Account_No", "IFSC_Code", "Net_Pay", "CTC", "Data_From", "Input_no" };
            return BulkUploadBankInvoiceManualUploadColumns;
        }

        public static string[] GetBulkUpload_CompanyProvidedUpload()
        {
            string[] BulkUploadCompanyProvidedUploadColumns = { "EmployeeCode", "FinancialYear", "Date", "PerkCode", "PerkAmount","Mode" };
            return BulkUploadCompanyProvidedUploadColumns;
        }
        public static string[] GetBulkUpload_ManualBatchCreation()
        {
            string[] BulkUploadManualBatchCreationColumns = { "EmployeeCode", "InvoiceNumber", "SalaryReleaseDate" };
            return BulkUploadManualBatchCreationColumns;
        }

        public static string[] GetBulkUpload_BatchRejection()
        {
            string[] BulkUploadBatchRejectionColumns = { "InvoiceNumber" };
            return BulkUploadBatchRejectionColumns;
        }

        //Added By Vijay on 22Feb2019 for Tax Remittance Upload
        public static string[] GetBulkUpload_TaxRemittanceColumns()
        {
            string[] GetBulkUpload_TaxRemittanceColumns = { "CompanyCode", "EmployeeCode", "PayPeriod", "TaxableIncome", "TDS", "SurCharge", "EduCess", "Interest", "Others" };
            return GetBulkUpload_TaxRemittanceColumns;
        }
        //Added By Vijay on 28Feb2019 for Bad Debt Update
        public static string[] GetBulkUpload_BadDebtUpdateColumns()
        {
            string[] GetBulkUpload_BadDebtUpdateColumns = { "InvoiceNumber", "BadDebtAmount", "Remarks", "ApprovedBy" };
            return GetBulkUpload_BadDebtUpdateColumns;
        }
        //Added By Vijay on 3Mar2019
        public static string[] GetBulkUpload_BeneficiaryUpdateColumns()
        {
            string[] GetBulkUpload_BeneficiaryUpdateColumns = { "EmployeeCode" };
            return GetBulkUpload_BeneficiaryUpdateColumns;
        }

        public static string[] GetBulkUpload_LoanPreClosure()
        {
            string[] BulkUploadAccountNumberRequestColumns = { "Company_Code", "Employee_Code", "PreClosure_Date", "Loan_Number", "Adjustable", "PreClosed_Amount" };
            return BulkUploadAccountNumberRequestColumns;
        }

        //Added By Rudra on 05 May 2019
        public static string[] GetBulkUpload_PayfrequencygroupColumns()
        {
            string[] GetBulkUpload_PayfrequencygroupColumns = { "Company Code", "Group", "Starting Date", "Ending Date", "Pay Sequence No", "Pay Period", "Start At", "End At", "Weekly Holiday" };
            return GetBulkUpload_PayfrequencygroupColumns;
        }

        // Method for uploading MIS accrual details. Added By Vijay on 11/June/2019
        public static string[] GetUploadAccrualsColumns()
        {
            string[] GetUploadAccrualsColumns = { "CompanyCode", "Location", "PayPeriod", "HeadCount", "CTC", "AdditionalCTCAmount", "ServiceCharge", "AdditionalServiceCharge", "SourcingFee", "AbsorptionFee", "OnboardingCharge", "InedgeCharge", "Mode" };
            return GetUploadAccrualsColumns;
        }

        public static string[] GetBulkUpload_MultipleClientInvoiceMapping()
        {
            string[] BulkUploadMultipleClientInvoiceMappingColumns = { "Invoice_No", "Employee_code", "New_Invoice_No" };
            return BulkUploadMultipleClientInvoiceMappingColumns;
        }

        public static string[] GetBulkUpload_EmployeeTransferRequest()
        {
            string[] BulkUploadEmployeeTransferRequestColumns = { "CompanyCode", "PayPeriod", "EmployeeCode", "TransferType", "TransferCompanyCode", "TransferGroup", "TransferMapName", "TransferDepartment", "TransferDesignation", "EffectiveFrom", "EffectiveTo", "TransferBand", "TransferLocation", "TransferState", "Mode" };
            return BulkUploadEmployeeTransferRequestColumns;
        }
        public static string[] GetBulkUpload_EmployeeTransferApproval()
        {
            string[] BulkUploadEmployeeTransferApprovalColumns = { "CompanyCode", "EmployeeCode", "PayPeriod", "TransferCompanyCode", "TransferGroup", "TransferMap", "TransferBand", "TransferLocation", "TransferState", "Status", "Remarks" };
            return BulkUploadEmployeeTransferApprovalColumns;
        }

        
         public static string[] GetBulkUpload_BNIFinanceHoldTemplate()
        {
            string[] BulkUploadFinanceHoldColumns = { "COMPANY_CODE", "GROUP_NAME", "PAY_PERIOD" };
            return BulkUploadFinanceHoldColumns;
        }

        //GetBulkUpload_BNIFinanceReleaseTemplate
        public static string[] GetBulkUpload_BNIFinanceReleaseTemplate()
        {
            string[] BulkUploadFinanceReleaseColumns = { "COMPANY_CODE", "GROUP_NAME", "PAY_PERIOD", "PURPOSE"};
            return BulkUploadFinanceReleaseColumns;
        }

        public static string[] GetBulkUpload_CreditNoteAdjustment()
        {
            string[] BulkUploadMultipleClientInvoiceMappingColumns = { "CreditNoteNumber", "AdjustedAmount" };
            return BulkUploadMultipleClientInvoiceMappingColumns;
        }
        public static string[] GetBulkUploadVendorEmployeeUpload_Columns()
        {

            string[] BulkUploadEmployeeUploadColumns = { "EMPLOYEE_CODE","EMPLOYEE_NAME","CLIENT_NAME","COMPANY_CODE","MAP_NAME","UNIQUE_TYPE","EACTIVE","DATE_JOINED","VENDOR","DOS","VERTICAL","PO_VALUE","PO_START_DATE","PO_END_DATE","REQUISITIONER_NAME","SALARY","MOBILE_NUMBER","PAN_NUMBER","EMAIL_ID", "FULL_ADDRESS", "STATE_NAME", "GROUP_NAME" };
            return BulkUploadEmployeeUploadColumns;
        }
        public static string[] GetBulkUpload_MapNameChanges()
        {
            string[] BulkUploadFinanceReleaseColumns = { "COMPANY_CODE", "EMPLOYEE_CODE", "PAY_PERIOD", "ACTION" };
            return BulkUploadFinanceReleaseColumns;
        }
        public static string[] GetBulkUpload_OtherincomeMapNameChanges()
        {
            string[] BulkUploadFinanceReleaseColumns = { "COMPANY_CODE", "EMPLOYEE_CODE", "PAY_PERIOD","MAP_NAME", "INPUT_NO", "ACTION" };
            return BulkUploadFinanceReleaseColumns;
        }
        public static string[] GetBulkUpload_AttendanceLeaveUpdate()
        {
            string[] BulkUploadFinanceReleaseColumns = { "EMPLOYEE_CODE", "PAY_PERIOD", "LEAVE_OPENING_BALANCE", "LEAVE_CREDIT" , "LEAVE_CLOSING_BALANCE", "ACTUAL_LEAVE_CLOSING_BALANCE", "LEAVE_AVAILED" };
            return BulkUploadFinanceReleaseColumns;
        }
        public static string[] GetBulkUpload_PfCodeCulture()
        {
            string[] BulkUploadPfCodeCultureColumns = { "Entity", "CultureType", "PfCode", "CompanyCode", "ECRFileName", "Mode" };
            return BulkUploadPfCodeCultureColumns;
        }
        //Rudra
        public static string[] GetBulkUpload_UanActiveStatus()
        {
            string[] BulkUploadMultipleClientInvoiceMappingColumns = { "EmployeeCode", "UanActiveStatus", "UanActiveStatusRemarks" };
            return BulkUploadMultipleClientInvoiceMappingColumns;
        }
        public static string[] GetBulkUpload_ECRDataUpload()
        {
            string[] BulkUploadECRDataUploadColumns = { "CompanyCode", "PayPeriod", "EmployeeCode" };
            return BulkUploadECRDataUploadColumns;
        }
        public static string[] GetBulkUpload_PanRemarks()
        {
            string[] BulkUploadMultipleClientInvoiceMappingColumns = { "EmployeeCode", "PanNumberRemarks"};
            return BulkUploadMultipleClientInvoiceMappingColumns;
        }
        public static string[] GetBulkUpload_TiscActiveStatus()
        {
            string[] BulkUploadMultipleClientInvoiceMappingColumns = { "EmployeeCode", "TiscActiveStatus", "TiscActiveStatusRemarks" };
            return BulkUploadMultipleClientInvoiceMappingColumns;
        }
        public static string[] GetBulkUpload_Esisubcode()
        {
            string[] BulkUploadMultipleClientInvoiceMappingColumns = { "EmployeeCode","EsiSubCode" , "EsiSubCodeName" };
            return BulkUploadMultipleClientInvoiceMappingColumns;
        }
        public static string[] GetBulkUpload_EsisubcodeCodeCulture()
        {
            string[] BulkUploadPfCodeCultureColumns = { "Entity", "CultureType", "ESISUBCODE", "CompanyCode", "ECRFileName", "Mode" };
            return BulkUploadPfCodeCultureColumns;
        }
        public static string[] GetBulkUpload_ESIECRDataUpload()
        {
            string[] BulkUploadECRDataUploadColumns = { "CompanyCode", "EmployeeCode", "PayPeriod" };
            return BulkUploadECRDataUploadColumns;
        }
        public static string[] GetBulkUpload_PfEcrChallanUpload()
        {
            string[] BulkUploadPfEcrChallanUploadColumns = { "EmployeeCode", "ChallanName", "ChallanNumber", "PaymentDate", "DataType" };
            return BulkUploadPfEcrChallanUploadColumns;
        }
        public static string[] GetBulkUpload_AadhaarAuthenticateStatus()
        {
            string[] BulkUploadAadhaarAuthenticateStatusColumns = { "EmployeeCode", "AadhaarAuthenticateStatus", "Remarks" };
            return BulkUploadAadhaarAuthenticateStatusColumns;
        }
        public static string[] GetBulkUpload_SubmissionDateUpdate()
        {
            string[] BulkUploadSubmissionDateUpdateColumns = { "InvoiceNumber", "SubmissionDate" };
            return BulkUploadSubmissionDateUpdateColumns;
        }
        public static string[] GetBulkUpload_PoCultureDateUpdate()
        {
            string[] BulkUploadSubmissionDateUpdateColumns = { "CompanyCode","MapName","PurchaseRequestNo"};
            return BulkUploadSubmissionDateUpdateColumns;
        }
        public static string[] GetBulkUpload_BranchMaster()
        {
            string[] BulkUploadBranchMaster = { "PT_State", "Branch_Name" };
            return BulkUploadBranchMaster;
        }
        public static string[] GetBulkUpload_VendorVerticalUpdate()
        {
            string[] BulkUploadVendorVerticalUpdateColumns = { "CompanyCode", "MapName", "PurchaseRequestNo" };
            return BulkUploadVendorVerticalUpdateColumns;
        }
        public static string[] GetBulkUpload_WBSUpdation()
        {
            string[] Columns = { "EmployeeCode" };
            return Columns;
        }
        public static string[] GetBulkUpload_PayRegisterUploadDeletion()
        {
            string[] BulkUploadPayRegisterUploadDeletionColumns = { "Company_Code", "Pay_Period", "Employee_Code" };
            return BulkUploadPayRegisterUploadDeletionColumns;
        }
        public static string[] GetBulkUploadClraUpload_Columns()
        {

            string[] BulkUploadEmployeeUploadColumns = {"SERIAL_NO","REGION_NAME","ENTITY_NAME" ,"COMPANY_CODE" , "CUSTOMER_NAME", "CUSTOMER_LOCATION_ADDRESS" ,"LICENSE_NO" ,"ISSUING_AUTHORITY" ,"LICENSE_ISSUING_LOCATION" ,"NATURE_OF_WORK" ,"NO_OF_EMPLOYEES" ,"AMENDMENT_WITH_QUESS" ,"LOCAL_AUTHORITY"  ,"LOCATION_SPOC_EMAIL" ,"VALID_FROM" ,"VALID_TO","STATUS"  ,"REMARKS" ,"ADDITIONAL_REMARKS" ,"UPLOAD_LICENSE_FILE_NAME"     };
            return BulkUploadEmployeeUploadColumns;
        }
        public static string[] GetBulkUpload_ProfomaImport()
        {
            string[] BulkUploadProfomaImportColumns = { "Proforma_No", "Employee_Code", "Map_Name","Group_Name", "Batch_Id", "Action" };
            return BulkUploadProfomaImportColumns;
        }

        // satart by praveen kumar on 25-Mar-24 for DBT Hold Salary
        public static string[] GetBulkUpload_DBTHoldEmployeeSalary()
        {
            string[] BulkUploadAccountNumberRequestColumns = { "InvoiceNumber", "EmployeeCode", "HoldAmount", "SalaryType" };
            return BulkUploadAccountNumberRequestColumns;
        }
        // End by praveen kumar on 25-Mar-24 for DBT Hold Salary
        // satart by praveen kumar on 25-Mar-24 for DBT Hold Salary Release
        public static string[] GetBulkUpload_DBTHoldSalaryRelease()
        {
            string[] BulkUploadAccountNumberRequestColumns = { "InvoiceNumber", "EmployeeCode", "DBTReleaseAmount", "SalaryType" };
            return BulkUploadAccountNumberRequestColumns;
        }
        // End by praveen kumar on 25-Mar-24 for DBT Hold Salary Release

        // satart by praveen kumar on 25-Mar-24 for BlockCNAmountHold
        public static string[] GetBulkUpload_BlockCNAmountHold()
        {
            string[] BulkUploadAccountNumberRequestColumns = { "CompanyCode", "EmployeeCode", "InvoiceNumber", "CreditNoteNo", "Amount", "HoldStatus" };
            return BulkUploadAccountNumberRequestColumns;
        }
        // End by praveen kumar on 25-Mar-24 for BlockCNAmountHold
    }
}