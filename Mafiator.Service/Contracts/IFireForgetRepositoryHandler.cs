using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mafiator.Repository;
using Mafiator.Repository.Contracts;

namespace Mafiator.Service.Contracts
{
    public interface IFireForgetRepositoryHandler
    {
        void Execute(Func<IUnitOfWork, Task> databaseWork);
    }
}
