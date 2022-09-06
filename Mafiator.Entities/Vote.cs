using System;

namespace Mafiator.Entities;

/// <summary>
/// Vote.
/// </summary>
public class Vote:BaseEntity
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Voter key identifier.
    /// </summary>
    public Guid VoterId { get; set; }

    /// <summary>
    /// Target key identifier.
    /// </summary>
    public Guid TargetId { get; set; }

    /// <summary>
    /// Indicates vote is validated.
    /// </summary>
    public bool IsValidated { get; set; }

    /// <summary>
    /// Game.
    /// </summary>
    public virtual Game Game { get; set; }

    /// <summary>
    /// Voter.
    /// </summary>
    public virtual GameMember Voter { get; set; }

    /// <summary>
    /// Target.
    /// </summary>
    public virtual GameMember Target { get; set; }
}