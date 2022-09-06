using System;

namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Game creation result dto.
/// </summary>
[MessagePackObject]
public class GameResultDto
{
    /// <summary>
    /// Game key identifier.
    /// </summary>
    [Key(0)]
    public Guid Id { get; set; }
}