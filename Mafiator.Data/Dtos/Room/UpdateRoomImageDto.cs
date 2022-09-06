using System;

namespace Mafiator.Data.Dtos.Room;

/// <summary>
/// Update room image dto.
/// </summary>
[MessagePackObject]
public class UpdateRoomImageDto
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