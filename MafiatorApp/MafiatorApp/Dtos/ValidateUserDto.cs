using System;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject()]
    public class ValidateUserDto
    {
        [Key(0)]
        public Ulid Id { get; set; }
        [Key(1)]
        public string DisplayName { get; set; }
        [Key(2)]
        public string Image { get; set; }
    }
}
