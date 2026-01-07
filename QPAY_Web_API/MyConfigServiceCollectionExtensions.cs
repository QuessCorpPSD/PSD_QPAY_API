using QPay.API.Extensions;
using QPay.API.Models;
using QPay.BAL.IRepository;
using QPay.BAL.IRepository.Common;
using QPay.BAL.Repository;
using QPay.BAL.Repository.Common;

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
            //services.AddSingleton<IInvoiceInitiationRepository, InvoiceInitiationRepository>();

            #endregion



            #region Dependencies   SOP DI

            services.AddSingleton<IQARepository, QARepository>();
            #endregion


            #region Dependencies Common Master DI (Depdency Injection) 
            services.AddSingleton<IProcessCategoryRepository, ProcessCategoryRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccesstypeRepository, AccesstypeRepository>();
            #endregion
        }
    }

}
