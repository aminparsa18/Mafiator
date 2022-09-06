using System;

namespace Mafiator.Data.Dtos.Room;

/// <summary>
/// Room identifier dto.
/// </summary>
[MessagePackObject]
public class RoomIdDto
{
    /// <summary>
    /// Room key identifier.
    /// </summary>
    [Key(0)] 
    public Guid Id { get; set; }
}