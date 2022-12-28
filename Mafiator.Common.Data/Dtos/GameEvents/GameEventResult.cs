using Mafiator.Common.Data.Enums;
using MemoryPack;

namespace Mafiator.Data.Dtos.GameEvent;

/// <summary>
/// Game event result dto.
/// </summary>
[MemoryPackable]
public sealed partial class GameEventResult
{
    /// <summary>
    /// Game member key identifier.
    /// </summary>
    public string MemberId { get; set; }

    /// <summary>
    /// Game event type.
    /// </summary>
    public GameEventType EventType { get; set; }
}