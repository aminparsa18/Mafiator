using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Repository.Repositories
{
    public class ChatMessageRepository:Repository<ChatMessage>,IChatMessageRepository
    {
        public ChatMessageRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
        {
        }

        public Task<List<ChatMessageDto>> GetByRoom(Guid roomId)
        {
            return Context.ChatMessage.AsNoTracking().Where(c => c.RoomId == roomId)
                .Select(s => new ChatMessageDto()
                {
                    Type = s.MessageType,
                    Image = s.User.Image,
                    DisplayName = s.User.DisplayName,
                    Content = s.Content
                }).ToListAsync();
        }
    }
}
