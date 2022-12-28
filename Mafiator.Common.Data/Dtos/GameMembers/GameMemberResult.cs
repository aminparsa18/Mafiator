using Mafiator.Common.Data.Enums;
using MemoryPack;

namespace Mafiator.Common.Data.Dtos.GameMembers;

/// <summary>
/// Game member dto.
/// </summary>
[MemoryPackable]
public sealed partial class GameMemberResult
{
    /// <summary>
    /// Game member key identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Player score.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    ///Player status.
    /// </summary>
    public PlayerStatus Status { get; set; }
}