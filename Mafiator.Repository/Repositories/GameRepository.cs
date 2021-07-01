using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Entities.Enums;
using Mafiator.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using RepoDb;
using DateTime = System.DateTime;

namespace Mafiator.Repository.Repositories
{
   public class GameRepository:Repository<Game>,IGameRepository
    {
        public GameRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }

        public Task<IEnumerable<RoomGameDto>> GetByRoom(string roomId)
        {
            return connection.ExecuteQueryAsync<RoomGameDto>(@"SELECT [g].[Status], [g].[StartDate]
            FROM[Game] AS[g]
            WHERE[g].[RoomId] = @RoomId
            ORDER BY[g].[StartDate] DESC",new{RoomId=roomId});
        }

        public Task<WaitingGameDto> GetWaitingGameByGame(Ulid gameId)
        {
            return _context.Game.AsNoTracking().Where(g => g.Id == gameId && (g.Status == GameStatus.NotStarted || g.Status == GameStatus.Playing))
                .Select(s => new WaitingGameDto()
                {
                    Id = s.Id,
                    Date = s.StartDate,
                    Roles = s.GameMember.Select(s => s.Role).ToList(),
                    Status = s.Status,
                    Members = s.GameMember.Where(m => m.UserId.HasValue).Select(x => new WaitingGameMemberDto()
                    {
                        DisplayName = x.User.DisplayName,
                        Image = x.User.Image,
                        UserId = x.UserId.Value.ToString(),
                        Score = x.User.Score
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public Task<WaitingGameDto> GetWaitingGameByRoom(Ulid roomId)
        {
            return _context.Game.AsNoTracking().Where(g => g.RoomId == roomId && (g.Status==GameStatus.NotStarted || g.Status==GameStatus.Playing) && g.StartDate < DateTime.Now)
                .Select(s => new WaitingGameDto()
                {
                    Id = s.Id,
                    Date = s.StartDate,
                    Roles = s.GameMember.Select(s => s.Role).ToList(),
                    Members = s.GameMember.Where(m => m.UserId.HasValue).Select(x => new WaitingGameMemberDto()
                    {
                        DisplayName = x.User.DisplayName,
                        Image = x.User.Image,
                        UserId = x.UserId.Value.ToString(),
                        Score = x.User.Score
                    }).ToList(),
                    Status = s.Status
                }).FirstOrDefaultAsync();

        }

        public Task<List<GameDto>> GetAvailables()
        {
            //            return connection.ExecuteQueryAsync<GameDto>(@"SELECT CAST((
            //    SELECT COUNT(*)
            //    FROM [dbo].[GameMember] AS [g]
            //    WHERE ([g0].[Id] = [g].[GameId]) AND [g].[UserId] IS NULL) AS smallint) AS [Capacity], [g0].[Id], [g0].[RoomId], [r].[Image] AS [RoomImage]
            //FROM [dbo].[Game] AS [g0]
            //INNER JOIN [dbo].[Room] AS [r] ON [g0].[RoomId] = [r].[Id]
            //WHERE ([g0].[Status] = CAST(0 AS smallint)) AND ((
            //    SELECT COUNT(*)
            //    FROM [dbo].[GameMember] AS [g1]
            //    WHERE ([g0].[Id] = [g1].[GameId]) AND [g1].[UserId] IS NOT NULL) < (
            //    SELECT COUNT(*)
            //    FROM [dbo].[GameMember] AS [g2]
            //    WHERE [g0].[Id] = [g2].[GameId])) ");
            return _context.Game.Where(g => g.Status == GameStatus.NotStarted && g.GameMember.Count(m => m.UserId != null) < g.GameMember.Count)
                .Select(s => new GameDto()
                {
                    Capacity = (short)s.GameMember.Count(m => !m.UserId.HasValue),
                    Count = (short)s.GameMember.Count,
                    Id = s.Id,
                    RoomId = s.RoomId,
                    Date = s.StartDate
                }).ToListAsync();
        }
        public Task<GameMember> IsJoined(Ulid userId, Ulid gameId)
        {
            return _context.GameMember.FirstOrDefaultAsync(m => m.UserId == userId && m.GameId == gameId);
        }
        public Task<string> IsJoinedFast(string userId, string gameId)
        {
            return connection.ExecuteScalarAsync<string>(
                @"SELECT TOP(1) [g].[Id]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[UserId] = @userId) AND ([g].[GameId] = @gameId)", new { userId, gameId });
        }
        public Task<string> IsAlreadyPlaying(string roomId)
        {
            return connection.ExecuteScalarAsync<string>(
                @"SELECT TOP(1) [g].[Id]
                  FROM [dbo].[Game] AS [g]
                  WHERE ([g].[RoomId] = @roomId) AND ([g].[Status] = 0 OR [g].[Status] = 1)", new { roomId});
        }

        public Task<int> StartGame(string gameId)
        {
            return connection.ExecuteNonQueryAsync("UPDATE [Game] SET [Status] = 1 WHERE [Id] = @gameId",
                new { gameId });
        }
    }
}
