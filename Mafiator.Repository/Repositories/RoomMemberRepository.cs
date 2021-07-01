using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using RepoDb;

namespace Mafiator.Repository.Repositories
{
   public class RoomMemberRepository:Repository<RoomMember>,IRoomMemberRepository
    {
        public RoomMemberRepository(ApplicationDbContext context,IDbConnection connection) : base(context,connection)
        {
        }

        public Task<IEnumerable<Ulid>> FindInRoom(Ulid roomId,Ulid userId)
        {
            return connection.ExecuteQueryAsync<Ulid>(@"SELECT TOP 1 [Id] FROM [RoomMember] Where [RoomId]=@RoomId AND [UserId]=@UserId ", new { RoomId = roomId.ToString(),UserId=userId.ToString() });

        }
        public Task<List<RoomMemberDto>> GetByRoom(Ulid roomId)
        {
            return _context.RoomMember.AsNoTracking().Where(r => r.RoomId == roomId)
                .OrderByDescending(o => o.CreatedDate)
                .Select(s => new RoomMemberDto()
                {
                    UserId = s.UserId,
                    Name = s.User.DisplayName,
                    Image = s.User.Image,
                    TotalGame = s.User.GameMember.Count
                }).ToListAsync();
        }
        public Task<IEnumerable<RoomMemberDto>> GetByRoomFast(string roomId)
        {
           return connection.ExecuteQueryAsync<RoomMemberDto>(@"SELECT [r].[UserId], [u].[DisplayName] AS [Name], [u].[Image], (
           SELECT COUNT(*)
           FROM[dbo].[GameMember] AS[g]
           WHERE[u].[Id] = [g].[UserId]) AS[TotalGame]
           FROM[dbo].[RoomMember] AS[r]
           INNER JOIN[dbo].[Users] AS[u] ON[r].[UserId] = [u].[Id]
           WHERE[r].[RoomId] = @RoomId
               ORDER BY[r].[CreatedDate] DESC ",new {RoomId=roomId});
        }
    }
}
