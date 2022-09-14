using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.RoomMembers
{
    /// <summary>
    /// Room member dto.
    /// </summary>
    [MessagePackObject]
    public sealed class RoomMemberResult
    {
        /// <summary>
        /// Room member user key identifier.
        /// </summary>
        [Key(0)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [Key(1)] public string Name { get; set; }

        /// <summary>
        /// Image.
        /// </summary>
        [Key(2)] public string Image { get; set; }

        /// <summary>
        /// Total game played count.
        /// </summary>
        [Key(3)] public int TotalGame { get; set; }

        /// <summary>
        /// Member level.
        /// </summary>
        [Key(4)] public int Level { get; set; }
    }
}