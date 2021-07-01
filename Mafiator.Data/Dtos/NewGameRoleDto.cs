using Mafiator.Entities.Enums;
using MessagePack;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class NewGameRoleDto
    {
        [Key(0)]
        public GameRole Role { get; set; }
        [Key(1)]
        public string Image { get; set; }
    }
}
