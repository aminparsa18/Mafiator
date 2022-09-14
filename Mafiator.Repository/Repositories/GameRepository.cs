using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Dtos.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DateTime = System.DateTime;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public sealed class GameRepository : BaseRepository<Game>, IGameRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameRepository"/> class.
    /// </summary>
    public GameRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<RoomGameResult>> GetByRoom(string roomId)
    {
        return Connection.ExecuteQueryAsync<RoomGameResult>(@"SELECT [g].[Status], [g].[StartDate]
            FROM[Game] AS[g]
            WHERE[g].[RoomId] = @RoomId
            ORDER BY[g].[StartDate] DESC", new { RoomId = roomId });
    }

    /// <inheritdoc/>
    public Task<AppointedGameResult> GetWaitingGameInformations(Guid gameId)
    {
        return Context.Game.AsNoTracking().Where(g => g.Id == gameId && (g.Status == GameStatus.NotStarted || g.Status == GameStatus.Playing))
            .Select(s => new AppointedGameResult()
            {
                Id = s.Id,
                StartDate = s.StartDate,
                Roles = s.GameMember.Select(s => s.Role).ToList(),
                Status = s.Status,
                Members = s.GameMember.Where(m => m.UserId.HasValue).Select(x => new WaitingPlayerResult()
                {
                    DisplayName = x.User.DisplayName,
                    Image = x.User.Image,
                    UserId = x.UserId.Value.ToString(),
                    Score = x.User.Score
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public Task<AppointedGameResult> GetWaitingGameByRoom(Guid roomId)
    {
        return Context.Game.AsNoTracking().Where(g => g.RoomId == roomId && (g.Status == GameStatus.NotStarted || g.Status == GameStatus.Playing) && g.StartDate < DateTime.Now)
            .Select(s => new AppointedGameResult()
            {
                Id = s.Id,
                StartDate = s.StartDate,
                Roles = s.GameMember.Select(s => s.Role).ToList(),
                Members = s.GameMember.Where(m => m.UserId.HasValue).Select(x => new WaitingPlayerResult()
                {
                    DisplayName = x.User.DisplayName,
                    Image = x.User.Image,
                    UserId = x.UserId.Value.ToString(),
                    Score = x.User.Score
                }).ToList(),
                Status = s.Status
            }).FirstOrDefaultAsync();

    }

    /// <inheritdoc/>
    public Task<List<AvailableGameResult>> GetAvailables()
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
        return Context.Game.Where(g => g.Status == GameStatus.NotStarted && g.GameMember.Count(m => m.UserId != null) < g.GameMember.Count)
            .Select(s => new AvailableGameResult()
            {
                Capacity = (short)s.GameMember.Count(m => !m.UserId.HasValue),
                MemberCount = (short)s.GameMember.Count,
                Id = s.Id,
                RoomId = s.RoomId,
                StartDate = s.StartDate
            }).ToListAsync();
    }

    /// <inheritdoc/>
    public Task<GameMember> IsJoined(Guid userId, Guid gameId)
    {
        return Context.GameMember.FirstOrDefaultAsync(m => m.UserId == userId && m.GameId == gameId);
    }

    /// <inheritdoc/>
    public Task<string> IsJoinedFast(string userId, string gameId)
    {
        return Connection.ExecuteScalarAsync<string>(
            @"SELECT TOP(1) [g].[Id]
                  FROM [dbo].[GameMember] AS [g]
                  WHERE ([g].[UserId] = @userId) AND ([g].[GameId] = @gameId)", new { userId, gameId });
    }

    /// <inheritdoc/>
    public Task<string> IsAlreadyPlaying(string roomId)
    {
        return Connection.ExecuteScalarAsync<string>(
            @"SELECT TOP(1) [g].[Id]
                  FROM [dbo].[Game] AS [g]
                  WHERE ([g].[RoomId] = @roomId) AND ([g].[Status] = 0 OR [g].[Status] = 1)", new { roomId });
    }

    /// <inheritdoc/>
    public Task<int> StartGame(string gameId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [Game] SET [Status] = 1 WHERE [Id] = @gameId",
            new { gameId });
    }

    /// <inheritdoc/>
    public Task<int> MafiaWin(string gameId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [Game] SET [Status] = 2 WHERE [Id] = @gameId",
            new { gameId });
    }

    /// <inheritdoc/>
    public Task<int> CitizenWin(string gameId)
    {
        return Connection.ExecuteNonQueryAsync("UPDATE [Game] SET [Status] = 3 WHERE [Id] = @gameId",
            new { gameId });
    }
}