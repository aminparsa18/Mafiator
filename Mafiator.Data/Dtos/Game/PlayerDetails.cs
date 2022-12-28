using Mafiator.Common.Data.Enums;

namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Player dto.
/// </summary>
[MemoryPackable]
public sealed partial class PlayerDetails
{
    /// <summary>
    /// Game member key identifier.
    /// </summary>
    public string MemberId { get; set; }

    /// <summary>
    /// Game member user key identifier.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Player game role.
    /// </summary>
    public GameRole Role{ get; set; }

    /// <summary>
    /// Player status.
    /// </summary>
    public PlayerStatus Status{ get; set; }
}