using Mafiator.Data.Dtos;
using Mafiator.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IChatMessageRepository:IRepository<ChatMessage>
    {
        Task<List<ChatMessageDto>> GetByRoom(Guid roomId);
    }
}
