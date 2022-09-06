namespace Mafiator.Data.Dtos.Game;

/// <summary>
/// Detective inquiry status dto.
/// </summary>
[MessagePackObject()]
public class InquiryStatusDto
{
    /// <summary>
    /// Indicating player is mafia.
    /// </summary>
    [Key(0)]
    public bool IsMafia { get; set; }
}