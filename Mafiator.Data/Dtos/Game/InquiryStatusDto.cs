using MessagePack;

namespace Mafiator.Data.Dtos.Game
{
    [MessagePackObject()]
    public class InquiryStatusDto
    {
        [Key(0)]
        public bool IsMafia { get; set; }
    }
}
