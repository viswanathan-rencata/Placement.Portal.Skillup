using Placement.Portal.Skillup.Data;
using Placement.Portal.Skillup.Interface.Data;
using Placement.Portal.Skillup.Interface;

namespace Placement.Portal.Skillup.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ICollegeMasterRepository, CollegeMasterRepository>();
            services.AddTransient<ICollegeMasterRepository, CollegeMasterRepository>();
            services.AddTransient<IAppUserRepository, AppUserRepository>();
            services.AddTransient<ICompanyRequestRepository, CompanyRequestRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
