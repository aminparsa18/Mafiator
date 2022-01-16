using MessagePack;

namespace MafiatorApp.Dtos.User
{
    [MessagePackObject]
    public class UserLoginDto
    {
        [Key(0)] public string Username { get; set; }
        [Key(1)] public string Password { get; set; }
    }
}