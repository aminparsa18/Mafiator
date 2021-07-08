using System;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
   public class GemDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
        [Key(1)]
        public int Count { get; set; }
        [Key(2)]
        public int Price { get; set; }
        [Key(3)]
        public string Image { get; set; }
    }
}
