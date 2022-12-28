using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Enums;
using MemoryPack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.Games;

/// <summary>
/// Waiting game dto.
/// </summary>
[MemoryPackable]
public sealed partial class AppointedGameResult
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Game start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// List of game roles.
    /// </summary>
    public List<GameRole> Roles { get; set; }

    /// <summary>
    /// List of waiting game members.
    /// </summary>
    public List<WaitingPlayerResult> Members { get; set; }

    /// <summary>
    /// Game status.
    /// </summary>
    public GameStatus Status { get; set; }
}