using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Mafiator.Data.Dtos.User
{
    [MessagePackObject]
    public class UserLoginDto
    {
        [MessagePack.Key(0)]
        [Required] 
        public string Username { get; set; }
        [MessagePack.Key(1)]
        [Required]
        public string Password { get; set; }
    }
}