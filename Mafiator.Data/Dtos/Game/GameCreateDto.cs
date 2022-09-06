using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Game create dto.
/// </summary>
[MessagePackObject]
public class GameCreateDto
{
    /// <summary>
    /// Start date.
    /// </summary>
    [Key(0)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Game roles.
    /// </summary>
    [Key(1)]
    public List<GameRoleDto> Roles { get; set; }

    /// <summary>
    /// Room key identifier.
    /// </summary>
    [Key(2)]
    public Guid RoomId { get; set; }
}