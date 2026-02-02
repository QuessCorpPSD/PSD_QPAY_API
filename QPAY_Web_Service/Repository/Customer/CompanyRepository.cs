using QPay.BAL.IRepository.Customer;
using QPay.DAL.Repository;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static QPay.UI.Customer.Company;

namespace QPay.BAL.Repository.Customer
{
    public class CompanyRepository : ICompanyRepository
    {

        private readonly DbRepository _dbRepository;

        public CompanyRepository(DbRepository dbRepository)
        {
            this._dbRepository = dbRepository;
        }

        public async Task<DataTable> Search(string action, int? companyId, string xml)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@mode"] = action,
                ["@value1"] = companyId,
                ["@value2"] = xml,
            };
            return _dbRepository.ExecuteStoredProcedureToDataTableAsync("sp_GetCompanyDetailsData", parameters);

        }

        public async Task<DataSet> View(string action, int? companyId, string xml)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@mode"] = action,
                ["@value1"] = companyId,
                ["@value2"] = xml,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetCompanyDetailsData", parameters);

        }

        public async Task<DataSet> ExportToExcel(string action, int? companyId, string xml)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@mode"] = action,
                ["@value1"] = companyId,
                ["@value2"] = xml,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("Proc_GetCompanyDetailsDataExportToExcel", parameters);
        }



        public async Task<CompanyDetails> masters()
        {
            CompanyDetails obj = new CompanyDetails();
            obj.Getallbinddata();
            return _dbRepository.GetAllCompanyDefaultBindData();
        }




        public async Task<DataSet> GetBussinessunitLocation(int? BusinessUnitId)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@mode"] = 2,
                ["@Value"] = BusinessUnitId,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllCompanyDefaultBindData", parameters);
        }


        public async Task<DataSet> GetCityBasedonState(int? Stateid)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@mode"] = 3,
                ["@Value"] = Stateid,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllCompanyDefaultBindData", parameters);
        }

        public async Task<DataSet> GetStatebasedoncity(int? cityid)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@mode"] = 4,
                ["@Value"] = cityid,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_GetAllCompanyDefaultBindData", parameters);
        }

        public async Task<DataSet> GetInvoiceFormat()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@InvoiceType_Id"] = 0
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("GetAllInvoiceFormat", parameters);
        }

        public async Task<DataSet> GetReimbInvoiceFormat()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@InvoiceType_Id"] = 5
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("GetAllInvoiceFormat", parameters);
        }

        public async Task<DataSet> GetPortalPayslipFormat()
        {
            var parameters = new Dictionary<string, object>
            {
                ["@pageName"] = "Company"
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("GetPortalPayslipFormat", parameters);
        }


        //public async Task<List<CategoryUI>> GetCategory()
        //{
        //    var parameters = new DynamicParameters();

        //    var res = await this._dbRepository.GetItemsAsync("Proc_GetCategory_CPF", parameters);

        //    if (!string.IsNullOrEmpty(res))
        //    {
        //        return JsonConvert.DeserializeObject<List<CategoryUI>>(res) ?? new List<CategoryUI>();
        //    }

        //    return new List<CategoryUI>();
        //}


        public async Task<DataSet> Create(CompanyCreateRequest request)
        {
            var parentdatawrapper = new CompanyCreateRequestWrapper
            {
                Company = new CompanyRequest
                {
                    Client_Id = request.companyrequest.Client_Id,
                    Financial_Year_Id = request.companyrequest.Financial_Year_Id,
                    Client_Since = request.companyrequest.Client_Since,
                    Company_Active = request.companyrequest.Company_Active,
                    Is_Zip_Documents = request.companyrequest.Is_Zip_Documents,
                    Payroll_Type = request.companyrequest.Payroll_Type,
                    Invoicing_Type = request.companyrequest.Invoicing_Type,
                    Investment_Block_Date = request.companyrequest.Investment_Block_Date,
                    Business_Unit_Name_Id = request.companyrequest.Business_Unit_Name_Id,
                    Month_Days = request.companyrequest.Month_Days,
                    Salary_Fix_Days = request.companyrequest.Salary_Fix_Days,
                    Business_Unit_Location_Id = request.companyrequest.Business_Unit_Location_Id,
                    Attendance_Cycle_From = request.companyrequest.Attendance_Cycle_From,
                    Attendance_Cycle_To = request.companyrequest.Attendance_Cycle_To,
                    Is_PF_Remittance = request.companyrequest.Is_PF_Remittance,
                    Input_Date = request.companyrequest.Input_Date,
                    Output_Date = request.companyrequest.Output_Date,
                    Work_Days_Based_On = request.companyrequest.Work_Days_Based_On,
                    CTC = request.companyrequest.CTC,
                    Sourcing_Fee_Criteria_Type = request.companyrequest.Sourcing_Fee_Criteria_Type,
                    Sourcing_Fee = request.companyrequest.Sourcing_Fee,
                    Absorption_Fee_Criteria_Type = request.companyrequest.Absorption_Fee_Criteria_Type,
                    Absorption_Fee = request.companyrequest.Absorption_Fee,
                    Incentive_Type = request.companyrequest.Incentive_Type,
                    Is_PO_Applicable = request.companyrequest.Is_PO_Applicable,
                    Salary_SMS = request.companyrequest.Salary_SMS,
                    Dues_Based_On = request.companyrequest.Dues_Based_On,
                    Is_Insurance_Applicable = request.companyrequest.Is_Insurance_Applicable,
                    ReimbInvoiceFormat_Id = request.companyrequest.ReimbInvoiceFormat_Id,
                    Segment_Id = request.companyrequest.Segment_Id,
                    SubSegment_Id = request.companyrequest.SubSegment_Id,
                    Payslip_Format = request.companyrequest.Payslip_Format,
                    Mode_Of_Payment = request.companyrequest.Mode_Of_Payment,
                    TAT = request.companyrequest.TAT,
                    Billing_Type = request.companyrequest.Billing_Type,
                    Is_RoundOff_Applicable = request.companyrequest.Is_RoundOff_Applicable,
                    Deviation = request.companyrequest.Deviation,
                    Incharge = request.companyrequest.Incharge,
                    Credit_Days_Upfront = request.companyrequest.Credit_Days_Upfront,
                    Customer_Type = request.companyrequest.Customer_Type,
                    Incentive_Date = request.companyrequest.Incentive_Date,
                    Service_Tax_Applicable = request.companyrequest.Service_Tax_Applicable,
                    Reimbursement_Type = request.companyrequest.Reimbursement_Type,
                    Salary_Transfer_Date = request.companyrequest.Salary_Transfer_Date,
                    Effective_Date = request.companyrequest.Effective_Date,
                    Sales_Person = request.companyrequest.Sales_Person,
                    Branch_Location = request.companyrequest.Branch_Location,
                    Reimbursement_Date = request.companyrequest.Reimbursement_Date,
                    SAP_Code = request.companyrequest.SAP_Code,
                    Pin_Code = request.companyrequest.Pin_Code,
                    Address = request.companyrequest.Address,
                    Phone_Number = request.companyrequest.Phone_Number,
                    PAN_Number = request.companyrequest.PAN_Number,
                    TAN_Number = request.companyrequest.TAN_Number,
                    Service_Tax_Number = request.companyrequest.Service_Tax_Number,
                    PF_Code = request.companyrequest.PF_Code,
                    ESI_Code = request.companyrequest.ESI_Code,
                    PT_Code = request.companyrequest.PT_Code,
                    Email_Id = request.companyrequest.Email_Id,
                    Certificate_Number = request.companyrequest.Certificate_Number,
                    Fax_Number = request.companyrequest.Fax_Number,
                    Website_Name = request.companyrequest.Website_Name,
                    Wages = request.companyrequest.Wages,
                    Particulars = request.companyrequest.Particulars,
                    Is_NonInvoice = request.companyrequest.Is_NonInvoice,
                    Mis_Name = request.companyrequest.Mis_Name,
                    Zone_Tagging = request.companyrequest.Zone_Tagging,
                    IsHeaderFooter = request.companyrequest.IsHeaderFooter,
                    Sap_Customer_Code = request.companyrequest.Sap_Customer_Code,
                    Profit_Center_Code = request.companyrequest.Profit_Center_Code,
                    Inedge_charges = request.companyrequest.Inedge_charges,
                    Inedge_charges_Criteria_Type = request.companyrequest.Inedge_charges_Criteria_Type,
                    CompanyGroupCode = request.companyrequest.CompanyGroupCode,
                    OnBoarding_Category = request.companyrequest.OnBoarding_Category,
                    InEdge_Category = request.companyrequest.InEdge_Category,
                    Is_PO_Wise_Batch = request.companyrequest.Is_PO_Wise_Batch,
                    IsBonusPayThroughFF = request.companyrequest.IsBonusPayThroughFF,
                    IsExtraWorkingDaysServiceFee = request.companyrequest.IsExtraWorkingDaysServiceFee,
                    AttendanceInputWithLeave = request.companyrequest.AttendanceInputWithLeave,
                    Management_MIS = request.companyrequest.Management_MIS,
                    PfCode_Id = request.companyrequest.PfCode_Id,
                    IsDecimal = request.companyrequest.IsDecimal,
                    IsProforma = request.companyrequest.IsProforma,
                    CompanyType = request.companyrequest.CompanyType,
                    Manual_NewJoinee = request.companyrequest.Manual_NewJoinee,
                    Invoice_Submission_Date = request.companyrequest.Invoice_Submission_Date,
                    Collection_Date = request.companyrequest.Collection_Date,
                    PE_User_ID = request.companyrequest.PE_User_ID,
                    PE_Name = request.companyrequest.PE_Name,
                    PE_Email_Id = request.companyrequest.PE_Email_Id,
                    RM_User_ID = request.companyrequest.RM_User_ID,
                    RM_Name = request.companyrequest.RM_Name,
                    RM_Email_Id = request.companyrequest.RM_Email_Id,
                    Client_SPOC_Name = request.companyrequest.Client_SPOC_Name,
                    Client_SPOC_Email_Id = request.companyrequest.Client_SPOC_Email_Id,
                    Client_SPOC_Mobile_No = request.companyrequest.Client_SPOC_Mobile_No,
                    Client_Escalation_Manager_Name = request.companyrequest.Client_Escalation_Manager_Name,
                    Client_Escalation_Manager_Email_Id = request.companyrequest.Client_Escalation_Manager_Email_Id,
                    Client_Escalation_Manager_Mobile_No = request.companyrequest.Client_Escalation_Manager_Mobile_No,
                    Portal_Payslip_Format = request.companyrequest.Portal_Payslip_Format,
                    IsNewJoinee = request.companyrequest.IsNewJoinee,
                    ReimbPaymentId = request.companyrequest.ReimbPaymentId,
                    PayrollWithDecimalId = request.companyrequest.PayrollWithDecimalId,
                    PfCategoryId = request.companyrequest.PfCategoryId,
                    IsSignature = request.companyrequest.IsSignature,
                    ServiceFeeWithDecimalId = request.companyrequest.ServiceFeeWithDecimalId,
                    Qdemy_charges = request.companyrequest.Qdemy_charges,
                    IsCurrencyConversion = request.companyrequest.IsCurrencyConversion,
                    TechSubscriptionCharges = request.companyrequest.TechSubscriptionCharges,
                    Tech_Subscription_Charges_Criteria_Type = request.companyrequest.Tech_Subscription_Charges_Criteria_Type,
                    DigitalPlatformConsent = request.companyrequest.DigitalPlatformConsent,
                    DGPSF = request.companyrequest.DGPSF,
                    Vertical_Id = request.companyrequest.Vertical_Id,
                    ServiceChargeClubbing = request.companyrequest.ServiceChargeClubbing,
                    IsOneTouchInvoicing = request.companyrequest.IsOneTouchInvoicing,
                    IsInvoicePoBased = request.companyrequest.IsInvoicePoBased,
                    IS_ESI_split = request.companyrequest.IS_ESI_split,
                    WorkingHours = request.companyrequest.WorkingHours,
                    Is40BillingModel = request.companyrequest.Is40BillingModel,
                    BillingCompanyId = request.companyrequest.BillingCompanyId,
                    Contract_Start_Date = request.companyrequest.Contract_Start_Date,
                    Contract_End_Date = request.companyrequest.Contract_End_Date,
                    Contract_File_Path = request.companyrequest.Contract_File_Path,
                    Contract_File_Name = request.companyrequest.Contract_File_Name,
                    Contract_Uploaded_File_Name = request.companyrequest.Contract_Uploaded_File_Name,
                    Service_Tax_Date = request.companyrequest.Service_Tax_Date,
                    Service_Tax_File_Path = request.companyrequest.Service_Tax_File_Path,
                    Service_Tax_File_Name = request.companyrequest.Service_Tax_File_Name,
                    Service_Tax_Uploaded_File_Name = request.companyrequest.Service_Tax_Uploaded_File_Name,
                    Bank_Id = request.companyrequest.Bank_Id,
                    IFSC_Code = request.companyrequest.IFSC_Code,
                    Account_Number = request.companyrequest.Account_Number,
                    Bank_Address = request.companyrequest.Bank_Address,
                    Branch = request.companyrequest.Branch,
                    BranchCode = request.companyrequest.BranchCode,
                    BankCode = request.companyrequest.BankCode,
                    BankAdviceId = request.companyrequest.BankAdviceId,

                    OT_WEEKEND_TYPE=request.companyrequest.OT_WEEKEND_TYPE,
                    OT_WEEKEND_VLAUE= request.companyrequest.OT_WEEKEND_VLAUE,
                    OT_WEEKEND_FORMULA= request.companyrequest.OT_WEEKEND_FORMULA,

                    OT_WEEK_DAY_TYPE = request.companyrequest.OT_WEEK_DAY_TYPE,
                    OT_WEEK_DAY_VLAUE = request.companyrequest.OT_WEEK_DAY_VLAUE,
                    OT_WEEK_DAY_FORMULA = request.companyrequest.OT_WEEK_DAY_FORMULA,

                    OT_NIGHT_SHIFT_TYPE = request.companyrequest.OT_NIGHT_SHIFT_TYPE,
                    OT_NIGHT_SHIFT_VLAUE = request.companyrequest.OT_NIGHT_SHIFT_VLAUE,
                    OT_NIGHT_SHIFT_FORMULA = request.companyrequest.OT_NIGHT_SHIFT_FORMULA,

                    OT_HOLIDAY_TYPE = request.companyrequest.OT_HOLIDAY_TYPE,
                    OT_HOLIDAY_VLAUE = request.companyrequest.OT_HOLIDAY_VLAUE,
                    OT_HOLIDAY_FORMULA = request.companyrequest.OT_HOLIDAY_FORMULA,

                    Adhoc_Service_Fee = request.companyrequest.Adhoc_Service_Fee,
                    Adhoc_Service_Formula = request.companyrequest.Adhoc_Service_Formula,

                    Portal_Type = request.companyrequest.Portal_Type
                }
            };

            string parentdata = ToXmlCompanyCreate(parentdatawrapper);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@mode"] = request.mode,
                ["@CreatedBy"] = request.CreatedBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdate_Company", parameters);
        }


        public async Task<DataSet> Update(CompanyUpdateRequestPayload request)
        {
            var parentdatawrapper = new CompanyUpdateRequestWrapper
            {
                Company = new CompanyUpdateRequest
                {
                    Company_Id = request.companyrequest.Company_Id,
                    Company_Contract_Id = request.companyrequest.Company_Contract_Id,
                    Client_Id = request.companyrequest.Client_Id,
                    Financial_Year_Id = request.companyrequest.Financial_Year_Id,
                    Client_Since = request.companyrequest.Client_Since,
                    Company_Active = request.companyrequest.Company_Active,
                    Is_Zip_Documents = request.companyrequest.Is_Zip_Documents,
                    Payroll_Type = request.companyrequest.Payroll_Type,
                    Invoicing_Type = request.companyrequest.Invoicing_Type,
                    Investment_Block_Date = request.companyrequest.Investment_Block_Date,
                    Business_Unit_Name_Id = request.companyrequest.Business_Unit_Name_Id,
                    Month_Days = request.companyrequest.Month_Days,
                    Salary_Fix_Days = request.companyrequest.Salary_Fix_Days,
                    Business_Unit_Location_Id = request.companyrequest.Business_Unit_Location_Id,
                    Attendance_Cycle_From = request.companyrequest.Attendance_Cycle_From,
                    Attendance_Cycle_To = request.companyrequest.Attendance_Cycle_To,
                    Is_PF_Remittance = request.companyrequest.Is_PF_Remittance,
                    Input_Date = request.companyrequest.Input_Date,
                    Output_Date = request.companyrequest.Output_Date,
                    Work_Days_Based_On = request.companyrequest.Work_Days_Based_On,
                    CTC = request.companyrequest.CTC,
                    Sourcing_Fee_Criteria_Type = request.companyrequest.Sourcing_Fee_Criteria_Type,
                    Sourcing_Fee = request.companyrequest.Sourcing_Fee,
                    Absorption_Fee_Criteria_Type = request.companyrequest.Absorption_Fee_Criteria_Type,
                    Absorption_Fee = request.companyrequest.Absorption_Fee,
                    Incentive_Type = request.companyrequest.Incentive_Type,
                    Is_PO_Applicable = request.companyrequest.Is_PO_Applicable,
                    Salary_SMS = request.companyrequest.Salary_SMS,
                    Dues_Based_On = request.companyrequest.Dues_Based_On,
                    Is_Insurance_Applicable = request.companyrequest.Is_Insurance_Applicable,
                    ReimbInvoiceFormat_Id = request.companyrequest.ReimbInvoiceFormat_Id,
                    Segment_Id = request.companyrequest.Segment_Id,
                    SubSegment_Id = request.companyrequest.SubSegment_Id,
                    Payslip_Format = request.companyrequest.Payslip_Format,
                    Mode_Of_Payment = request.companyrequest.Mode_Of_Payment,
                    TAT = request.companyrequest.TAT,
                    Billing_Type = request.companyrequest.Billing_Type,
                    Is_RoundOff_Applicable = request.companyrequest.Is_RoundOff_Applicable,
                    Deviation = request.companyrequest.Deviation,
                    Incharge = request.companyrequest.Incharge,
                    Credit_Days_Upfront = request.companyrequest.Credit_Days_Upfront,
                    Customer_Type = request.companyrequest.Customer_Type,
                    Incentive_Date = request.companyrequest.Incentive_Date,
                    Service_Tax_Applicable = request.companyrequest.Service_Tax_Applicable,
                    Reimbursement_Type = request.companyrequest.Reimbursement_Type,
                    Salary_Transfer_Date = request.companyrequest.Salary_Transfer_Date,
                    Effective_Date = request.companyrequest.Effective_Date,
                    Sales_Person = request.companyrequest.Sales_Person,
                    Branch_Location = request.companyrequest.Branch_Location,
                    Reimbursement_Date = request.companyrequest.Reimbursement_Date,
                    SAP_Code = request.companyrequest.SAP_Code,
                    Pin_Code = request.companyrequest.Pin_Code,
                    Address = request.companyrequest.Address,
                    Phone_Number = request.companyrequest.Phone_Number,
                    PAN_Number = request.companyrequest.PAN_Number,
                    TAN_Number = request.companyrequest.TAN_Number,
                    Service_Tax_Number = request.companyrequest.Service_Tax_Number,
                    PF_Code = request.companyrequest.PF_Code,
                    ESI_Code = request.companyrequest.ESI_Code,
                    PT_Code = request.companyrequest.PT_Code,
                    Email_Id = request.companyrequest.Email_Id,
                    Certificate_Number = request.companyrequest.Certificate_Number,
                    Fax_Number = request.companyrequest.Fax_Number,
                    Website_Name = request.companyrequest.Website_Name,
                    Wages = request.companyrequest.Wages,
                    Particulars = request.companyrequest.Particulars,
                    Is_NonInvoice = request.companyrequest.Is_NonInvoice,
                    Mis_Name = request.companyrequest.Mis_Name,
                    Zone_Tagging = request.companyrequest.Zone_Tagging,
                    IsHeaderFooter = request.companyrequest.IsHeaderFooter,
                    Sap_Customer_Code = request.companyrequest.Sap_Customer_Code,
                    Profit_Center_Code = request.companyrequest.Profit_Center_Code,
                    Inedge_charges = request.companyrequest.Inedge_charges,
                    Inedge_charges_Criteria_Type = request.companyrequest.Inedge_charges_Criteria_Type,
                    CompanyGroupCode = request.companyrequest.CompanyGroupCode,
                    OnBoarding_Category = request.companyrequest.OnBoarding_Category,
                    InEdge_Category = request.companyrequest.InEdge_Category,
                    Is_PO_Wise_Batch = request.companyrequest.Is_PO_Wise_Batch,
                    IsBonusPayThroughFF = request.companyrequest.IsBonusPayThroughFF,
                    IsExtraWorkingDaysServiceFee = request.companyrequest.IsExtraWorkingDaysServiceFee,
                    AttendanceInputWithLeave = request.companyrequest.AttendanceInputWithLeave,
                    Management_MIS = request.companyrequest.Management_MIS,
                    PfCode_Id = request.companyrequest.PfCode_Id,
                    IsDecimal = request.companyrequest.IsDecimal,
                    IsProforma = request.companyrequest.IsProforma,
                    CompanyType = request.companyrequest.CompanyType,
                    Manual_NewJoinee = request.companyrequest.Manual_NewJoinee,
                    Invoice_Submission_Date = request.companyrequest.Invoice_Submission_Date,
                    Collection_Date = request.companyrequest.Collection_Date,
                    PE_User_ID = request.companyrequest.PE_User_ID,
                    PE_Name = request.companyrequest.PE_Name,
                    PE_Email_Id = request.companyrequest.PE_Email_Id,
                    RM_User_ID = request.companyrequest.RM_User_ID,
                    RM_Name = request.companyrequest.RM_Name,
                    RM_Email_Id = request.companyrequest.RM_Email_Id,
                    Client_SPOC_Name = request.companyrequest.Client_SPOC_Name,
                    Client_SPOC_Email_Id = request.companyrequest.Client_SPOC_Email_Id,
                    Client_SPOC_Mobile_No = request.companyrequest.Client_SPOC_Mobile_No,
                    Client_Escalation_Manager_Name = request.companyrequest.Client_Escalation_Manager_Name,
                    Client_Escalation_Manager_Email_Id = request.companyrequest.Client_Escalation_Manager_Email_Id,
                    Client_Escalation_Manager_Mobile_No = request.companyrequest.Client_Escalation_Manager_Mobile_No,
                    Portal_Payslip_Format = request.companyrequest.Portal_Payslip_Format,
                    IsNewJoinee = request.companyrequest.IsNewJoinee,
                    ReimbPaymentId = request.companyrequest.ReimbPaymentId,
                    PayrollWithDecimalId = request.companyrequest.PayrollWithDecimalId,
                    PfCategoryId = request.companyrequest.PfCategoryId,
                    IsSignature = request.companyrequest.IsSignature,
                    ServiceFeeWithDecimalId = request.companyrequest.ServiceFeeWithDecimalId,
                    Qdemy_charges = request.companyrequest.Qdemy_charges,
                    IsCurrencyConversion = request.companyrequest.IsCurrencyConversion,
                    TechSubscriptionCharges = request.companyrequest.TechSubscriptionCharges,
                    Tech_Subscription_Charges_Criteria_Type = request.companyrequest.Tech_Subscription_Charges_Criteria_Type,
                    DigitalPlatformConsent = request.companyrequest.DigitalPlatformConsent,
                    DGPSF = request.companyrequest.DGPSF,
                    Vertical_Id = request.companyrequest.Vertical_Id,
                    ServiceChargeClubbing = request.companyrequest.ServiceChargeClubbing,
                    IsOneTouchInvoicing = request.companyrequest.IsOneTouchInvoicing,
                    IsInvoicePoBased = request.companyrequest.IsInvoicePoBased,
                    IS_ESI_split = request.companyrequest.IS_ESI_split,
                    WorkingHours = request.companyrequest.WorkingHours,
                    Is40BillingModel = request.companyrequest.Is40BillingModel,
                    BillingCompanyId = request.companyrequest.BillingCompanyId,
                    Contract_Start_Date = request.companyrequest.Contract_Start_Date,
                    Contract_End_Date = request.companyrequest.Contract_End_Date,
                    Contract_File_Path = request.companyrequest.Contract_File_Path,
                    Contract_File_Name = request.companyrequest.Contract_File_Name,
                    Contract_Uploaded_File_Name = request.companyrequest.Contract_Uploaded_File_Name,
                    Service_Tax_Date = request.companyrequest.Service_Tax_Date,
                    Service_Tax_File_Path = request.companyrequest.Service_Tax_File_Path,
                    Service_Tax_File_Name = request.companyrequest.Service_Tax_File_Name,
                    Service_Tax_Uploaded_File_Name = request.companyrequest.Service_Tax_Uploaded_File_Name,
                    Bank_Id = request.companyrequest.Bank_Id,
                    IFSC_Code = request.companyrequest.IFSC_Code,
                    Account_Number = request.companyrequest.Account_Number,
                    Bank_Address = request.companyrequest.Bank_Address,
                    Branch = request.companyrequest.Branch,
                    BranchCode = request.companyrequest.BranchCode,
                    BankCode = request.companyrequest.BankCode,
                    BankAdviceId = request.companyrequest.BankAdviceId,

                    OT_WEEKEND_TYPE = request.companyrequest.OT_WEEKEND_TYPE,
                    OT_WEEKEND_VLAUE = request.companyrequest.OT_WEEKEND_VLAUE,
                    OT_WEEKEND_FORMULA = request.companyrequest.OT_WEEKEND_FORMULA,

                    OT_WEEK_DAY_TYPE = request.companyrequest.OT_WEEK_DAY_TYPE,
                    OT_WEEK_DAY_VLAUE = request.companyrequest.OT_WEEK_DAY_VLAUE,
                    OT_WEEK_DAY_FORMULA = request.companyrequest.OT_WEEK_DAY_FORMULA,

                    OT_NIGHT_SHIFT_TYPE = request.companyrequest.OT_NIGHT_SHIFT_TYPE,
                    OT_NIGHT_SHIFT_VLAUE = request.companyrequest.OT_NIGHT_SHIFT_VLAUE,
                    OT_NIGHT_SHIFT_FORMULA = request.companyrequest.OT_NIGHT_SHIFT_FORMULA,

                    OT_HOLIDAY_TYPE = request.companyrequest.OT_HOLIDAY_TYPE,
                    OT_HOLIDAY_VLAUE = request.companyrequest.OT_HOLIDAY_VLAUE,
                    OT_HOLIDAY_FORMULA = request.companyrequest.OT_HOLIDAY_FORMULA,

                    Adhoc_Service_Fee = request.companyrequest.Adhoc_Service_Fee,
                    Adhoc_Service_Formula = request.companyrequest.Adhoc_Service_Formula,

                    Portal_Type = request.companyrequest.Portal_Type

                }
            };

            string parentdata = ToXmlCompanyUpdate(parentdatawrapper);

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = parentdata,
                ["@mode"] = request.mode,
                ["@CreatedBy"] = request.CreatedBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdate_Company", parameters);
        }


        public async Task<DataSet> DeleteCompany(CompanyDeleteRequest request)
        {
            XDocument xml = new XDocument(
    new XElement("Main",
        new XElement("Company",
            new XElement("Company_Id", request.Company_Id)
        )
    )
);

            string xmlString = xml.ToString();

            var parameters = new Dictionary<string, object>
            {
                ["@xmlInput"] = xmlString,
                ["@mode"] = request.mode,
                ["@CreatedBy"] = request.CreatedBy,
            };
            return _dbRepository.ExecuteStoredProcedureToDataSetAsync("sp_CreateUpdate_Company", parameters);
        }

        public string ToXmlCompanyCreate(CompanyCreateRequestWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(CompanyCreateRequestWrapper));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // remove xmlns

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // 🚀 remove the XML header
                Indent = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, wrapper, ns);
                return sw.ToString();
            }
        }

        public string ToXmlCompanyUpdate(CompanyUpdateRequestWrapper wrapper)
        {
            var serializer = new XmlSerializer(typeof(CompanyUpdateRequestWrapper));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // remove xmlns

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // 🚀 remove the XML header
                Indent = true
            };

            using (var sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                serializer.Serialize(writer, wrapper, ns);
                return sw.ToString();
            }
        }

    }
}
