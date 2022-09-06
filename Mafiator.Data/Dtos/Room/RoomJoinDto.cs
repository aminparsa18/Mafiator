namespace Mafiator.Data.Dtos.Room;

/// <summary>
/// Room join dto.
/// </summary>
[MessagePackObject()]
public class RoomJoinDto
{
    /// <summary>
    /// Room code.
    /// </summary>
    [Key(0)] 
    public string Code { get; set; }
}