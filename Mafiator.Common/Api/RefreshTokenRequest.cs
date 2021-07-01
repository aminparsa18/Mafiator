using MessagePack;
namespace Mafiator.Common.Api
{
    [MessagePackObject()]
   public class RefreshTokenRequest
    {
        [Key(0)]
        public string Token { get; set; }
        [Key(1)]
        public string RefreshToken { get; set; }
    }
}
