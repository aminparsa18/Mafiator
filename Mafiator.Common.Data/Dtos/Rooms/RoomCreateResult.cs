namespace Mafiator.Common.Data.Dtos.Rooms;

/// <summary>
/// Room create result dto.
/// </summary>
[MessagePackObject]
public class RoomCreateResult
{
    /// <summary>
    /// Room key identifier.
    /// </summary>
    [Key(0)]
    public Guid Id { get; set; }
}