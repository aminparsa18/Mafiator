using Mafiator.Data.Dtos.Room;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class RoomMemberRepository : BaseRepository<RoomMember>, IRoomMemberRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoomMemberRepository"/> class.
    /// </summary>
    public RoomMemberRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Guid>> FindInRoom(Guid roomId, Guid userId)
    {
        return Connection.ExecuteQueryAsync<Guid>(@"SELECT TOP 1 [Id] FROM [RoomMember] Where [RoomId]=@RoomId AND [UserId]=@UserId ", new { RoomId = roomId.ToString(), UserId = userId.ToString() });

    }

    /// <inheritdoc/>
    public Task<List<RoomMemberDto>> GetByRoom(Guid roomId)
    {
        return Context.RoomMember.AsNoTracking().Where(r => r.RoomId == roomId)
            .OrderByDescending(o => o.CreatedDate)
            .Select(s => new RoomMemberDto()
            {
                UserId = s.UserId,
                Name = s.User.DisplayName,
                Image = s.User.Image,
                TotalGame = s.User.GameMember.Count
            }).ToListAsync();
    }

    /// <inheritdoc/>
    public Task<IEnumerable<RoomMemberDto>> GetByRoomFast(string roomId)
    {
        return Connection.ExecuteQueryAsync<RoomMemberDto>(@"SELECT [r].[UserId], [u].[DisplayName] AS [Name], [u].[Image], (
           SELECT COUNT(*)
           FROM[dbo].[GameMember] AS[g]
           WHERE[u].[Id] = [g].[UserId]) AS[TotalGame]
           FROM[dbo].[RoomMember] AS[r]
           INNER JOIN[dbo].[Users] AS[u] ON[r].[UserId] = [u].[Id]
           WHERE[r].[RoomId] = @RoomId
               ORDER BY[r].[CreatedDate] DESC ", new { RoomId = roomId });
    }
}