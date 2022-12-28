using Mafiator.Common.Data.Dtos.ChatMessages;
using System;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle chat message data.
/// </summary>
public interface IChatMessageRepository : IBaseRepository<ChatMessage>
{
    /// <summary>
    /// Retrieves all chat messages in a room.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>List of chat messages.</returns>
    Task<List<ChatMessageResult>> GetByRoom(Guid roomId);
}