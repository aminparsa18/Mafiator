using MemoryPack;

namespace Mafiator.Common.Data.Dtos.GameMembers;

/// <summary>
/// Waiting game member dto.
/// </summary>
[MemoryPackable]
public sealed partial class WaitingPlayerResult
{
    /// <summary>
    /// Game member user key identifier.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Score.
    /// </summary>
    public int Score { get; set; }
}