using System;

namespace Mafiator.Data.Dtos.Room;

/// <summary>
/// Room member dto.
/// </summary>
[MessagePackObject]
public class RoomMemberDto
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