using Mafiator.Common.Data.Dtos.ChatMessages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.ChatMessages;

public interface IChatMessageService
{
    Task<List<ChatMessageResult>> GetByRoom(Guid roomId);
}