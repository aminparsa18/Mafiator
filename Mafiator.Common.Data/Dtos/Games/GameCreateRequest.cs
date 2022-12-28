using MemoryPack;
using System;
using System.Collections.Generic;

namespace Mafiator.Common.Data.Dtos.Games;

/// <summary>
/// Game create dto.
/// </summary>
[MemoryPackable]
public sealed partial class GameCreateRequest
{
    /// <summary>
    /// Start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Game roles.
    /// </summary>
    public List<GameRoleCreateRequest> Roles { get; set; }

    /// <summary>
    /// Room key identifier.
    /// </summary>
    public Guid RoomId { get; set; }
}