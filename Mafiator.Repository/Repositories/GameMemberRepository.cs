using Mafiator.Data;
using Mafiator.Data.Dtos.Game;
using Mafiator.Data.Dtos.User;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Repository.Repositories
{
    public class GameMemberRepository : Repository<GameMember>, IGameMemberRepository
    {
        public GameMemberRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
        {
        }

        public Task<List<GameMemberDto>> GetByGame(Guid gameId)
        {
            return Context.GameMember.AsNoTracking().Where(g => g.UserId.HasValue && g.GameId == gameId).Select(s =>
                new GameMemberDto()
                {
                    DisplayName = s.User.DisplayName,
                    Image = s.User.Image,
                    Score = s.User.Score,
                    Id = s.Id.ToString(),
                    Status = s.Status
                }).ToListAsync();
        }

        public Task<IEnumerable<GameMemberDto>> GetByGameFast(string gameId)
        {
            return Connection.ExecuteQueryAsync<GameMemberDto>(
                @"SELECT [u].[DisplayName], [u].[Image], [u].[Score], [g].[Id],[g].[Status]
               FROM [dbo].[GameMember] AS [g]
               LEFT JOIN [dbo].[Users] AS [u] ON [g].[UserId] = [u].[Id]
               WHERE [g].[UserId] IS NOT NULL AND ([g].[GameId] = @gameId)", new {gameId});
        }

        public Task<IEnumerable<WaitingPlayerDto>> GetWaitingPlayersByGame(string gameId)
        {
            return Connection.ExecuteQueryAsync<WaitingPlayerDto>(
                @"SELECT [u].[DisplayName], [u].[Image], [u].[Score], [g].[UserId],[g].[Status]
               FROM [dbo].[GameMember] AS [g]
               LEFT JOIN [dbo].[Users] AS [u] ON [g].[UserId] = [u].[Id]
               WHERE [g].[UserId] IS NOT NULL AND ([g].[GameId] = @gameId)", new {gameId});
        }


        public Task<List<UserGameStatusDto>> GetUserStatus(Guid userId)
        {
            return Context.GameMember.AsNoTracking().Where(g => g.UserId == userId).Select(s => new UserGameStatusDto()
            {
                GameStatus = s.Game.Status,
                GameRole = s.Role
            }).ToListAsync();
        }

        public Task<IEnumerable<UserGameStatusDto>> GetUserStatusFast(string userId)
        {
            return Connection.ExecuteQueryAsync<UserGameStatusDto>(
                @"SELECT [g0].[Status] AS [GameStatus], [g].[Role] AS [GameRole],[g].[Id] as [MemberId]
            FROM [dbo].[GameMember] AS [g]
            INNER JOIN[dbo].[Game] AS[g0] ON[g].[GameId] = [g0].[Id]
            WHERE[g].[UserId] = @userId", new {userId});
        }

        public Task<IEnumerable<UserGameStatusDto>> GetPlayerStatusFast(string memberId)
        {
            return Connection.ExecuteQueryAsync<UserGameStatusDto>(
                @"SELECT [g].[Status] AS [GameStatus], [g].[Role] AS [GameRole]
            FROM [dbo].[GameMember] g WHERE [g].[Id] = @memberId" , new { memberId});
        }

        public Task<IEnumerable<PlayerRoleDto>> GetPlayerByRoleFast(string gameId,short role)
        {
            return Connection.ExecuteQueryAsync<PlayerRoleDto>(
                @"SELECT [g].[Id] AS [MemberId] FROM [dbo].[GameMember] g WHERE [g].[GameId] = @gameId AND [g].[Role] = @role", new { gameId,role });
        }

        public Task<IEnumerable<PlayerRoleDto>> GetUser(string userId, string gameId)
        {
            return Connection.ExecuteQueryAsync<PlayerRoleDto>(
                @"SELECT TOP(1) g.[Id] AS [MemberId]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[UserId] = @userId) AND ([g].[GameId] = @gameId)", new { userId, gameId });
        }


        public Task<IEnumerable<PlayerRoleDto>> GetRoleOfPlayer(string userId, string gameId)
        {
            return Connection.ExecuteQueryAsync<PlayerRoleDto>(
                @"SELECT TOP(1) [g].[Role],g.[Id] AS [MemberId]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[UserId] = @userId) AND ([g].[GameId] = @gameId)", new {userId, gameId});
        }

        public Task<IEnumerable<PlayerRoleDto>> GetMafiaPartners(string memberId, string gameId)
        {
            return Connection.ExecuteQueryAsync<PlayerRoleDto>(
                @"SELECT [g].[Role],g.[Id] AS [MemberId]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[Id] <> @memberId) AND ([g].[GameId] = @gameId) AND ([g].[Role] = 0 OR  [g].[Role] = 1)",
                new {memberId, gameId});
        }

        public Task<IEnumerable<PlayerDto>> GetPlayerByGame(string gameId)
        {
            return Connection.ExecuteQueryAsync<PlayerDto>(
                @"SELECT [g].[UserId],[g].[Id] AS MemberId,[g].[Role],[g].[Status] FROM [dbo].[GameMember] AS [g] WHERE [g].[GameId] = @gameId AND g.[Status] = 0",
                new {gameId}, cacheKey: $"GamePlayers-{gameId}", cache: CacheFactory.GetCache());
        }

        public Task<IEnumerable<PlayerDto>> GetUsersByGame(string gameId)
        {
            return Connection.ExecuteQueryAsync<PlayerDto>(
                @"SELECT [g].[UserId],[g].[Id] As MemberId FROM [dbo].[GameMember] AS [g] WHERE[g].[GameId] = @gameId", new {gameId});
        }

        public Task<int> CountPlayerByGame(string gameId)
        {
            return Connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) FROM [dbo].[GameMember] AS [g] WHERE[g].[GameId] = @gameId", new {gameId},
                cacheKey: $"GamePlayersCount-{gameId}", cache: CacheFactory.GetCache());
        }

        public Task<int> Join(string userId, string memberId)
        {
            return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [UserId] = @userId WHERE [Id] = @memberId",
                new {userId, memberId});
        }
        public Task<int> Leave(string userId)
        {
            return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [UserId] = NULL WHERE [UserId] = @userId",
                new { userId});
        }
        public Task<int> KickMember(string memberId)
        {
            return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [Status] = 2 WHERE [Id] = @id",
                new {id=memberId});
        }

        public Task<int> KillMember(string memberId)
        {
            return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [Status] = 1 WHERE [Id] = @id",
                new {id=memberId});
        }

        public Task<int> SilenceMember(string memberId)
        {
            return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [Status] = 3 WHERE [Id] = @id",
                new {id=memberId});
        }
    }
}