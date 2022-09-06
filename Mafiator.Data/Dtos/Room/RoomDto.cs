using Mafiator.Common.Helpers;
using RepoDb.Attributes;
using System;

namespace Mafiator.Data.Dtos.Room;

/// <summary>
/// Room dto.
/// </summary>
[MessagePackObject()]
public class RoomDto
{
    /// <summary>
    /// Room key identifier.
    /// </summary>
    [PropertyHandler(typeof(GuidPropertyHandler))]
    [Key(0)] public Guid Id { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    [Key(1)] public string Name { get; set; }

    /// <summary>
    /// Code.
    /// </summary>
    [Key(2)] public string Code { get; set; }

    /// <summary>
    /// Member count.
    /// </summary>
    [Key(3)] public short MemberCount { get; set; }

    /// <summary>
    /// Game played count.
    /// </summary>
    [Key(4)] public int GamePlayedCount { get; set; }

    /// <summary>
    /// Indicating if requester user is admin of room.
    /// </summary>
    [Key(5)] public bool IsAdmin{ get; set; }
}