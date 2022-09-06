using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities;

/// <summary>
/// Room.
/// </summary>
public class Room:BaseEntity
{
    /// <summary>
    /// Room name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Room owner.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Room code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Room country.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Indicates room is private (joinable only with invitation).
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public virtual User User { get; set; }

    /// <summary>
    /// Collection of games.
    /// </summary>
    public virtual ICollection<Game> Game { get; set; }

    /// <summary>
    /// Collection of chat messages.
    /// </summary>
    public virtual ICollection<ChatMessage> ChatMessage{ get; set; }

    /// <summary>
    /// Collection of room members.
    /// </summary>
    public virtual ICollection<RoomMember> RoomMember { get; set; }
}