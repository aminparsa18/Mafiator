using MessagePack;
using System;

namespace Mafiator.Common.Data.Dtos.Rooms
{
    /// <summary>
    /// Leave game dto.
    /// </summary>
    [MessagePackObject]
    public sealed class LeaveRoomRequest
    {
        // Room key identifier.
        [Key(0)]
        public Guid RoomId { get; set; }
    }
}