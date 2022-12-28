using System;

namespace Mafiator.Entities.Models;

/// <summary>
/// Vote.
/// </summary>
public sealed class Vote : BaseEntity
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
    public Game Game { get; set; }

    /// <summary>
    /// Voter.
    /// </summary>
    public GameMember Voter { get; set; }

    /// <summary>
    /// Target.
    /// </summary>
    public GameMember Target { get; set; }
}