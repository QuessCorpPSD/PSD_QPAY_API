using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Common;
using QPay.BAL.IRepository.Customer;
using QPay.BAL.IRepository.GlobalMaster;
using QPay.BAL.Repository;
using QPay.BAL.Repository.Common;
using QPay.BAL.Repository.Customer;
using QPay.BAL.Repository.GlobalMaster;
using QPay.BAL.IRepository.Invoice;
using QPay.BAL.Repository.Invoice;
using QPay.IRepository.Repository.Common;


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
         
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<IPayRegisterRepository, PayRegisterRepository>();
            services.AddSingleton<IAssignmentRepository, AssignmentRepository>();
            services.AddSingleton<IDashboardRepository, DashboardRepository>();
            services.AddSingleton<ICheckInCheckOutRepository, CheckInCheckOutRepository>();
            services.AddSingleton<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddSingleton<IFinancialYearRepository, FinancialYearRepository>();
            services.AddSingleton<IInvoiceInitiationRepository, InvoiceInitiationRepository>();
            

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

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IClientAddressRespository, ClientAddressRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IServiceChargeRepository, ServiceChargeRepository>();
            services.AddScoped<ICostCenterMappingRepository, CostCenterMappingRepository>();
            services.AddScoped<ICompanyPaycodeMappingRepository, CompanyPaycodeMappingRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IITCalenderRepository, ITCalenderRepository>();
            services.AddScoped<IPayFrequencyRepository, PayFrequencyRepository>();

            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IInvoiceLegalEntityRepository, InvoiceLegalEntityRepository>();
            services.AddScoped<ICorporateBankRepository, CorporateBankRepository>();


            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IFormulaRepository, FormulaRepository>();
            services.AddScoped<ISiteMasterRepository, SiteMasterRepository>();
            services.AddScoped<IVendorMasterRepository, VendorMasterRepository>();
            services.AddScoped<IBandRepository, BandRepository>();
            services.AddScoped<IESIRepository, ESIRepository>();
            services.AddScoped<ILWFRepository, LWFRepository>();
            services.AddScoped<IPTRepository, PTRepository>();
            services.AddScoped<IPFRepository, PFRepository>();

            //services.AddScoped<ITDSSlabMasterRepository, TDSSlabMasterRepository>();
            //services.AddScoped<IBranchMasterRepository, BranchMasterRepository>();
            //services.AddScoped<IComputationRuleRepository, ComputationRuleRepository>();
           
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IGSTInvoiceRepository, GSTInvoiceRepository>();

        }
    }

}
