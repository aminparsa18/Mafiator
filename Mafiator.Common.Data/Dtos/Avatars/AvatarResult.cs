using MessagePack;

namespace Mafiator.Common.Data.Dtos.Avatars
{
    [MessagePackObject()]
    public sealed class AvatarResult
    {
        [Key(0)]
        public string Name { get; set; }
    }
}