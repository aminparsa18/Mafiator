using System;
using Mafiator.Common.Helpers;
using MessagePack;
using RepoDb.Attributes;

namespace Mafiator.Data.Dtos
{
    [MessagePackObject()]
    public class RoomDto
    {
        [PropertyHandler(typeof(UlidPropertyHandler))]
        [Key(0)] public Ulid Id { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string Code { get; set; }
        [Key(3)] public short MemberCount { get; set; }
        [Key(4)] public int GamePlayedCount { get; set; }
        [Key(5)] public bool IsAdmin{ get; set; }
    }
}