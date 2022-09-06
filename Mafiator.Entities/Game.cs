using Mafiator.Common.Data.Enums;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities;

/// <summary>
/// Game.
/// </summary>
public class Game:BaseEntity
{
    /// <summary>
    /// Game start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Game room key identifier.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Game status.
    /// </summary>
    public GameStatus Status { get; set; }

    /// <summary>
    /// Game capacity.
    /// </summary>
    public short Capacity { get; set; }

    /// <summary>
    /// Room.
    /// </summary>
    public virtual Room Room { get; set; }

    /// <summary>
    /// Collection of game events.
    /// </summary>
    public virtual ICollection<GameEvent> GameEvent{ get; set; }

    /// <summary>
    /// Collection of game members.
    /// </summary>
    public virtual ICollection<GameMember> GameMember { get; set; }

    /// <summary>
    /// Collection of game messages.
    /// </summary>
    public virtual ICollection<GameMessage> GameMessage { get; set; }

    /// <summary>
    /// Collection of game votes.
    /// </summary>
    public virtual ICollection<Vote> Vote{ get; set; }

    /// <summary>
    /// Collection of game violation reports.
    /// </summary>
    public virtual ICollection<GameViolationReport> Report{ get; set; }
}