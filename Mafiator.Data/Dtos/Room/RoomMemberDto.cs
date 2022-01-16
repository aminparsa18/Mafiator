using Mafiator.Common.Helpers;
using MessagePack;
using RepoDb.Attributes;
using System;

namespace Mafiator.Data.Dtos.Room
{
    [MessagePackObject]
    public class RoomMemberDto
    {
        [Key(0)]
        [PropertyHandler(typeof(GuidPropertyHandler))]
        public Guid UserId { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string Image { get; set; }
        [Key(3)] public int TotalGame { get; set; }
        [Key(4)] public int Level { get; set; }
    }
}