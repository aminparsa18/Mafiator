using MemoryPack;

namespace Mafiator.Common.Data.Dtos.Users;

/// <summary>
/// User dto.
/// </summary>
[MemoryPackable]
public sealed partial class UserDetailsResult
{
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

    /// <summary>
    /// Country code.
    /// </summary>
    public string CountryCode { get; set; }
}