
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
   public class VoteStatusDto
    {
        [Key(0)]
        public string VoterId { get; set; }
        [Key(1)]
        public string TargetId { get; set; }
    }
}
