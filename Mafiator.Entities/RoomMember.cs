using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Entities;

/// <summary>
/// Room member.
/// </summary>
public class RoomMember:BaseEntity
{
    /// <summary>
    /// Room key identifier.
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Room member user key identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Room.
    /// </summary>
    public virtual Room Room { get; set; }
    
    /// <summary>
    /// User.
    /// </summary>
    public virtual User User { get; set; }
}