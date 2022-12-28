using Mafiator.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities.Models;

/// <summary>
/// Room.
/// </summary>
public sealed class Room : BaseEntity
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
    public User User { get; set; }

    /// <summary>
    /// Collection of games.
    /// </summary>
    public ICollection<Game> Game { get; set; }

    /// <summary>
    /// Collection of chat messages.
    /// </summary>
    public ICollection<ChatMessage> ChatMessage { get; set; }

    /// <summary>
    /// Collection of room members.
    /// </summary>
    public ICollection<RoomMember> RoomMember { get; set; }
}