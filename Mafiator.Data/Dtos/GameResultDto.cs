using System;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject]
    public class GameResultDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
    }
}
