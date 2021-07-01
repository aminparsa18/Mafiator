using MessagePack;

namespace Mafiator.Api.Models
{
    [MessagePackObject()]
    public class Turn
    {
        [Key(0)]
        public short Index { get; set; }
    }
}
