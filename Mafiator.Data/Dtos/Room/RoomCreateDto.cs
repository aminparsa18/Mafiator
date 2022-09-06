using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos.Room;

/// <summary>
/// Room create dto.
/// </summary>
[MessagePackObject]
public class RoomCreateDto
{
    /// <summary>
    /// Room name.
    /// </summary>
    [Key(0)]
    public string Name { get; set; }

    /// <summary>
    /// Indicating room is private.
    /// </summary>
    [Key(1)]
    public bool IsPrivate { get; set; }

    /// <summary>
    /// List of users.
    /// </summary>
    [Key(2)]
    public List<Guid> Users{ get; set; }

    /// <summary>
    /// Country.
    /// </summary>
    [Key(3)]
    public string Country { get; set; }
}