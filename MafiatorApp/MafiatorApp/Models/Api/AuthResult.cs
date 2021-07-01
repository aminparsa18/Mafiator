using MessagePack;

namespace MafiatorApp.Models.Api
{
    [MessagePackObject]
    public class AuthResult:ApiResult
    {
        [Key(3)]
        public string Token { get; set; }
        [Key(4)]
        public string RefreshToken { get; set; }
    }
}
