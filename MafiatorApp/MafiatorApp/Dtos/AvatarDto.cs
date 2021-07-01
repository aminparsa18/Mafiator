using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
   public class AvatarDto
    {
        [Key(0)]
        public string Name { get; set; }
       
    }
}
