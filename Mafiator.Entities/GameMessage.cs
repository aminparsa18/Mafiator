using Mafiator.Common.Enums;
using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities;

/// <summary>
/// Game messages.
/// </summary>
public class GameMessage:BaseEntity
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Game owner user key identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Game message type.
    /// </summary>
    public GameMessageType MessageType { get; set; }

    /// <summary>
    /// Game message content.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Game.
    /// </summary>
    public virtual Game Game { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public virtual User User { get; set; }
}