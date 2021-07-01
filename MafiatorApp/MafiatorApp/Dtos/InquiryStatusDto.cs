using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
    public class InquiryStatusDto
    {
        [Key(0)]
        public bool IsMafia { get; set; }
    }
}
