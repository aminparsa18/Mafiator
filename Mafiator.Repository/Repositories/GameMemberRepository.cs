using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Data.Dtos.Game;
using Mafiator.Data.Dtos.User;
using Mafiator.Repository.Cache;
using System;
using System.Data;
using System.Linq;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public sealed class GameMemberRepository : BaseRepository<GameMember>, IGameMemberRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameMemberRepository"/> class.
    /// </summary>
    public GameMemberRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<List<GameMemberResult>> GetByGame(Guid gameId)
    {
        return Context.GameMember.AsNoTracking().Where(g => g.UserId.HasValue && g.GameId == gameId).Select(s =>
            new GameMemberResult()
            {
                DisplayName = s.User.DisplayName,
                Image = s.User.Image,
                Score = s.User.Score,
                Id = s.Id.ToString(),
                Status = s.Status
            }).ToListAsync();
    }

    /// <inheritdoc/>
    public Task<IEnumerable<GameMemberResult>> GetByGameFast(string gameId)
    {
        return Connection.ExecuteQueryAsync<GameMemberResult>(
            @"SELECT [u].[DisplayName], [u].[Image], [u].[Score], [g].[Id],[g].[Status]
               FROM [dbo].[GameMember] AS [g]
               LEFT JOIN [dbo].[Users] AS [u] ON [g].[UserId] = [u].[Id]
               WHERE [g].[UserId] IS NOT NULL AND ([g].[GameId] = @gameId)", new { gameId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<WaitingPlayerResult>> GetWaitingPlayersByGame(string gameId)
    {
        return Connection.ExecuteQueryAsync<WaitingPlayerResult>(
            @"SELECT [u].[DisplayName], [u].[Image], [u].[Score], [g].[UserId],[g].[Status]
               FROM [dbo].[GameMember] AS [g]
               LEFT JOIN [dbo].[Users] AS [u] ON [g].[UserId] = [u].[Id]
               WHERE [g].[UserId] IS NOT NULL AND ([g].[GameId] = @gameId)", new { gameId });
    }

    /// <inheritdoc/>
    public Task<List<UserGameStatus>> GetUserStatus(Guid userId)
    {
        return Context.GameMember.AsNoTracking().Where(g => g.UserId == userId).Select(s => new UserGameStatus()
        {
            GameStatus = s.Game.Status,
            GameRole = s.Role
        }).ToListAsync();
    }

    /// <inheritdoc/>
    public Task<IEnumerable<UserGameStatus>> GetUserStatusFast(string userId)
    {
        return Connection.ExecuteQueryAsync<UserGameStatus>(
            @"SELECT [g0].[Status] AS [GameStatus], [g].[Role] AS [GameRole],[g].[Id] as [MemberId]
            FROM [dbo].[GameMember] AS [g]
            INNER JOIN[dbo].[Game] AS[g0] ON[g].[GameId] = [g0].[Id]
            WHERE[g].[UserId] = @userId", new { userId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<UserGameStatus>> GetPlayerStatusFast(string memberId)
    {
        return Connection.ExecuteQueryAsync<UserGameStatus>(
            @"SELECT [g].[Status] AS [GameStatus], [g].[Role] AS [GameRole]
            FROM [dbo].[GameMember] g WHERE [g].[Id] = @memberId", new { memberId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<PlayerRoleResult>> GetPlayerByRoleFast(string gameId, short role)
    {
        return Connection.ExecuteQueryAsync<PlayerRoleResult>(
            @"SELECT [g].[Id] AS [MemberId] FROM [dbo].[GameMember] g WHERE [g].[GameId] = @gameId AND [g].[Role] = @role", new { gameId, role });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<PlayerRoleResult>> GetUser(string userId, string gameId)
    {
        return Connection.ExecuteQueryAsync<PlayerRoleResult>(
            @"SELECT TOP(1) g.[Id] AS [MemberId]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[UserId] = @userId) AND ([g].[GameId] = @gameId)", new { userId, gameId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<PlayerRoleResult>> GetRoleOfPlayer(string userId, string gameId)
    {
        return Connection.ExecuteQueryAsync<PlayerRoleResult>(
            @"SELECT TOP(1) [g].[Role],g.[Id] AS [MemberId]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[UserId] = @userId) AND ([g].[GameId] = @gameId)", new { userId, gameId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<PlayerRoleResult>> GetMafiaPartners(string memberId, string gameId)
    {
        return Connection.ExecuteQueryAsync<PlayerRoleResult>(
            @"SELECT [g].[Role],g.[Id] AS [MemberId]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[Id] <> @memberId) AND ([g].[GameId] = @gameId) AND ([g].[Role] = 0 OR  [g].[Role] = 1)",
            new { memberId, gameId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<PlayerDetails>> GetPlayerByGame(string gameId)
    {
        return Connection.ExecuteQueryAsync<PlayerDetails>(
            @"SELECT [g].[UserId],[g].[Id] AS MemberId,[g].[Role],[g].[Status] FROM [dbo].[GameMember] AS [g] WHERE [g].[GameId] = @gameId AND g.[Status] = 0",
            new { gameId }, cacheKey: $"GamePlayers-{gameId}", cache: CacheFactory.GetCache());
    }

    /// <inheritdoc/>
    public Task<IEnumerable<PlayerDetails>> GetUsersByGame(string gameId)
    {
        return Connection.ExecuteQueryAsync<PlayerDetails>(
            @"SELECT [g].[UserId],[g].[Id] As MemberId FROM [dbo].[GameMember] AS [g] WHERE[g].[GameId] = @gameId", new { gameId });
    }

    /// <inheritdoc/>
    public Task<int> CountPlayerByGame(string gameId)
    {
        return Connection.ExecuteScalarAsync<int>(
            @"SELECT COUNT(*) FROM [dbo].[GameMember] AS [g] WHERE[g].[GameId] = @gameId", new { gameId },
            cacheKey: $"GamePlayersCount-{gameId}", cache: CacheFactory.GetCache());
    }

    /// <inheritdoc/>
    public Task<int> Join(string userId, string memberId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [UserId] = @userId WHERE [Id] = @memberId",
            new { userId, memberId });
    }

    /// <inheritdoc/>
    public Task<int> Leave(string userId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [UserId] = NULL WHERE [UserId] = @userId",
            new { userId });
    }

    /// <inheritdoc/>
    public Task<int> KickMember(string memberId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [Status] = 2 WHERE [Id] = @id",
            new { id = memberId });
    }

    /// <inheritdoc/>
    public Task<int> KillMember(string memberId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [Status] = 1 WHERE [Id] = @id",
            new { id = memberId });
    }

    /// <inheritdoc/>
    public Task<int> SilenceMember(string memberId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [GameMember] SET [Status] = 3 WHERE [Id] = @id",
            new { id = memberId });
    }
}