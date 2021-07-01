using System;
using Mafiator.Common.Helpers;
using MessagePack;
using RepoDb.Attributes;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class ValidateUserDto
    {
        [Key(0)]
        [PropertyHandler(typeof(UlidPropertyHandler))]
        public Ulid Id { get; set; }
        [Key(1)]
        public string DisplayName { get; set; }
        [Key(2)]
        public string Image { get; set; }
    }
}
