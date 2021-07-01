using System;
using MessagePack;

namespace MafiatorApp.Dtos
{
    [MessagePackObject]
    public class RoomMemberDto
    {
        [Key(0)] public Ulid UserId { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string Image { get; set; }
        [Key(3)] public int TotalGame { get; set; }
        [Key(4)] public int Level { get; set; }
    }
}