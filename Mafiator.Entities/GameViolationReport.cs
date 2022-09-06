using Mafiator.Common.Enums;
using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities;

/// <summary>
/// Game violation report.
/// </summary>
public class GameViolationReport:BaseEntity
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Reporter ket identifier.
    /// </summary>
    public Guid ReporterId { get; set; }

    /// <summary>
    /// Reported key identifier.
    /// </summary>
    public Guid ReportedId { get; set; }

    /// <summary>
    /// Game violation type.
    /// </summary>
    public GameViolationType ViolationType { get; set; }

    /// <summary>
    /// Game.
    /// </summary>
    public virtual Game Game { get; set; }

    /// <summary>
    /// Reporter.
    /// </summary>
    public virtual User Reporter { get; set; }

    /// <summary>
    /// Reported.
    /// </summary>
    public virtual User Reported { get; set; }
}