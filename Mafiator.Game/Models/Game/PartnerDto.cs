using Mafiator.Common.Data.Enums;

namespace Mafiator.Game.Models.Game;

public sealed class PartnerDto
{
    public GameRole Role { get; set; }
    public string Name { get; set; }
}