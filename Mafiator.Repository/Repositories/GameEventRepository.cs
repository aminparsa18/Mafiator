using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using RepoDb;

namespace Mafiator.Repository.Repositories
{
   public class GameEventRepository:Repository<GameEvent>,IGameEventRepository
    {
        public GameEventRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }

        public Task<IEnumerable<GameEventStatusDto>> GetByGame(string gameId)
        {
            return connection.ExecuteQueryAsync<GameEventStatusDto>("SELECT [g].[MemberId],[g].[EventType] FROM [GameEvent] g WHERE g.[GameId] = @gameId AND g.[IsValidated] = 0",new{gameId});
        }

        public Task<int> Validate(string gameId)
        {
            return connection.ExecuteNonQueryAsync("UPDATE [GameEvent] SET [IsValidated] = 1 WHERE [GameId] = @gameId", new { gameId });

        }
    }
}
