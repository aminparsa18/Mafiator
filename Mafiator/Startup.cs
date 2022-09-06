using Hangfire;
using Mafiator.Data;
using Mafiator.IocConfig.Middleware;
using Mafiator.IocConfig.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoDb;
using System.Data;
using System.Threading.Tasks;

namespace Mafiator.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }
        
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment{ get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           // services.AddHangfire(x => x.UseSqlServerStorage("Server=tcp:mftor.database.windows.net,1433;Initial Catalog=mftor_jobs;Persist Security Info=False;User ID=mftor_admin;Password=54Delta45!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
           // services.AddHangfireServer();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MafiatorContext")).EnableSensitiveDataLogging());
            SqlServerBootstrap.Initialize();
            services.AddTransient<IDbConnection>(sp => new SqlConnection(Configuration.GetConnectionString("MafiatorContext")));
            services.AddCustomServices(Configuration,WebHostEnvironment);
            services.AddScoped<IGameService, GameService>();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddCustomMiddleware();
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[]
            //    {
            //        new HangfireAuthorizationFilter()
            //    }
            //});
            app.Run(async context => await Task.Run(() => context.Response.Redirect("/swagger")));
        }
    }
}
