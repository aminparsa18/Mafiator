using System.ComponentModel.DataAnnotations;
using MessagePack;

namespace Mafiator.Data.Dtos
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