namespace Mafiator.Common.Data.Dtos.Games;

/// <summary>
/// Game create dto.
/// </summary>
[MessagePackObject]
public record GameCreateRequest
{
    /// <summary>
    /// Start date.
    /// </summary>
    [Key(0)]
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Game roles.
    /// </summary>
    [Key(1)]
    public List<GameRoleCreateRequest> Roles { get; init; }

    /// <summary>
    /// Room key identifier.
    /// </summary>
    [Key(2)]
    public Guid RoomId { get; init; }
}