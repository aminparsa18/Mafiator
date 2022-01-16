using Mafiator.Common.Helpers;
using MessagePack;
using RepoDb.Attributes;
using System;

namespace Mafiator.Data.Dtos.Room
{
    [MessagePackObject()]
    public class RoomDto
    {
        [PropertyHandler(typeof(GuidPropertyHandler))]
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string Code { get; set; }
        [Key(3)] public short MemberCount { get; set; }
        [Key(4)] public int GamePlayedCount { get; set; }
        [Key(5)] public bool IsAdmin{ get; set; }
    }
}