using Mafiator.Common.Data.Enums;
using MemoryPack;
using System;

namespace Mafiator.Data.Dtos.GameEvent;

/// <summary>
/// Game event dto.
/// </summary>
[MemoryPackable]
public sealed partial class GameEventRequest
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Game member key identifier.
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Game event type.
    /// </summary>
    public GameEventType EventType { get; set; }
}