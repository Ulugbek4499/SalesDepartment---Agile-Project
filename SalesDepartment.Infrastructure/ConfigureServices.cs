using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Infrastructure.Persistence;
using SalesDepartment.Infrastructure.Persistence.Interceptors;

namespace SalesDepartment.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {

                options.UseNpgsql(configuration.GetConnectionString("DbConnect"));
                options.UseLazyLoadingProxies();
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            return services;
        }
    }
}
