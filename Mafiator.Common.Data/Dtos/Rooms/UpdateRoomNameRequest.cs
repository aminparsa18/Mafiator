using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Rooms
{
    /// <summary>
    /// Update room image dto.
    /// </summary>
    [MessagePackObject]
    public sealed class UpdateRoomNameRequest
    {
        /// <summary>
        /// Room key identifier.
        /// </summary>
        [Key(0)]
        public Guid RoomId { get; set; }

        /// <summary>
        /// Image name.
        /// </summary>
        [Key(1)]
        public string Name { get; set; }
    }
}