using Mafiator.Common.Data.Enums;

namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Game member dto.
/// </summary>
[MessagePackObject()]
public class GameMemberDto
{
    /// <summary>
    /// Game member key identifier.
    /// </summary>
    [Key(0)]
    public string Id{ get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    [Key(1)]
    public string DisplayName { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    [Key(2)]
    public string Image { get; set; }

    /// <summary>
    /// Player score.
    /// </summary>
    [Key(3)]
    public int Score { get; set; }

    /// <summary>
    ///Player status.
    /// </summary>
    [Key(4)]
    public PlayerStatus Status { get; set; }
}