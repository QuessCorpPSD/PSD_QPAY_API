using QPay.BAL.IRepository;
using QPay.BAL.Repository;

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
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<IPayRegisterRepository, PayRegisterRepository>();
            services.AddSingleton<IAssignmentRepository, AssignmentRepository>();
            services.AddSingleton<IQARepository, QARepository>();
            services.AddSingleton<IDashboardRepository, DashboardRepository>();
            services.AddSingleton<ICheckInCheckOutRepository, CheckInCheckOutRepository>();
        }
    }

}
