using Mafiator.Common.Data.Enums;

namespace Mafiator.Common.Data.Dtos.Games;

/// <summary>
/// Game role dto.
/// </summary>
[MessagePackObject()]
public record GameRoleCreateRequest
{
    /// <summary>
    /// Game role.
    /// </summary>
    [Key(0)]
    public GameRole Role { get; init; }

    /// <summary>
    /// Count.
    /// </summary>
    [Key(1)]
    public short Count { get; init; }
}