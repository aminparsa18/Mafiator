using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject]
   public class RegisterUserDto
    {
        [Key(0)]
        public string DisplayName { get; set; }
        [Key(1)]
        public string PhoneNumber { get; set; }
        [Key(2)]
        public string Password { get; set; }
        [Key(3)]
        public string Username{ get; set; }
        [Key(4)]
        public string CountryCode { get; set; }
    }
}
