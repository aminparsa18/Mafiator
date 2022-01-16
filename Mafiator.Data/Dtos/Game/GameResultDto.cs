using MessagePack;
using System;

namespace Mafiator.Data.Dtos.Game
{
    [MessagePackObject]
    public class GameResultDto
    {
        [Key(0)]
        public Guid Id { get; set; }
    }
}
