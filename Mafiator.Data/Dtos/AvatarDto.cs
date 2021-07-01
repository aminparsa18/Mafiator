using MessagePack;

namespace Mafiator.Data.Dtos
{
   [MessagePackObject()]
   public class AvatarDto
    {
        [Key(0)]
        public string Name { get; set; }
    }
}
