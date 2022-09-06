using Mafiator.Common.Enums;

namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Game role dto.
/// </summary>
[MessagePackObject()]
public class GameRoleDto
{
    /// <summary>
    /// Game role.
    /// </summary>
    [Key(0)] 
    public GameRole Role { get; set; }

    /// <summary>
    /// Count.
    /// </summary>
    [Key(1)] 
    public short Count { get; set; }
}