using Mafiator.Common.Data.Enums;
using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities;

/// <summary>
/// Game member.
/// </summary>
public sealed class GameMember : BaseEntity
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Game member user key identifier.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Game mamber role.
    /// </summary>
    public GameRole Role { get; set; }

    /// <summary>
    /// Player status.
    /// </summary>
    public PlayerStatus Status { get; set; }

    /// <summary>
    /// Game.
    /// </summary>
    public Game Game { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Collection of voters.
    /// </summary>
    public ICollection<Vote> Voter { get; set; }

    /// <summary>
    /// Collection of targets.
    /// </summary>
    public ICollection<Vote> Target { get; set; }

    /// <summary>
    /// Collection of game events.
    /// </summary>
    public ICollection<GameEvent> GameEvent { get; set; }
}