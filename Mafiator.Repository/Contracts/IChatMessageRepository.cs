using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
    public interface IChatMessageRepository:IRepository<ChatMessage>
    {
        Task<List<ChatMessageDto>> GetByRoom(Ulid roomId);
    }
}
