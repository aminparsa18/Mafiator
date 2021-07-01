using Mafiator.Data;
using Mafiator.Service.Contracts.Identity;
using Mafiator.Service.Contracts.Impl.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mafiator.IocConfig.Extensions
{
    public static class AddIdentityRegistryExtentions
    {
        public static void AddCustomIdentityServices(this IServiceCollection services,IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            services.AddIdentityWithOptions(configuration,webHostEnvironment);
            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
            services.AddScoped<ApplicationIdentityErrorDescriber>();
        }
        public static void CallDbInitializer(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var identityDbInitialize = scope.ServiceProvider.GetService<IIdentityDbInitializer>();
            identityDbInitialize.Initialize();
            identityDbInitialize.SeedData();
        }
    }
}
