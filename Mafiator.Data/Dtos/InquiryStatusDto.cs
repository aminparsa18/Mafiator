using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class InquiryStatusDto
    {
        [Key(0)]
        public bool IsMafia { get; set; }
    }
}
