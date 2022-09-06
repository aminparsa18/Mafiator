using Mafiator.Common.Enums;

namespace Mafiator.Data.Dtos.GameEvent;

/// <summary>
/// Game event status dto.
/// </summary>
[MessagePackObject()]
public class GameEventStatusDto
{
    /// <summary>
    /// Game member key identifier.
    /// </summary>
    [Key(0)]
    public string MemberId { get; set; }

    /// <summary>
    /// Game event type.
    /// </summary>
    [Key(1)]
    public GameEventType EventType { get; set; }
}