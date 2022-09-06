using Mafiator.Common.Data.Enums;
using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities;

/// <summary>
/// Game member.
/// </summary>
public class GameMember:BaseEntity
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
    public PlayerStatus Status{ get; set; } 

    /// <summary>
    /// Game.
    /// </summary>
    public virtual Game Game { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public virtual User User { get; set; }

    /// <summary>
    /// Collection of voters.
    /// </summary>
    public virtual ICollection<Vote> Voter{ get; set; }

    /// <summary>
    /// Collection of targets.
    /// </summary>
    public virtual ICollection<Vote> Target{ get; set; }
    
    /// <summary>
    /// Collection of game events.
    /// </summary>
    public virtual ICollection<GameEvent> GameEvent{ get; set; }
}