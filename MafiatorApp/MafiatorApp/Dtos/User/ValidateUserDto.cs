using MessagePack;
using System;

namespace MafiatorApp.Dtos.User
{
    [MessagePackObject()]
    public class ValidateUserDto
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string DisplayName { get; set; }
        [Key(2)]
        public string Image { get; set; }
    }
}
