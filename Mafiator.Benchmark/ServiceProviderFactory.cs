using Mafiator.Data;
using Mafiator.Repository;
using MessagePipe;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoDb;
using System.Data;

namespace Mafiator.Benchmark;

public class ServiceProviderFactory
{
    private const string connectionString = "data source=.;Initial Catalog=MafiatorBenchmarkDB;Integrated Security=true;MultipleActiveResultSets=True;App=EntityFramework;Encrypt=False";
    private static ServiceProvider? _serviceProvider;

    public static ServiceProvider BuildServiceProvider()
    {
        GlobalConfiguration.Setup().UseSqlServer();
        _serviceProvider ??= new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString))
            .AddTransient<IDbConnection>(sp => new SqlConnection(connectionString))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddMessagePipe()
            .BuildServiceProvider();
        return _serviceProvider;
    }
}