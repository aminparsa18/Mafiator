using Mafiator.Common.Data.Dtos.ChatMessages;
using Mafiator.Repository;
using Mafiator.Service.Contracts.ChatMessages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.ChatMessages;

public class ChatMessageService : IChatMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatMessageService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<List<ChatMessageResult>> GetByRoom(Guid roomId)
    {
        var chats = await _unitOfWork.ChatMessage.GetByRoom(roomId);

        // set full uri path on images
        chats.ForEach(c => c.Image = $"{Data.Constants.BlobStorageEndpoint}{c.Image}");
        return chats;
    }
}