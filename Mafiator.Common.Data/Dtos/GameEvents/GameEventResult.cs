using Mafiator.Common.Data.Enums;
using MessagePack;

namespace Mafiator.Data.Dtos.GameEvent
{
    /// <summary>
    /// Game event result dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class GameEventResult
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
}