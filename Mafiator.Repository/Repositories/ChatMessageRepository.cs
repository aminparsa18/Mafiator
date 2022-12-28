using Mafiator.Common.Data.Dtos.ChatMessages;
using System;
using System.Data;
using System.Linq;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public sealed class ChatMessageRepository : BaseRepository<ChatMessage>, IChatMessageRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageRepository"/> class.
    /// </summary>
    public ChatMessageRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    /// <inheritdoc/>
    public Task<List<ChatMessageResult>> GetByRoom(Guid roomId)
    {
        return Context.ChatMessage.AsNoTracking().Where(c => c.RoomId == roomId)
            .Select(s => new ChatMessageResult()
            {
                Type = s.MessageType,
                Image = s.User.Image,
                DisplayName = s.User.DisplayName,
                Content = s.Content
            }).ToListAsync();
    }
}