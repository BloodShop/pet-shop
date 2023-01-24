using Microsoft.EntityFrameworkCore;
using PetShopProj.Data;
using PetShopProj.Extensions;
using PetShopProj.Repositories;

namespace PetShopProj.Configuration
{
    public class InfrastuctureServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, string environmentName)
        {
            services.AddTransient<IRepository, PetRepository>();
            string connectionString = configuration.GetConnectionStringFromEnvironment(environmentName);
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddDbContext<ICallCenterContext, PetDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
        }
    }
}
