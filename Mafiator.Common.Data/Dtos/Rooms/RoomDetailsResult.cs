using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Rooms
{
    /// <summary>
    /// Room details result.
    /// </summary>
    [MessagePackObject()]
    public sealed class RoomDetailsResult
    {
        /// <summary>
        /// Room key identifier.
        /// </summary>
        [Key(0)]
        public Guid Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [Key(1)]
        public string Name { get; set; }

        /// <summary>
        /// Code.
        /// </summary>
        [Key(2)]
        public string Code { get; set; }

        /// <summary>
        /// Member count.
        /// </summary>
        [Key(3)]
        public short MemberCount { get; set; }

        /// <summary>
        /// Game played count.
        /// </summary>
        [Key(4)]
        public int GamePlayedCount { get; set; }

        /// <summary>
        /// Indicating if requester user is admin of room.
        /// </summary>
        [Key(5)]
        public bool IsAdmin { get; set; }
    }
}