using Mafiator.Common.Helpers;
using MessagePack;
using RepoDb.Attributes;
using System;

namespace Mafiator.Data.Dtos.User
{
    [MessagePackObject()]
    public class ValidateUserDto
    {
        [Key(0)]
        [PropertyHandler(typeof(GuidPropertyHandler))]
        public Guid Id { get; set; }
        [Key(1)]
        public string DisplayName { get; set; }
        [Key(2)]
        public string Image { get; set; }
    }
}
