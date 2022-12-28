using Mafiator.Common.Data.Enums;
using MemoryPack;
using System;

namespace Mafiator.Common.Data.Dtos.Games;

/// <summary>
/// Room game dto.
/// </summary>
[MemoryPackable]
public sealed partial class RoomGameResult
{
    /// <summary>
    /// Game start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Game status.
    /// </summary>
    public GameStatus Status { get; set; }
}