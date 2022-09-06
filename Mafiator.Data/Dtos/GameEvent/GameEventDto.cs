using Mafiator.Common.Enums;
using System;

namespace Mafiator.Data.Dtos.GameEvent;

/// <summary>
/// Game event dto.
/// </summary>
[MessagePackObject()]
public class GameEventDto
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    [Key(0)]
    public Guid GameId { get; set; }

    /// <summary>
    /// Game member key identifier.
    /// </summary>
    [Key(1)]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Game event type.
    /// </summary>
    [Key(2)]
    public GameEventType EventType { get; set; }
}
