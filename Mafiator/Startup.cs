using System.Data;
using System.Threading.Tasks;
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
            services.AddHangfire(x => x.UseSqlServerStorage("Server=94.130.50.85;Initial Catalog=MafiatorJobs;User ID=MafiatorJobs;Integrated Security=False;Password=54Delta45!;MultipleActiveResultSets=true;"));
            services.AddHangfireServer();
            var c = Configuration.GetConnectionString("MafiatorContext");
            var cs = "Server=94.130.50.85;Initial Catalog=MafiatorDB;User ID=Mafiator;Integrated Security=False;Password=54Delta45!;MultipleActiveResultSets=true;";
            var csLocal = "data source=LAPTOP-OFP1Q77E;Initial Catalog=mftor;integrated security=True;MultipleActiveResultSets=true;";
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(cs).EnableSensitiveDataLogging());
            SqlServerBootstrap.Initialize();
            services.AddTransient<IDbConnection>(sp => new SqlConnection(cs));
            services.AddCustomServices(Configuration,WebHostEnvironment);
            services.AddScoped<IGameService, GameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddCustomMiddleware();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter()
                }
            });
            app.Run(async context => await Task.Run(() => context.Response.Redirect("/swagger")));
        }
    }
}
