using MessagePack;

namespace Mafiator.Data.Dtos.User
{
    [MessagePackObject()]
    public class UpdateProfileDto
    {
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public string Image { get; set; }
    }
}
