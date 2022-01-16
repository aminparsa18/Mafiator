using Mafiator.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Impl
{
    public class FireForgetRepositoryHandler : IFireForgetRepositoryHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FireForgetRepositoryHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Execute(Func<IUnitOfWork, Task> databaseWork)
        {
            // Fire off the task, but don't await the result
            Task.Run(async () =>
            {
                // Exceptions must be caught
                    using var scope = _serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await databaseWork(repository);
            });
        }
    }
}