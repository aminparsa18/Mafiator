using MessagePack;
using System;

namespace MafiatorApp.Dtos.Room
{
    [MessagePackObject]
    public class RoomMemberDto
    {
        [Key(0)] public Guid UserId { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string Image { get; set; }
        [Key(3)] public int TotalGame { get; set; }
        [Key(4)] public int Level { get; set; }
    }
}