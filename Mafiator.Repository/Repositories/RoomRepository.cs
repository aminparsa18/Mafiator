using Mafiator.Data;
using Mafiator.Data.Dtos.Room;
using Mafiator.Entities;
using Mafiator.Entities.Enums;
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
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
        {
        }

        public Task<List<RoomDto>> GetDtoPage(int skip)
        {
            return Context.Room.AsNoTracking().Where(r => !r.IsPrivate)
                .Skip(skip).Take(20).OrderByDescending(o => o.CreatedDate).Select(s => new RoomDto()
                {
                    Id = s.Id,
                    MemberCount = (short) s.RoomMember.Count,
                    GamePlayedCount = s.Game.Count(g => g.Status != GameStatus.NotStarted),
                    Name = s.Name
                }).ToListAsync();
        }

        public Task<IEnumerable<RoomDto>> GetDtoPageFast(int skip)
        {
            return Connection.ExecuteQueryAsync<RoomDto>(@"SELECT [t].[Id], [t].[Image], CAST((
    SELECT COUNT(*) FROM [RoomMember] AS [r]
    WHERE [t].[Id] = [r].[RoomId]) AS smallint) AS [MemberCount], (
    SELECT COUNT(*) FROM [Game] AS [g]
    WHERE ([t].[Id] = [g].[RoomId]) AND ([g].[Status] <> @Status)) AS [GamePlayedCount], [t].[Name]
    FROM (
    SELECT [r0].[Id], [r0].[CreatedDate], [r0].[Image], [r0].[IsPrivate], [r0].[ModifiedDate], [r0].[Name], [r0].[UserId]
    FROM [Room] AS [r0] WHERE [r0].[IsPrivate] <> CAST(1 AS bit)
    ORDER BY (SELECT 1) OFFSET @Offset ROWS FETCH NEXT @Take ROWS ONLY
    ) AS [t] ORDER BY [t].[CreatedDate] DESC", new {Status = GameStatus.NotStarted, Offset = skip, Take = 20});
        }

        public Task<RoomDto> GetRoom(string roomId)
        {
            var parsed = Guid.Parse(roomId);
            return Context.Room.AsNoTracking().Where(r => r.Id == parsed)
                .Select(s => new RoomDto()
                {
                    Id = s.Id,
                    MemberCount = (short) s.RoomMember.Count,
                    GamePlayedCount = s.Game.Count(g => g.Status != GameStatus.NotStarted),
                    Name = s.Name
                }).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<RoomDto>> GetRoomFast(string roomId)
        {
            return Connection.ExecuteQueryAsync<RoomDto>(@"SELECT TOP(1) CAST((
            SELECT COUNT(*)
            FROM[dbo].[RoomMember] AS[r]
            WHERE[r0].[Id] = [r].[RoomId]) AS smallint) AS[MemberCount], (
                SELECT COUNT(*)
            FROM[dbo].[Game] AS[g]
            WHERE([r0].[Id] = [g].[RoomId]) AND([g].[Status] <> CAST(0 AS smallint))) AS[GamePlayedCount], [r0].[Name],[r0].[Code]
            FROM[dbo].[Room] AS[r0]
            WHERE[r0].[Id] = @roomId", new {roomId});
        }

        public Task<List<RoomDto>> GetMyRooms(Guid userId)
        {
            return Context.RoomMember.AsNoTracking().Where(r => r.UserId == userId)
                .Select(s => new RoomDto()
                {
                    Id = s.RoomId,
                    MemberCount = (short) s.Room.RoomMember.Count,
                    GamePlayedCount = s.Room.Game.Count(g => g.Status != GameStatus.NotStarted),
                    Name = s.Room.Name,
                    IsAdmin = s.Room.UserId == userId
                }).ToListAsync();
        }

        public Task<IEnumerable<RoomDto>> GetMyRoomsFast(Guid userId)
        {
            return Connection.ExecuteQueryAsync<RoomDto>(@"SELECT [r0].[RoomId] AS [Id], CAST((
    SELECT COUNT(*)
    FROM [dbo].[RoomMember] AS [r]
    WHERE [r1].[Id] = [r].[RoomId]) AS smallint) AS [MemberCount], (
    SELECT COUNT(*)
    FROM [dbo].[Game] AS [g]
    WHERE ([r1].[Id] = [g].[RoomId]) AND ([g].[Status] <> CAST(0 AS smallint))) AS [GamePlayedCount],[r1].[Code],[r1].[Name], CASE
    WHEN [r1].[UserId] = @userId THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
    END AS [IsAdmin]
    FROM [dbo].[RoomMember] AS [r0]
    INNER JOIN [dbo].[Room] AS [r1] ON [r0].[RoomId] = [r1].[Id]
    WHERE [r0].[UserId] = @userId ", new {userId = userId.ToString()});
        }

        public Task<RoomMember> IsJoined(Guid userId, Guid roomId)
        {
            return Context.RoomMember.FirstOrDefaultAsync(m => m.UserId == userId && m.RoomId == roomId);
        }

        public Task<string> IsJoinedFast(string userId, string roomId)
        {
            return Connection.ExecuteScalarAsync<string>(
                @"SELECT TOP(1) [r].[Id]
                  FROM [dbo].[RoomMember] AS [r]
                  WHERE ([r].[UserId] = @userId) AND ([r].[RoomId] = @roomId)", new {userId, roomId});
        }
        public Task<string> GetByCode(string code)
        {
            return Connection.ExecuteScalarAsync<string>(
                @"SELECT TOP(1) [r].[Id]
                  FROM [dbo].[Room] AS [r]
                  WHERE ([r].[Code] = @code)", new { code });
        }
    }
}