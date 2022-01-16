using Mafiator.Data;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using System.Data;

namespace Mafiator.Repository.Repositories
{
    public class GameMessageRepository:Repository<GameMessage>,IGameMessageRepository
    {
        public GameMessageRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }
    }
}
