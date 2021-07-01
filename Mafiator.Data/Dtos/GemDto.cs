using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
   public class GemDto
    {
        [Key(0)]
        public int Count { get; set; }
        [Key(1)]
        public int Price { get; set; }
        [Key(2)]
        public string Image { get; set; }
    }
}
