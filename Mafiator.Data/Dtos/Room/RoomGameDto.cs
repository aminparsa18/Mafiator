using Mafiator.Common.Enums;
using System;

namespace Mafiator.Data.Dtos.Room;

/// <summary>
/// Room game dto.
/// </summary>
[MessagePackObject]
public class RoomGameDto
{
    /// <summary>
    /// Game start date.
    /// </summary>
    [Key(0)] 
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Game end date.
    /// </summary>
    [Key(1)] 
    public GameStatus Status { get; set; }
}