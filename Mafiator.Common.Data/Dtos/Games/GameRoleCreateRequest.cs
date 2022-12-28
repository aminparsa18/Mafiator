using Mafiator.Common.Data.Enums;
using MemoryPack;

namespace Mafiator.Common.Data.Dtos.Games;

/// <summary>
/// Game role dto.
/// </summary>
[MemoryPackable]
public sealed partial class GameRoleCreateRequest
{
    /// <summary>
    /// Game role.
    /// </summary>
    public GameRole Role { get; set; }

    /// <summary>
    /// Count.
    /// </summary>
    public short Count { get; set; }
}