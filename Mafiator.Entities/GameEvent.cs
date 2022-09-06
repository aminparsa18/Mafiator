using Mafiator.Common.Enums;
using System;

namespace Mafiator.Entities;

/// <summary>
/// Game events.
/// </summary>
public class GameEvent:BaseEntity
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Flag indicating that game event has been checked.
    /// </summary>
    public bool IsValidated{ get; set; }

    /// <summary>
    /// Indicates the member who made the event in game.
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Game event type.
    /// </summary>
    public GameEventType EventType { get; set; }

    /// <summary>
    /// Game.
    /// </summary>
    public virtual Game Game { get; set; }

    /// <summary>
    /// Game member.
    /// </summary>
    public virtual GameMember Member{ get; set; }
}