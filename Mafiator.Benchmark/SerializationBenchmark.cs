using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Mafiator.Common.Data.Dtos.Avatars;
using MessagePack;
using System.Text.Json;

namespace Mafiator.Benchmark;

[SimpleJob(RunStrategy.ColdStart, launchCount: 30)]
[MemoryDiagnoser(false)]
public class SerializationBenchmark
{
    private List<AvatarResult>? _mockAvatarData;
    
    [GlobalSetup]
    public void Setup()
    {
        _mockAvatarData = new List<AvatarResult>()
        {
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
            new AvatarResult { Name = "Avatar test data" },
        };
    }

    [Benchmark]
    public void SerializeJson()
    {
        JsonSerializer.Serialize(_mockAvatarData);
    }

    [Benchmark]
    public void SerializeMessagePack()
    {
        MessagePackSerializer.Serialize(_mockAvatarData, options: new MessagePackSerializerOptions(new CustomMessagePackResolver())
        {
        });
    }
}