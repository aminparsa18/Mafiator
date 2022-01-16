using MessagePack;

namespace Mafiator.Data.Dtos.User
{
   [MessagePackObject]
   public class UserDto
    {
        [Key(0)]
        public string DisplayName { get; set; }
        [Key(1)]
        public string Image { get; set; }
        [Key(2)]
        public int Score { get; set; }
        [Key(3)]
        public string CountryCode { get; set; }
    }
}
