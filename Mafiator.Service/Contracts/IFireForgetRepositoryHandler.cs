using Mafiator.Repository;
using System;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts
{
    public interface IFireForgetRepositoryHandler
    {
        void Execute(Func<IUnitOfWork, Task> databaseWork);
    }
}
