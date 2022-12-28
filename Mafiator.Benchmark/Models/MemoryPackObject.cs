using MemoryPack;

namespace Mafiator.Benchmark.Models;

[MemoryPackable]
public sealed partial class MemoryPackObject
{
    public string Name { get; set; }
}