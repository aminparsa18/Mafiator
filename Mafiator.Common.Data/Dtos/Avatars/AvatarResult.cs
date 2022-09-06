namespace Mafiator.Common.Data.Dtos.Avatars;

[MessagePackObject()]
public record AvatarResult
{
    [Key(0)]
    public string Name { get; set; }
}