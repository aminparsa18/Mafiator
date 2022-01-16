using Mafiator.Data;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using System.Data;

namespace Mafiator.Repository.Repositories
{
    public class GameViolationReportRepository:Repository<GameViolationReport>,IGameViolationReportRepository
    {
        public GameViolationReportRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }
    }
}
