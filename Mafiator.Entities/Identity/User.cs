using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Mafiator.Entities.Identity;

/// <summary>
/// User.
/// </summary>
public sealed class User : IdentityUser<Guid>
{
    /// <summary>
    /// User key identifier.
    /// </summary>
    public override Guid Id { get; set; }

    /// <summary>
    /// Country code.
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// User code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Indicates if user is suspended.
    /// </summary>
    public bool IsSuspended { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Score.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Collection of event joins.
    /// </summary>
    public ICollection<EventJoin> EventJoin { get; set; }

    /// <summary>
    /// Collection of game members.
    /// </summary>
    public ICollection<GameMember> GameMember { get; set; }

    /// <summary>
    /// Collection of game messages.
    /// </summary>
    public ICollection<GameMessage> GameMessage { get; set; }

    /// <summary>
    /// Collection of chat messages.
    /// </summary>
    public ICollection<ChatMessage> ChatMessage{ get; set; }

    /// <summary>
    /// Collection of refresh tokens.
    /// </summary>
    public ICollection<RefreshToken> RefreshToken { get; set; }

    /// <summary>
    /// Collection of reporters.
    /// </summary>
    public ICollection<GameViolationReport> Reporter { get; set; }

    /// <summary>
    /// Collectio of reported.
    /// </summary>
    public ICollection<GameViolationReport> Reported { get; set; }

    /// <summary>
    /// Collectio of rooms.
    /// </summary>
    public ICollection<Room> Room { get; set; }

    /// <summary>
    /// Collection of room members.
    /// </summary>
    public ICollection<RoomMember> RoomMember { get; set; }

    /// <summary>
    /// Collection of user roles.
    /// </summary>
    public ICollection<UserRole> Roles { get; set; }

    /// <summary>
    /// Collection of user claims.
    /// </summary>
    public ICollection<UserClaim> Claims { get; set; }
}