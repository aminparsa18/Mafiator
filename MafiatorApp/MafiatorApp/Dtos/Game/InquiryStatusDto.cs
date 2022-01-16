using MessagePack;

namespace MafiatorApp.Dtos.Game
{
    [MessagePackObject()]
    public class InquiryStatusDto
    {
        [Key(0)]
        public bool IsMafia { get; set; }
    }
}
