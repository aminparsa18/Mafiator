using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Mafiator.Data.Dtos.User
{
    [MessagePackObject]
    public class ConfirmPhoneDto
    {
        [MessagePack.Key(0)]
        [Required]
        public string PhoneNo{ get; set; }
        [MessagePack.Key(1)]
        [Required]
        public string Token { get; set; }
    }
}
