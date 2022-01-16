using MessagePack;

namespace MafiatorApp.Dtos.User
{
   [MessagePackObject]
   public class RegisterUserDto
    {
        [Key(0)]
        public string PhoneNumber { get; set; }
        [Key(1)]
        public string Password { get; set; }
        [Key(2)]
        public string Username{ get; set; }
        [Key(3)]
        public string CountryCode { get; set; }
    }
}
