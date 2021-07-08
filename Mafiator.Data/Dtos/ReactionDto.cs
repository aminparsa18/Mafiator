using System;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class ReactionDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
        [Key(1)]
        public string Title { get; set; }
        [Key(2)]
        public string Image { get; set; }
    }
}
