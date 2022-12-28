using MessagePack;

namespace Mafiator.Benchmark.Models;

[MessagePackObject()]
public sealed class MessagePackObject
{
    [Key(0)]
    public string Name { get; set; }
}