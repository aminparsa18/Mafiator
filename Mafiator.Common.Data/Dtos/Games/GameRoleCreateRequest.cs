using Mafiator.Common.Data.Enums;
using MessagePack;

namespace Mafiator.Common.Data.Dtos.Games
{
    /// <summary>
    /// Game role dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class GameRoleCreateRequest
    {
        /// <summary>
        /// Game role.
        /// </summary>
        [Key(0)]
        public GameRole Role { get; set; }

        /// <summary>
        /// Count.
        /// </summary>
        [Key(1)]
        public short Count { get; set; }
    }
}