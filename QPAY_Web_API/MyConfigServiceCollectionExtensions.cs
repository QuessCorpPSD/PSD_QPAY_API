using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.AccountReceivable;
using QPay.BAL.IRepository.Billing;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.IRepository.Process;
using QPay.BAL.IRepository.Reports;
using QPay.BAL.IRepository.SalaryReleaseInvoice;
using QPay.BAL.IRepository.Tools;
using QPay.BAL.Repository;
using QPay.BAL.Repository.Billing;
using QPay.BAL.Repository.Common;
using QPay.BAL.Repository.Customer;
using QPay.BAL.Repository.GlobalMaster;
using QPay.BAL.Repository.Invoice;
using QPay.BAL.Repository.Invoice;
using QPay.BAL.Repository.Process;
using QPay.BAL.Repository.Reports;
using QPay.BAL.Repository.SalaryReleaseInvoice;
using QPay.BAL.Repository.Tools;
using QPay.IRepository.Repository.Common;
using QPAY_Web_API.Controller;


namespace QPay.API
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            return services;
        }
    }
    public static class Bootstrapper
    {
        public static void AddServices(this IServiceCollection services)
        {

            services.AddHttpClient();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IEmailService, EmailService>();

            #region Dependencies  PSD DI       

            services.AddSingleton<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddSingleton<IAssignmentRepository, AssignmentRepository>();
            services.AddSingleton<IBillableDaysRepository, BillableDaysRepository>();
            services.AddSingleton<ICheckInCheckOutRepository, CheckInCheckOutRepository>();
            services.AddSingleton<IDashboardRepository, DashboardRepository>();
            services.AddSingleton<IFinancialYearRepository, FinancialYearRepository>();
            services.AddSingleton<BAL.IRepository.IInvoiceInitiationRepository, BAL.Repository.InvoiceInitiationRepository>();
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<IPayRegisterRepository, PayRegisterRepository>();
            services.AddTransient<Itbl_InputLot_DetailsRepository, Itbl_InputLot_DetailsRepository>();

            #endregion

            #region Dependencies   SOP DI
            services.AddSingleton<IQARepository, QARepository>();
            #endregion

            #region Dependencies Common Master DI (Depdency Injection) 
            services.AddSingleton<IProcessCategoryRepository, ProcessCategoryRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccesstypeRepository, AccesstypeRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();

            #endregion

            #region Dependencies Billing DI (Depdency Injection) 
            services.AddScoped<ISapBookClosureRepository, SapBookClosureRepository>();
            services.AddScoped<IGenericUploadRepository, GenericUploadRepository>();

            #endregion
            #region Dependencies Customer DI (Depdency Injection) 
            services.AddScoped<IBandRepository, BandRepository>();
            services.AddScoped<ICancelDocumentRepository, CancelDocumentRepository>();
            services.AddScoped<IClientAddressRespository, ClientAddressRepository>();
            services.AddScoped<ICompanyPaycodeMappingRepository, CompanyPaycodeMappingRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICorporateBankRepository, CorporateBankRepository>();
            services.AddScoped<ICostCenterMappingRepository, CostCenterMappingRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IITCalenderRepository, ITCalenderRepository>();
            services.AddScoped<IPayFrequencyRepository, PayFrequencyRepository>();
            services.AddScoped<IServiceChargeRepository, ServiceChargeRepository>();

            #endregion
            #region Dependencies Global Master DI (Depdency Injection) 
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IESIRepository, ESIRepository>();
            services.AddScoped<IFormulaRepository, FormulaRepository>();
            services.AddScoped<IGstRepository, GstRepository>();
            services.AddScoped<IInvoiceLegalEntityRepository, InvoiceLegalEntityRepository>();
            services.AddScoped<ILWFRepository, LWFRepository>();
            services.AddScoped<IMaterialCodeRepository, MaterialCodeRepository>();
            services.AddScoped<IPaycodeRepository, PaycodeRepository>();
            services.AddScoped<IPFRepository, PFRepository>();
            services.AddScoped<IPTRepository, PTRepository>();
            services.AddScoped<ISiteMasterRepository, SiteMasterRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IVendorMasterRepository, VendorMasterRepository>();

            #endregion
            #region Dependencies Invoice DI (Depdency Injection) 
            services.AddScoped<IBillingPayFrequencyRepository, BillingPayFrequencyRepository>();
            services.AddScoped<IClientBillableReportRepository, ClientBillableReportRepository>();
            services.AddScoped<ICompanyInvoiceFormatRepository, CompanyInvoiceFormatRepository>();
            services.AddScoped<ICreditNoteApproveRepository, CreditNoteApproveRepository>();
            services.AddScoped<ICreditNoteRepository, CreditNoteRepository>();
            services.AddScoped<ICreditNoteUpdateRepository, CreditNoteUpdateRepository>();
            //services.AddScoped<IDraftNewRepository, DraftNewRepository>();
            services.AddScoped<IGSTInvoiceRepository, GSTInvoiceRepository>();
            services.AddScoped<IInvoiceCultureRepository, InvoiceCultureRepository>();
            services.AddScoped<BAL.IRepository.Invoice.IInvoiceInitiationRepository, BAL.Repository.Invoice.InvoiceInitiationRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IPOInvoiceInitiateRepository, POInvoiceInitiateRepository>();
            services.AddScoped<IProvisionalInvoiceRepository, ProvisionalInvoiceRepository>();

            #endregion
            #region Dependencies Process DI (Depdency Injection) 
            services.AddScoped<IAllowReprocessRepository, AllowReprocessRepository>();
            services.AddScoped<IArrearAttendanceProcessRepository, ArrearAttendanceProcessRepository>();
            services.AddScoped<IAttendanceProcessRepository, AttendanceProcessRepository>();
            services.AddScoped<IAttendanceBatchIdUpdateRepository, AttendanceBatchIdUpdateRepository>();
            services.AddScoped<IFNFRevokeRepository, FNFRevokeRepository>();
            services.AddScoped<IITAdjustmentRepository, ITAdjustmentRepository>();
            services.AddScoped<ILockPayperiodRepository, LockPayperiodRepository>();
            services.AddScoped<ILOPAdjustmentProcessRepository, LOPAdjustmentProcessRepository>();
            services.AddScoped<IOnetimeReplacementRepository, OnetimeReplacementRepository>();
            services.AddScoped<OtherIncomeRepository, OtherIncomeRepository>();
            services.AddScoped<IPayProcessRepository, PayProcessRepository>();
            services.AddScoped<IPayRegisterUploadRepository, PayRegisterUploadRepository>();
            services.AddScoped<IPayTransactionRepository, PayTransactionRepository>();
            services.AddScoped<IReimbursementCalendarRepository, ReimbursementCalendarRepository>();

            #endregion
            #region Dependencies Promotion DI (Depdency Injection) 
            services.AddScoped<IPromotionIncrementRepository, PromotionIncrementRepository>();

            #endregion
            #region Dependencies Reimbursement DI (Depdency Injection) 
            services.AddScoped<IReimbursementRepository, ReimbursementRepository>();
            //services.AddScoped<IReimbursementReIloapository, ReimbursementRepository>();
            //services.AddScoped<IReimbursementRepository, ReimbursementRepository>();

            #endregion
            #region Dependencies Reports DI (Depdency Injection) 
            services.AddScoped<IBillingReportRepository, BillingReportRepository>();
            services.AddScoped<IBillingUBRRepository, BillingUBRRepository>();
            services.AddScoped<ICreditNoteBalanceReportRepository, CreditNoteBalanceReportRepository>();
            services.AddScoped<IIncrementReportRepository, IncrementReportRepository>();
            services.AddScoped<IInvoiceLeaveBalanceReportRepository, InvoiceLeaveBalanceReportRepository>();
            services.AddScoped<IInvoiceSummaryRepository, InvoiceSummaryRepository>();
            services.AddScoped<ILeaveBalanceReportRepository, LeaveBalanceReportRepository>();
            services.AddScoped<INetpaySummaryRepository, NetpaySummaryRepository>();
            services.AddScoped<IOtherIncomeEntitywiseRepository, OtherIncomeEntitywiseRepository>();
            services.AddScoped<IOtherIncomeProcessEmployeeRepository, OtherIncomeProcessEmployeeRepository>();
            services.AddScoped<IOtherIncomeReportRepository, OtherIncomeReportRepository>();
            services.AddScoped<IPayregisterEntitywiseRepository, PayregisterEntitywiseRepository>();
            services.AddScoped<IPayregisterUnprocessedRepository, PayregisterUnprocessedRepository>();
            services.AddScoped<IPayslipReportRepository, PayslipReportRepository>();
            services.AddScoped<IPOBalanceReportRepository, POBalanceReportRepository>();
            services.AddScoped<IPoReportRepository, PoReportRepository>();
            services.AddScoped<IProcessEmployeeRepository, ProcessEmployeeRepository>();
            services.AddScoped<ITimesheetSummaryReportRepository, TimesheetSummaryReportRepository>();


            #endregion
            #region Dependencies TaxAndSaving DI (Depdency Injection) 
            services.AddScoped<IChildrenEducationAllowanceRepository, ChildrenEducationAllowanceRepository>();
            services.AddScoped<ICompanyProvidedBenefitsRepository, CompanyProvidedBenefitsRepository>();
            services.AddScoped<IGratuityRepository, GratuityRepository>();
            services.AddScoped<IHRARepository, HRARepository>();
            services.AddScoped<IIncomeLossHousingPropertyRepository, IncomeLossHousingPropertyRepository>();
            services.AddScoped<ILTACalculationRepository, LTACalculationRepository>();
            services.AddScoped<IPreviousEmploymentRepository, PreviousEmploymentRepository>();
            services.AddScoped<ITaxDeclarationAndActualRepository, TaxDeclarationAndActualRepository>();

            #endregion
            #region Dependencies Tools DI (Depdency Injection) 
            services.AddScoped<IDynamicUploadRepository, DynamicUploadRepository>();

            #endregion

            #region Dependencies Input Aggregator (Depdency Injection) 
            services.AddScoped<IInputAggregatorRepository, InputAggregatorRepository>();

            #endregion
            //services.AddScoped<ITDSSlabMasterRepository, TDSSlabMasterRepository>();
            //services.AddScoped<IBranchMasterRepository, BranchMasterRepository>();
            //services.AddScoped<IComputationRuleRepository, ComputationRuleRepository>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IGSTInvoiceRepository, GSTInvoiceRepository>();
            services.AddScoped<IEInvoiceRepository, EInvoiceRepository>();
            services.AddScoped<IInputAggregatorAttendanceRepository, InputAggregatorAttendanceRepository>();
            services.AddSingleton<ISezRepository, SezRepository>();
            services.AddScoped<ISalaryReleasePendingApprovalRepository, SalaryReleasePendingApprovalRepository>();
            services.AddScoped<IBatchGenerationRepository, BatchGenerationRepository>();
            services.AddScoped<IBankNeftCultureInvoiceRepository, BankNeftCultureInvoiceRepository>();
            services.AddScoped<IClientGSTRepository, ClientGSTRepository>();
            services.AddScoped<IVendorClientAddressRespository, VendorClientAddressRespository>();
            services.AddScoped<IVendorClientGstRepository, VendorClientGSTRepository>();
            services.AddScoped<ISEZRepositoryService, SEZRepositoryService>();
            services.AddScoped<IClientAdvancePaymentRepository, ClientAdvancePaymentRepository>();

        }
    }
}
