using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Games
{
    /// <summary>
    /// Game dto.
    /// </summary>
    [MessagePackObject()]
    public sealed class AvailableGameResult
    {
        /// <summary>
        /// Game key identifier.
        /// </summary>
        [Key(0)]
        public Guid Id { get; set; }

        /// <summary>
        /// Capacity.
        /// </summary>
        [Key(1)]
        public short Capacity { get; set; }

        /// <summary>
        /// Room key identifier.
        /// </summary>
        [Key(2)]
        public Guid RoomId { get; set; }

        /// <summary>
        /// Game member count.
        /// </summary>
        [Key(3)]
        public short MemberCount { get; set; }

        /// <summary>
        /// Game start date.
        /// </summary>
        [Key(4)]
        public DateTime StartDate { get; set; }
    }
}