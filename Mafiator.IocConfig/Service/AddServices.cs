using Mafiator.Common.SiteSetting;
using Mafiator.IocConfig.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mafiator.IocConfig.Service
{
    public static class AddServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services,
             IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            services.AddMainServices(configuration);
            services.AddCustomIdentityServices(configuration,webHostEnvironment);
            return services;
        }
    }
}