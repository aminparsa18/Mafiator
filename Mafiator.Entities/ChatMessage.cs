using Mafiator.Common.Data.Enums;
using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities;

/// <summary>
/// Chat messages in a room.
/// </summary>
public sealed class ChatMessage : BaseEntity
{
    /// <summary>
    /// Room key identifier.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// User key identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Message type.
    /// </summary>
    public GameMessageType MessageType { get; set; }

    /// <summary>
    /// Message contents.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Room.
    /// </summary>
    public Room Room { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public User User { get; set; }
}