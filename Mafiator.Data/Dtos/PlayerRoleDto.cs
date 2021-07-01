using Mafiator.Entities.Enums;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class PlayerRoleDto
    {
        [Key(0)]
        public GameRole Role { get; set; }
        [Key(1)]
        public string MemberId{ get; set; }
    }
}
