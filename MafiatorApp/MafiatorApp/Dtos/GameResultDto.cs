using System;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject]
    public class GameResultDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
    }
}
