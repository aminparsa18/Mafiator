using MessagePack;
using System;

namespace MafiatorApp.Dtos.Room
{
    [MessagePackObject]
    public class RoomDto
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string Code { get; set; }
        [Key(3)] public short MemberCount { get; set; }
        [Key(4)] public int GamePlayedCount { get; set; }
        [Key(5)] public bool IsAdmin { get; set; }
    }
}